using System;
using System.Collections.Generic;
using Banks.Entities.Accounts;

namespace Banks.Entities.Clients
{
    public class ClientBuilder
    {
        private Client _client;

        public ClientBuilder(Client client = null)
        {
            _client = client;
            if (_client == null)
                Reset();
        }

        public ClientBuilder Name(string name)
        {
            _client.Name = name ?? throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"argument '{nameof(name)}' is empty");
            return this;
        }

        public ClientBuilder Surname(string surname)
        {
            _client.Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException($"argument '{nameof(surname)}' is empty");
            return this;
        }

        public ClientBuilder Address(string address)
        {
            _client.Address = address ?? throw new ArgumentNullException(nameof(address));
            return this;
        }

        public ClientBuilder Passport(string passport)
        {
            _client.Passport = passport ?? throw new ArgumentNullException(nameof(passport));
            return this;
        }

        public Client GetResult()
        {
            if (!IsValidClient())
                throw new InvalidOperationException($"'{nameof(_client.Name)}' or '{nameof(_client.Surname)}' is null");
            Client result = _client;
            Reset();
            return result;
        }

        private void Reset()
        {
            _client = new Client
            {
                Id = Guid.NewGuid(),
                Accounts = new HashSet<BankAccount>(),
            };
        }

        private bool IsValidClient()
        {
            return !string.IsNullOrWhiteSpace(_client.Name) &&
                   !string.IsNullOrWhiteSpace(_client.Surname);
        }
    }
}