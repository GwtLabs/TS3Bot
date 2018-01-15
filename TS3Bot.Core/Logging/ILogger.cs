using System;

namespace TS3Bot.Core.Logging
{
    public interface ILogger
    {
        void Chat(string format, params object[] args);
        void Debug(string format, params object[] args);
        void Info(string format, params object[] args);
        void Notice(string format, params object[] args);
        void Warning(string format, params object[] args);
        void Error(string format, params object[] args);
        void Exception(string message, Exception ex);
        //void Critical();
    }
}
