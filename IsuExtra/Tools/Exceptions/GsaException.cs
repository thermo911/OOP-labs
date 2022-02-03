using System;

namespace IsuExtra.Tools.Exceptions
{
    public class GsaException : Exception
    {
        public GsaException()
        {
        }

        public GsaException(string message)
            : base(message)
        {
        }

        public GsaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}