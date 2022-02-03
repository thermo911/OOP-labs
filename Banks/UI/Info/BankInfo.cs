using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Banks;

namespace Banks.UI.Info
{
    public class BankInfo
    {
        public BankInfo(Bank bank)
        {
            Id = bank.Id;
            AccountInfos = new List<AccountInfo>(
                bank.Accounts.Select(account => new AccountInfo(account)));
        }

        public Guid Id { get; set; }
        public List<AccountInfo> AccountInfos { get; set; }
    }
}