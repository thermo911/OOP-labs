using System;

namespace Shops.Tools.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException()
        {
        }

        public PersonNotFoundException(string message)
        {
        }

        public PersonNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}