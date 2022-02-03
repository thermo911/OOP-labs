using System;

namespace Banks.Exceptions
{
    public class NotFoundException : BanksException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}