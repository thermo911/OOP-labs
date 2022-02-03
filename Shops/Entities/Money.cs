using System;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public readonly struct Money
    {
        private const double Eps = 1e-2;
        private const int Accuracy = 2;
        public Money(double value)
        {
            value = Math.Round(value, Accuracy);
            if (!IsValidValue(value))
                throw new InvalidArgumentException("Price constructor: invalid parameter 'value'");
            Value = value;
        }

        public double Value { get; }

        private static bool IsValidValue(double value) => !double.IsNegative(value);
    }
}