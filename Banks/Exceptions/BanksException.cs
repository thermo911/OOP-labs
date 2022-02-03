using System;

namespace Banks.Exceptions
{
    public class BanksException : Exception
    {
        public BanksException()
            : base()
        {
        }

        public BanksException(string message)
            : base(message)
        {
        }

        public BanksException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}