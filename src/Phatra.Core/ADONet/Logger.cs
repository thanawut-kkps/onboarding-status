using Phatra.Core.Logging;
using Phatra.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Phatra.Core.AdoNet
{
    /// <summary>
    /// To enable logging, set the Log property of the Logger class
    /// </summary>
    public class Logger
    {
        static Logger()
        {
            var logger = Log4NetLogger.CreateLoggerFor(typeof(Logger));
            LogDebug = (s) => { logger.Debug(s); };
            LogError = (s) => { logger.Error(s); };
        }


        public static Action<string> LogDebug;

        public static Action<string> LogError;

        internal static void LogException(Exception ex)
        {
            LogError(ex.ToString());
        }

        //#if DEBUG
        //        public static Action<string> Log = s => { Debug.WriteLine(s); };
        //#else
        //        public static Action<string> Log;
        //#endif

        internal static void LogMessage(string text)
        {
            LogDebug(text);
        }

        internal static void LogCommand(IDbCommand command)
        {
            if (LogDebug == null) return;

            LogDebug($"CommandType:'{command.CommandType}' CommandText:{command.CommandText}");
            List<string> param = new List<string>();
            foreach (IDbDataParameter p in command.Parameters)
            {
                string value = ValueString(p.Value);

                param.Add($"{p.ParameterName}={value}");
                //LogDebug($"{p.ParameterName} = {p.Value}");
            }
            LogDebug(string.Join(", ", param.ToArray()));
        }

        internal static string ValueString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "NULL";
            }
            else if (value.GetType() == typeof(string) && string.IsNullOrEmpty((string)value))
            {
                return $"NULL";
            }
            else if (value.GetType() == typeof(string))
            {
                return $"'{value}'";
            }
            else if (value.GetType() == typeof(DateTime))
            {
                return $"'{value:yyyy-MM-dd HH:mm:ss}'";
            }
            else if (value.GetType() == typeof(int) || value.GetType() == typeof(decimal))
            {
                return $"{value}";
            }
            return $"'{value}'";
        }
    }
}
