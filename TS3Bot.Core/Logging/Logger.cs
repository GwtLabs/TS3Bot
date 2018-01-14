using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Core.Logging
{
    public enum LogType
    {
        Chat,
        Debug,
        Info,
        Notice,
        Warning,
        Error,
    }

    public abstract class Logger
    {
        protected Logger()
        {
        }

        public virtual void Write(LogType type, string format, params object[] args)
        {
            string msg = $"[{DateTime.UtcNow}] " + string.Format(format, args);

            ConsoleColor consoleColor;

            switch (type)
            {
                case LogType.Chat:
                    consoleColor = ConsoleColor.Green;
                    break;

                case LogType.Debug:
                    consoleColor = ConsoleColor.Gray;
                    break;

                case LogType.Info:
                    consoleColor = ConsoleColor.Cyan;
                    break;

                case LogType.Notice:
                    consoleColor = ConsoleColor.Magenta;
                    break;

                case LogType.Warning:
                    consoleColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    consoleColor = ConsoleColor.Red;
                    break;

                default:
                    consoleColor = ConsoleColor.Gray;
                    break;
            }

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public virtual void WriteException(string message, Exception ex)
        {
            var outerEx = ex;
            while (ex.InnerException != null) ex = ex.InnerException;
            if (outerEx.GetType() != ex.GetType()) Write(LogType.Error, "ExType: {0}", outerEx.GetType().Name);
            Write(LogType.Error, $"{message} ({ex.GetType().Name}: {ex.Message})\n{ex.StackTrace}");
        }

    }
}
