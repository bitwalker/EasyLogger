using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

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

    public class LoggingContext
    {
        public Type CallingType { get; set; }
        public string CallingMethodName { get; set; }

        public LoggingContext()
        {
            var s = new StackTrace();
            var frame = s.GetFrame(2);
            if (frame != null)
            {
                var method = frame.GetMethod();
                if (method != null)
                {
                    CallingType = method.DeclaringType;
                    CallingMethodName = method.Name;
                }
                else
                {
                    CallingType = typeof (TypeUnavailable);
                    CallingMethodName = "N/A";
                }
            }
        }

        public string FormatMessage(string message, params dynamic[] args)
        {
            return string.Format(string.Format("{0}: {1}", this.CallingMethodName, message), args);
        }

        public class TypeUnavailable
        {
        }
    }

    public class Log
    {
        private static readonly object _lock = new object();

        public Logger Logger { get; private set; }

        private Log(Type loggingType)
        {
            this.Logger = LogManager.GetLogger(loggingType.Name);
        }

        public static Log Get()
        {
            var context = new LoggingContext();
            return new Log(context.CallingType);
        }

        public static Log Get(Type loggingType)
        {
            return new Log(loggingType);
        }

        public static Log Get<T>()
        {
            return new Log(typeof(T));
        }

        /// <summary>
        /// Log simple trace message.
        /// </summary>
        public void Trace(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Trace(context.FormatMessage(message));
        }

        /// <summary>
        /// Log a trace message by providing a format string and arguments.
        /// </summary>
        public void Trace(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Trace(context.FormatMessage(message, args));
        }

        /// <summary>
        /// Log a trace level exception.
        /// </summary>
        public void Trace(Exception ex)
        {
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            Logger.TraceException(context.FormatMessage(ex.Message), ex);
        }

        /// <summary>
        /// Log a trace message with additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Trace,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a trace message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Trace,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a trace exception with additional event context data.
        /// </summary>
        public void Trace(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Trace,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(ex.Message),
                                Exception = ex,
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log simple debug message.
        /// </summary>
        public void Debug(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Debug(context.FormatMessage(message));
        }

        /// <summary>
        /// Log a debug message by providing a format string and arguments.
        /// </summary>
        public void Debug(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Debug(context.FormatMessage(message), args);
        }

        /// <summary>
        /// Log a debug message with additional event context data.
        /// </summary>
        public void Debug(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Debug,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a debug message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Debug(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());


            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Debug,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log simple informational message.
        /// </summary>
        public void Info(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Info(context.FormatMessage(message));
        }

        /// <summary>
        /// Log an informational message by providing a format string and arguments.
        /// </summary>
        public void Info(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Info(context.FormatMessage(message), args);
        }

        /// <summary>
        /// Log a informational message with additional event context data.
        /// </summary>
        public void Info(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Info,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a informational message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Info(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Info,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log simple warning message.
        /// </summary>
        public void Warning(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Warn(context.FormatMessage(message));
        }

        /// <summary>
        /// Log a warning message by providing a format string and arguments.
        /// </summary>
        public void Warning(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Warn(context.FormatMessage(message, args));
        }

        /// <summary>
        /// Log a warning error message with additional event context data.
        /// </summary>
        public void Warning(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Warn,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a warning error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Warning(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Warn,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log simple error message.
        /// </summary>
        public void Error(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Error(context.FormatMessage(message));
        }

        /// <summary>
        /// Log an error message by providing a format string and arguments.
        /// </summary>
        public void Error(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Error(context.FormatMessage(message, args));
        }

        /// <summary>
        /// Log an exception message containing error, stack trace, and nested exceptions
        /// </summary>
        public void Error(Exception ex)
        {
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            Logger.ErrorException(context.FormatMessage(ex.Message), ex);
        }

        /// <summary>
        /// Log an error message with additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Error,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log an error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Error,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log an error exception with additional event context data.
        /// </summary>
        public void Error(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Error,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(ex.Message),
                                Exception = ex,
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a fatal error message.
        /// </summary>
        public void Fatal(string message)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Fatal(context.FormatMessage(message));
        }

        /// <summary>
        /// Log a fatal error message with provided format string and arguments.
        /// </summary>
        public void Fatal(string message, params dynamic[] args)
        {
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            Logger.Fatal(context.FormatMessage(message, args));
        }

        /// <summary>
        /// Log a fatal exception.
        /// </summary>
        public void Fatal(Exception ex)
        {
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            Logger.FatalException(context.FormatMessage(ex.Message), ex);
        }

        /// <summary>
        /// Log a fatal error message with additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Fatal,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a fatal error message with a format string and arguments, and additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, string message, params dynamic[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Fatal,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(message, args),
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// Log a fatal exception with additional event context data.
        /// </summary>
        public void Fatal(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            var context = new LoggingContext();
            var eventInfo = new LogEventInfo
                            {
                                Level = LogLevel.Fatal,
                                LoggerName = Logger.Name,
                                Message = context.FormatMessage(ex.Message),
                                Exception = ex,
                                TimeStamp = DateTime.Now
                            };
            eventContextData.Do(eventInfo.Properties.Add);
            Logger.Log(eventInfo);
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
    }
}
