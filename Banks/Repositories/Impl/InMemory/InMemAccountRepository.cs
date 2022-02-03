using System;
using System.Collections.Generic;
using Banks.Entities.Accounts;
using Banks.Exceptions;

namespace Banks.Repositories.Impl.InMemory
{
    // In meme we trust!
    public class InMemAccountRepository : InMemRepository<BankAccount>
    {
    }
}