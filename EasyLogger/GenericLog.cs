using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using NLog;

namespace EasyLogger
{
    public static class Log<T> where T : class
    {
        private static readonly Log _instance = Log.Initialize(typeof(T));

        public static Log ToFile(string path)
        {
            Contract.Requires(path.NotEmpty());

            return Log.Initialize(typeof(T), path);
        }

        public static void Trace(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Trace(message);
        }

        public static void Trace(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Trace(message, args);
        }

        public static void Trace(Exception ex)
        {
            Contract.Requires(ex != null);

            _instance.Trace(ex);
        }

        public static void Trace(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Trace(eventContextData, message);
        }

        public static void Trace(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Trace(eventContextData, message, args);
        }

        public static void Trace(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            _instance.Trace(eventContextData, ex);
        }

        public static void Debug(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Debug(message);
        }

        public static void Debug(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Debug(message, args);
        }

        public static void Debug(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Debug(eventContextData, message);
        }

        public static void Debug(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Debug(eventContextData, message, args);
        }

        public static void Info(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Info(message);
        }

        public static void Info(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Info(message, args);
        }

        public static void Info(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Info(eventContextData, message);
        }

        public static void Info(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Info(eventContextData, message, args);
        }

        public static void Warning(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Warning(message);
        }

        public static void Warning(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Warning(message, args);
        }

        public static void Warning(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Warning(eventContextData, message);
        }

        public static void Warning(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Warning(eventContextData, message, args);
        }

        public static void Error(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Error(message);
        }

        public static void Error(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Error(message, args);
        }

        public static void Error(Exception ex)
        {
            Contract.Requires(ex != null);

            _instance.Error(ex);
        }

        public static void Error(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Error(eventContextData, message);
        }

        public static void Error(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Error(eventContextData, message, args);
        }

        public static void Error(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            _instance.Error(eventContextData, ex);
        }

        public static void Fatal(string message)
        {
            Contract.Requires(message.NotEmpty());

            _instance.Fatal(message);
        }

        public static void Fatal(string message, params object[] args)
        {
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Fatal(message, args);
        }

        public static void Fatal(Exception ex)
        {
            Contract.Requires(ex != null);

            _instance.Fatal(ex);
        }

        public static void Fatal(IDictionary<object, object> eventContextData, string message)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());

            _instance.Fatal(eventContextData, message);
        }

        public static void Fatal(IDictionary<object, object> eventContextData, string message, params object[] args)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(message.NotEmpty());
            Contract.Requires(args != null);

            _instance.Fatal(eventContextData, message, args);
        }

        public static void Fatal(IDictionary<object, object> eventContextData, Exception ex)
        {
            Contract.Requires(eventContextData != null);
            Contract.Requires(ex != null);

            _instance.Fatal(eventContextData, ex);
        }
    }
}
