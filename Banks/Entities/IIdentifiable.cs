using System;

namespace Banks.Entities
{
    public interface IIdentifiable
    {
        Guid Id { get; init; }
    }
}