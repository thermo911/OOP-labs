using System;
using System.Linq;
using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Banks.Services;
using Banks.UI.Info;

namespace Banks.Managers
{
    public class BankManager
    {
        private readonly IService<BankAccount> _accountService;
        private readonly IService<Bank> _bankService;
        private readonly IService<Client> _clientService;
        private readonly IService<Transaction> _transactionService;

        public BankManager(
            IService<BankAccount> accountService,
            IService<Bank> bankService,
            IService<Client> clientService,
            IService<Transaction> transactionService)
        {
            _accountService = accountService;
            _bankService = bankService;
            _clientService = clientService;
            _transactionService = transactionService;
        }

        public Guid CreateAndRegisterBank(BankConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var bank = new Bank(config);
            _bankService.Save(bank);
            return bank.Id;
        }

        public void SetNewBankConfig(Guid bankId, BankConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            Bank bank = _bankService.GetById(bankId);
            bank.UpdateConfig(config);
            _bankService.Save(bank);
        }

        public void SaveClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _clientService.Save(client);
        }

        public Guid CreateDebitAccount(Guid bankId, Guid clientId)
        {
            Bank bank = _bankService.GetById(bankId);
            Client client = _clientService.GetById(clientId);

            DebitAccount account = bank.CreateAndRegisterDebitAccount(client);

            _bankService.Save(bank);
            _accountService.Save(account);

            return account.Id;
        }

        public Guid CreateDepositAccount(
            Guid bankId,
            Guid clientId,
            decimal startBalance,
            DateTime tillDateTime)
        {
            Bank bank = _bankService.GetById(bankId);
            Client client = _clientService.GetById(clientId);

            DepositAccount account = bank.CreateAndRegisterDepositAccount(client, startBalance, tillDateTime);
            Transaction transaction = account.TopUp(startBalance);

            _bankService.Save(bank);
            _transactionService.Save(transaction);
            _accountService.Save(account);

            return account.Id;
        }

        public Guid CreateCreditAccount(Guid bankId, Guid clientId)
        {
            Bank bank = _bankService.GetById(bankId);
            Client client = _clientService.GetById(clientId);

            CreditAccount account = bank.CreateAndRegisterCreditAccount(client);

            _bankService.Save(bank);
            _accountService.Save(account);

            return account.Id;
        }

        public Guid PerformWithdrawal(Guid accountId, decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"'{nameof(moneyAmount)}' is negative or zero");

            BankAccount account = _accountService.GetById(accountId);
            if (account.Owner.IsSuspicious())
                throw new BanksException($"operation forbidden: {accountId} is suspicious");

            Transaction transaction = account.Withdraw(moneyAmount);
            _transactionService.Save(transaction);
            _accountService.Save(account);

            return transaction.Id;
        }

        public Guid PerformToppingUp(Guid accountId, decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"'{nameof(moneyAmount)}' is negative or zero");

            BankAccount account = _accountService.GetById(accountId);
            if (account.Owner.IsSuspicious())
                throw new BanksException($"operation forbidden: {accountId} is suspicious");

            Transaction transaction = account.TopUp(moneyAmount);
            _transactionService.Save(transaction);
            _accountService.Save(account);

            return transaction.Id;
        }

        public Guid PerformTransfer(Guid sourceId, Guid receiverId, decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"'{nameof(moneyAmount)}' is negative or zero");

            BankAccount source = _accountService.GetById(sourceId);
            if (source.Owner.IsSuspicious())
                throw new BanksException($"operation forbidden: {sourceId} is suspicious");

            BankAccount receiver = _accountService.GetById(receiverId);
            Transaction transaction = source.Transfer(moneyAmount, receiver);
            _transactionService.Save(transaction);

            return transaction.Id;
        }

        public void CancelTransfer(Guid transactionId)
        {
            Transaction transfer = _transactionService.GetById(transactionId);
            Transaction.CancelTransfer(
                transfer,
                out Transaction topUp,
                out Transaction withdrawal);

            _accountService.Save(transfer.Receiver);
            _accountService.Save(transfer.Source);

            _transactionService.Save(transfer);
            _transactionService.Save(topUp);
            _transactionService.Save(withdrawal);
        }

        public void WithdrawFees()
        {
            _bankService.GetAll()
                .ToList()
                .ForEach(bank =>
                    bank.WithdrawFees()
                        .ToList()
                        .ForEach(transaction =>
                            _transactionService.Save(transaction)));
        }

        public void PayInterest(DateTime from, DateTime to)
        {
            if (from >= to)
                throw new ArgumentException("invalid time period");

            _bankService.GetAll()
                .ToList()
                .ForEach(bank =>
                    bank.PayInterests(from, to)
                        .ToList()
                        .ForEach(transaction =>
                            _transactionService.Save(transaction)));
        }

        public AccountInfo GetAccountInfo(Guid accountId)
        {
            BankAccount account = _accountService.GetById(accountId);
            return new AccountInfo(account);
        }

        public BankInfo GetBankInfo(Guid bankId)
        {
            Bank bank = _bankService.GetById(bankId);
            return new BankInfo(bank);
        }

        public Client GetClient(Guid clientId)
            => _clientService.GetById(clientId);

        public ClientInfo GetClientInfo(Guid clientId)
            => new ClientInfo(GetClient(clientId));

        public void SubscribeClientToBank(Guid bankId, Guid clientId)
        {
            Bank bank = _bankService.GetById(bankId);
            Client client = _clientService.GetById(clientId);

            bank.AddSubscriber(client);
            _bankService.Save(bank);
        }

        public void UnsubscribeClientToBank(Guid bankId, Guid clientId)
        {
            Bank bank = _bankService.GetById(bankId);
            Client client = _clientService.GetById(clientId);

            bank.RemoveSubscriber(client);
            _bankService.Save(bank);
        }
    }
}