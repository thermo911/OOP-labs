namespace Banks.Exceptions
{
    public class InvalidMoneyException : BanksException
    {
        public InvalidMoneyException()
        {
        }

        public InvalidMoneyException(string message)
            : base(message)
        {
        }
    }
}