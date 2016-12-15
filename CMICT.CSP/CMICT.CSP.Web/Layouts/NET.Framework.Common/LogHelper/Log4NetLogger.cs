using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace NET.Framework.Common.LogHelper
{
    public class Log4NetLogger : ILogger
    {
        #region Enum

        /// <summary>
        /// Enum the Log Type.
        /// </summary>
        public enum LoggerType
        {
            /// <summary>
            /// Record the system log.
            /// </summary>
            SystemLog,

            /// <summary>
            /// Record the active log.
            /// </summary>
            ActivityLog,
        }

        #endregion

        #region Members

        private ILog log;
        private static bool hasCreate;
        private string CurrentLoggerName = string.Empty;

        #endregion

        #region Constructor

        public Log4NetLogger(string name)
        {
            //Only configure the config file one time
            if (hasCreate == false)
            {
                Configure();
                hasCreate = true;
            }
            CurrentLoggerName = name;
            log = LogManager.GetLogger(name);
        }
        #endregion

        #region Configure Log4Net Configure File
        /// <summary>
        /// Read default config
        /// </summary>
        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Set configuration to log service.
        /// </summary>
        public static void Configure(string configXml)
        {
            if (string.IsNullOrEmpty(configXml)) return;
            StringReader reader = new StringReader(configXml);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            XmlElement logElement = document["log4net"];
            log4net.Config.XmlConfigurator.Configure(logElement);
        }

        #endregion

        #region Debug
        /// <summary>
        /// Log a debug message.
        /// </summary>
        public void Debug(string message)
        {
            if (this.IsDebugEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Debug(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null));
            }
        }

        #endregion

        #region Info
        /// <summary>
        /// Log a Info message.
        /// </summary>
        public void Info(string message)
        {
            if (this.IsInfoEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Info(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null));
            }
        }

        #endregion

        #region Warn
        /// <summary>
        /// Log a Warn message.
        /// </summary>
        public void Warn(string message)
        {
            if (this.IsWarnEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Warn(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null));
            }
        }

        #endregion

        #region Error
        /// <summary>
        /// Log a Error message.
        /// </summary>
        public void Error(string message)
        {
            if (this.IsErrorEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Error(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null));
            }
        }

        /// <summary>
        /// Log a Error message.
        /// </summary>
        public void Error(string message, Exception ex)
        {
            if (this.IsErrorEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Error(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null), ex);
            }
        }

        #endregion

        #region Fatal
        /// <summary>
        /// Log a Fatal message.
        /// </summary>
        public void Fatal(string message)
        {
            if (this.IsFatalEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Fatal(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null));
            }
        }

        /// <summary>
        /// Log a Fatal message.
        /// </summary>
        public void Fatal(string message, Exception ex)
        {
            if (this.IsFatalEnabled())
            {
                StackTrace stackTrance = new StackTrace();
                StackFrame stackSelfFrame = stackTrance.GetFrame(0); //[0]self method; [1]call method
                string loggerLevel = stackSelfFrame.GetMethod().Name;
                StackFrame stackFrame = stackTrance.GetFrame(1);
                string methodName = stackFrame.GetMethod().Name;
                string className = stackFrame.GetMethod().ReflectedType.Name;

                log.Fatal(this.CreateMessage(LoggerType.SystemLog, CurrentLoggerName, loggerLevel, methodName, className, message, null, null, null), ex);
            }
        }

        #endregion

        #region Checkers

        public bool IsFatalEnabled()
        {
            return log.IsFatalEnabled;
        }

        public bool IsErrorEnabled()
        {
            return log.IsErrorEnabled;
        }

        public bool IsInfoEnabled()
        {
            return log.IsInfoEnabled;
        }

        public bool IsWarnEnabled()
        {
            return log.IsWarnEnabled;
        }

        public bool IsDebugEnabled()
        {
            return log.IsDebugEnabled;
        }

        #endregion

        #region Private Function

        /// <summary>
        /// CreateMessage
        /// </summary>
        /// <param name="loggerType">loggerType</param>
        /// <param name="loggerName">loggerName</param>
        /// <param name="loggerLevel">loggerLevel</param>
        /// <param name="methodName">methodName</param>
        /// <param name="className">className</param>
        /// <param name="finalMessage">finalMessage</param>
        /// <param name="dictionaryValues">dictionaryValues</param>
        /// <param name="paramValues">paramValues</param>
        /// <returns>Message</returns>
        private string CreateMessage(LoggerType loggerType, string loggerName, string loggerLevel, string methodName, string className, string finalMessage, Dictionary<string, string> dictionaryValues, ArrayList arraylistValues, params string[] paramValues)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendFormat("{0};", loggerType.ToString());
            logMessage.AppendFormat("{0};", loggerName.ToString());
            logMessage.AppendFormat("{0};", loggerLevel.ToString());
            logMessage.AppendFormat("{0};", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            logMessage.AppendFormat("{0};", className.ToString());
            logMessage.AppendFormat("{0};", methodName.ToString());
            if (finalMessage != null)
            {
                logMessage.AppendFormat("{0};", finalMessage.Replace(";", ","));
            }
            
            return logMessage.ToString();
        }

        #endregion
    }
}
