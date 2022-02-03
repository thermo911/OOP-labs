namespace Banks.Exceptions
{
    public class InsufficientFundsException : BanksException
    {
        public InsufficientFundsException()
        {
        }

        public InsufficientFundsException(string message)
            : base(message)
        {
        }
    }
}