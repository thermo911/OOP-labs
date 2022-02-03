using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Clients;

namespace Banks.UI.Info
{
    public class ClientInfo
    {
        public ClientInfo(Client client)
        {
            Id = client.Id;
            Name = client.Name ?? "<unknown>";
            Surname = client.Surname ?? "<unknown>";
            Address = string.IsNullOrWhiteSpace(client.Address) ? "<unknown>" : client.Address;
            Passport = string.IsNullOrWhiteSpace(client.Passport) ? "<unknown>" : client.Passport;
            AccountInfos = new List<AccountInfo>(client.Accounts.Select(
                account => new AccountInfo(account)));
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }
        public List<AccountInfo> AccountInfos { get; set; }
    }
}