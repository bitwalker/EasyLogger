using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Web.Services.Protocols;

using NLog;

namespace EasyLogger
{
    public enum EventType
    {
        Trace,
        Debug,
        Informational,
        Warning,
        Error,
        Fatal
    }

    public class Log
    {
        private static readonly object _lock = new object();

        public Logger Logger { get; private set; }
        public Type LoggedType { get; private set; }
        public bool LogToFile { get; private set; }
        public string LogPath { get; private set; }

        private Log()
        {
            
        }

        public static Log Initialize(Type loggingType)
        {
            var log = new Log
                      {
                          Logger = LogManager.GetLogger(loggingType.Name),
                          LogToFile = false
                      };
            return log;
        }

        public static Log Initialize(Type loggingType, string logFile)
        {
            Contract.Requires(logFile.NotEmpty());

            var log = new Log
                      {
                          Logger = LogManager.GetLogger(loggingType.Name),
                          LoggedType = loggingType,
                          LogToFile = true,
                          LogPath = logFile
                      };

            // Ensure that the directory this log is being initialized against exists
            string directory = Path.GetDirectoryName(logFile);
            // If this is empty/null, then it's being saved in the current directory
            if (directory.NotEmpty()) 
                // If it doesn't exist, create it
                if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(logFile));

            return log;
        }

        /// <summary>
        /// Log simple trace message.
        /// </summary>
        public void Trace(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Trace, message);
            else
                Logger.Trace(message);
        }

        /// <summary>
        /// Log a trace message by providing a format string and arguments.
        /// </summary>
        public void Trace(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Fatal, message);
                else
                    WriteToFile(EventType.Fatal, string.Format(message, args));
            else
                Logger.Trace(message, args);
        }

        /// <summary>
        /// Log a trace level exception.
        /// </summary>
        public void Trace(Exception ex)
        {
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof(SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Fatal, message.ToString());
            }
            else
                Logger.TraceException("Exception occurred.", ex);
        }

        /// <summary>
        /// Log a trace message with additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Trace, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Trace,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a trace message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Trace, message);
                else
                    WriteToFile(EventType.Trace, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Trace,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a trace exception with additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof(SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Trace, message.ToString());
            }
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Trace,
                                    LoggerName = Logger.Name,
                                    Message = "Exception occurred.",
                                    Exception = ex,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log simple debug message.
        /// </summary>
        public void Debug(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Debug, message);
            else
                Logger.Debug(message);
        }

        /// <summary>
        /// Log a debug message by providing a format string and arguments.
        /// </summary>
        public void Debug(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Debug, message);
                else
                    WriteToFile(EventType.Debug, string.Format(message, args));
            else
                Logger.Debug(message, args);
        }

        /// <summary>
        /// Log a debug message with additional event context data.
        /// </summary>
        public void Debug(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Debug, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Debug,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a debug message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Debug(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Debug, message);
                else
                    WriteToFile(EventType.Debug, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Debug,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log simple informational message.
        /// </summary>
        public void Info(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Informational, message);
            else
                Logger.Info(message);
        }

        /// <summary>
        /// Log an informational message by providing a format string and arguments.
        /// </summary>
        public void Info(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Informational, message);
                else
                    WriteToFile(EventType.Informational, string.Format(message, args));
            else
                Logger.Info(message, args);
        }

        /// <summary>
        /// Log a informational message with additional event context data.
        /// </summary>
        public void Info(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Informational, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Info,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a informational message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Info(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Informational, message);
                else
                    WriteToFile(EventType.Informational, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Info,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log simple warning message.
        /// </summary>
        public void Warning(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Warning, message);
            else
                Logger.Warn(message);
        }

        /// <summary>
        /// Log a warning message by providing a format string and arguments.
        /// </summary>
        public void Warning(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Warning, message);
                else
                    WriteToFile(EventType.Warning, string.Format(message, args));
            else
                Logger.Warn(message, args);
        }

        /// <summary>
        /// Log a warning error message with additional event context data.
        /// </summary>
        public void Warning(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Warning, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Warn,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a warning error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Warning(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Warning, message);
                else
                    WriteToFile(EventType.Warning, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Warn,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log simple error message.
        /// </summary>
        public void Error(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Error, message);
            else
                Logger.Error(message);
        }

        /// <summary>
        /// Log an error message by providing a format string and arguments.
        /// </summary>
        public void Error(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Error, message);
                else
                    WriteToFile(EventType.Error, string.Format(message, args));
            else
                Logger.Error(message, args);
        }

        /// <summary>
        /// Log an exception message containing error, stack trace, and nested exceptions
        /// </summary>
        public void Error(Exception ex)
        {
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof (SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Error, message.ToString());
            }
            else
                Logger.ErrorException("Exception occurred.", ex);
        }

        /// <summary>
        /// Log an error message with additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Error, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Error,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log an error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Error, message);
                else
                    WriteToFile(EventType.Error, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Error,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log an error exception with additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof(SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Error, message.ToString());
            }
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Error,
                                    LoggerName = Logger.Name,
                                    Message = "Exception occurred.",
                                    Exception = ex,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a fatal error message.
        /// </summary>
        public void Fatal(string message)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Fatal, message);
            else
                Logger.Fatal(message);
        }

        /// <summary>
        /// Log a fatal error message with provided format string and arguments.
        /// </summary>
        public void Fatal(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Fatal, message);
                else
                    WriteToFile(EventType.Fatal, string.Format(message, args));
            else
                Logger.Fatal(message, args);
        }

        /// <summary>
        /// Log a fatal exception.
        /// </summary>
        public void Fatal(Exception ex)
        {
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof(SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Fatal, message.ToString());
            }
            else
                Logger.FatalException("Exception occurred.", ex);
        }

        /// <summary>
        /// Log a fatal error message with additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                WriteToFile(EventType.Fatal, message);
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Fatal,
                                    LoggerName = Logger.Name,
                                    Message = message,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a fatal error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            if (LogToFile)
                if (args == null)
                    WriteToFile(EventType.Fatal, message);
                else
                    WriteToFile(EventType.Fatal, string.Format(message, args));
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Fatal,
                                    LoggerName = Logger.Name,
                                    Message = args == null ? message : string.Format(message, args),
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Log a fatal exception with additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            if (LogToFile)
            {
                var message = new StringBuilder();

                if (ex.GetType() == typeof(SoapException))
                {
                    var soapEx = (SoapException) ex;
                    var formatted = string.Format("Actor: {0} - Code {1} - Sub Code {2} - Detail {3}", soapEx.Actor, soapEx.Code, soapEx.SubCode, soapEx.Detail.InnerXml);
                    message.AppendLine(formatted);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }
                else
                {
                    message.AppendLine(ex.GetType() + ": " + ex.Message);
                    message.AppendLine();
                    message.AppendLine(ex.StackTrace);
                }

                GetInnerExceptions(ex, ref message);

                WriteToFile(EventType.Fatal, message.ToString());
            }
            else
            {
                var eventInfo = new LogEventInfo
                                {
                                    Level = LogLevel.Fatal,
                                    LoggerName = Logger.Name,
                                    Message = "Exception occurred.",
                                    Exception = ex,
                                    TimeStamp = DateTime.Now
                                };
                eventContextData.Do(eventInfo.Properties.Add);
                Logger.Log(eventInfo);
            }
        }

        /// <summary>
        /// Will recursively walk a tree of exceptions and use the ref'd StringBuilder
        /// to build a string that contains the exceptions printed in order from highest
        /// to
        ///  lowest level.
        /// </summary>
        private static void GetInnerExceptions(Exception ex, ref StringBuilder message)
        {
            Contract.Requires(ex != null);
            Contract.Requires(message != null);

            if (ex.InnerException != null)
            {
                message.AppendLine();
                message.AppendLine(ex.InnerException.Message);
                message.AppendLine(ex.InnerException.StackTrace);

                GetInnerExceptions(ex.InnerException, ref message);

                return;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Writes an entry to a provided file
        /// </summary>
        private void WriteToFile(EventType type, string message)
        {
            Contract.Requires(message.NotEmpty());

            lock (_lock)
            {
                string trace_message = string.Format("{0, 19}:    {1, -13} - {2, -15} - {3}",
                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                    Enum.GetName(typeof(EventType), type),
                                                    LoggedType.Name,
                                                    message);

                File.AppendAllText(LogPath, trace_message, Encoding.UTF8);
            }
        }
    }
}
