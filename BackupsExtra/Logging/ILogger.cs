namespace BackupsExtra.Logging
{
    public interface ILogger
    {
        public void Log(string context, string message);
        public void Log(string context, LogType logType, string message);
    }
}