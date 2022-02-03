using System;

namespace Backups.Tools.Exceptions
{
    public class BackupException : Exception
    {
        public BackupException()
            : base()
        {
        }

        public BackupException(string message)
            : base(message)
        {
        }

        public BackupException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}