using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Core.Logging
{
    public class ConsoleLogger : Logger, ILogger
    {
        public void Chat(string format, params object[] args) => Write(LogType.Chat, format, args);

        public void Debug(string format, params object[] args) => Write(LogType.Debug, format, args);

        public void Info(string format, params object[] args) => Write(LogType.Info, format, args);

        public void Notice(string format, params object[] args) => Write(LogType.Notice, format, args);

        public void Warning(string format, params object[] args) => Write(LogType.Warning, format, args);

        public void Error(string format, params object[] args) => Write(LogType.Error, format, args);

        public void Exception(string message, Exception ex) => WriteException(message, ex);
    }
}
