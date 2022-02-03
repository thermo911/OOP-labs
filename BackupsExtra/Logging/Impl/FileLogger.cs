using System;
using System.IO;
using System.Text;

namespace BackupsExtra.Logging.Impl
{
    public class FileLogger : ILogger
    {
        private readonly string _fileName;

        public FileLogger(string fileName, bool timestampRequired)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException($"'{nameof(fileName)}' is null or white space");

            _fileName = fileName;
            TimestampRequired = timestampRequired;
        }

        public bool TimestampRequired { get; }
        public string FileName => _fileName;

        public void Log(string context, string message)
        {
            Log(context, LogType.Info, message);
        }

        public void Log(string context, LogType logType, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException($"'{nameof(context)}' is null or white space");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException($"'{nameof(message)}' is null or white space");

            using var writer = new StreamWriter(_fileName, true);
            string logString = BuildLogString(logType.ToString(), message);

            writer.WriteLine(logString);
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