using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NET.Framework.Common.LogHelper
{
    public class LoggerManager
    {
        public static LoggerManager instance;

        /// <summary>
        /// Create a new logger instance.
        /// </summary>
        /// <returns>Return the instance of the Logger by creating the Logger.</returns>
        public static LoggerManager GetInstance()
        {
            if (null == instance)
            {
                lock (typeof(LoggerManager))
                {
                    if (null == instance)
                    {
                        instance = new LoggerManager();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Create the instance of the Logger by the Logger Type and Logger Category
        /// </summary>
        /// <param name="name">The name of logger.</param>
        /// <returns>Return the instance of the Logger by the Logger type and Logger Category.</returns>
        public static ILogger GetLogger(string name)
        {
            return new Log4NetLogger(name);
        }
    }
}
