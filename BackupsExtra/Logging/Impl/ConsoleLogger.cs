using System;
using System.IO;
using System.Text;

namespace BackupsExtra.Logging.Impl
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger(bool timestampRequired)
        {
            TimestampRequired = timestampRequired;
        }

        public bool TimestampRequired { get; }

        public void Log(string context, string message)
        {
            Log(context, LogType.Info, message);
        }

        public void Log(string context, LogType logType, string message)
        {
            if (string.IsNullOrWhiteSpace(context))
                throw new ArgumentException($"'{nameof(context)}' is null or white space");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException($"'{nameof(message)}' is null or white space");

            string logString = BuildLogString($"[{context}]", logType.ToString(), message);
            Console.WriteLine(logString);
        }

        private string BuildLogString(params string[] args)
        {
            var builder = new StringBuilder();

            if (TimestampRequired)
                builder.Append(DateTime.Now);

            foreach (string arg in args)
                builder.Append(' ').Append(arg);

            return builder.ToString();
        }
    }
}