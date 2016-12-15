using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace NET.Framework.Common.LogHelper
{
    public interface ILogger
    {
        //private string Name;

        #region Debug

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">The debug message.</param>
        void Debug(string message);

        #endregion

        #region Info

        /// <summary>
        /// Log an info message.
        /// </summary>
        /// <param name="message">The info message.</param>
        void Info(string message);

        #endregion

        #region Warn

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        void Warn(string message);

        #endregion

        #region Error

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        void Error(string message);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="ex">The exception message.</param>
        void Error(string message, Exception ex);

        #endregion

        #region Fatal

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="message">The fatal message.</param>
        void Fatal(string message);

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="message">The fatal message.</param>
        /// <param name="ex">The exception message.</param>
        void Fatal(string message, Exception ex);

        #endregion


        #region Checkers

        /// <summary>
        /// Record the fatal level of the log.
        /// </summary>
        /// <returns>true if this error is fatal level, otherwise false</returns>
        bool IsFatalEnabled();

        /// <summary>
        /// Record the error level of the log.
        /// </summary>
        /// <returns>true if this error is error level, otherwise false</returns>
        bool IsErrorEnabled();

        /// <summary>
        /// Record the info level of the log.
        /// </summary>
        /// <returns>true if this error is info level, otherwise false</returns>
        bool IsInfoEnabled();

        /// <summary>
        /// Record the warning level of the log.
        /// </summary>
        /// <returns>true if this error is warning level, otherwise false</returns>
        bool IsWarnEnabled();

        /// <summary>
        /// Record the debug level of the log.
        /// </summary>
        /// <returns>true if this error is debug level, otherwise false</returns>
        bool IsDebugEnabled();

        #endregion
    }
}
