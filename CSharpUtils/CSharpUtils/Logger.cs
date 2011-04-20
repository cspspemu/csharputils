using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils
{
    public class Logger
    {
        public enum Level : int
        {
            Info = 0,
            Warning = 1,
            Error = 2,
            Fatal = 3,
        }

        public TextWriter LogTextWriter;
        public Level CurrentLogLevel;

        public Logger(Level CurrentLogLevel = Level.Warning, TextWriter LogTextWriter = null)
        {
            this.CurrentLogLevel = CurrentLogLevel;
            if (LogTextWriter == null)
            {
                LogTextWriter = Console.Out;
            }
            this.LogTextWriter = LogTextWriter;
        }

        public void Log(Level LogLevel, String Format, params Object[] Params)
        {
            if (LogLevel >= CurrentLogLevel)
            {
                LogTextWriter.WriteLine(LogLevel + "@" + Format, Params);
            }
        }
    }
}
