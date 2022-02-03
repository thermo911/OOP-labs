using System;
using Banks.AppConfig;
using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Managers;
using Banks.UI.Info;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BankManagerTests
    {
        private BankManager _manager;

        private Client _client1 = new ClientBuilder()
            .Name("Alex")
            .Surname("Blazhkov")
            .Address("SPb")
            .Passport("secret").GetResult();

        private BankConfig _config1 = new BankConfigBuilder()
            .CreditLimit(1)
            .DebitInterestRate(3)
            .FeeRate(100)
            .AddDepositRateEntry(new DepositRateEntry(2000, 5))
            .Build();

        [SetUp]
        public void Setup()
        {
            _manager = InMemManagerConfig.NewManager();
        }
        
        [Test]
        public void AddClient_ClientAdded()
        {
            _manager.SaveClient(_client1);
            ClientInfo clientInfo = _manager.GetClientInfo(_client1.Id);
            Assert.AreEqual(_client1.Id, clientInfo.Id);
        }

        [Test]
        public void CreateAndRegisterBank_BankAdded()
        {
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            BankInfo bankInfo = _manager.GetBankInfo(bankId);
            Assert.AreEqual(bankId, bankInfo.Id);
        }

        [Test]
        public void CreateDebitAccount_AccountCreated()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId = _manager.CreateDebitAccount(bankId, _client1.Id);
            AccountInfo accountInfo = _manager.GetAccountInfo(accId);
            Assert.AreEqual(_config1.DebitInterestRate, accountInfo.InterestRate);
            Assert.AreEqual(AccountType.Debit, accountInfo.Type);
        }
        
        [Test]
        public void CreateCreditAccount_AccountCreated()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId = _manager.CreateCreditAccount(bankId, _client1.Id);
            AccountInfo accountInfo = _manager.GetAccountInfo(accId);
            Assert.AreEqual(_config1.Fee, accountInfo.Fee);
            Assert.AreEqual(AccountType.Credit, accountInfo.Type);
        }
        
        [Test]
        public void CreateDepositAccount_AccountCreated()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId = _manager.CreateDepositAccount(bankId, _client1.Id, 4000, DateTime.Now.AddYears(1));
            AccountInfo accountInfo = _manager.GetAccountInfo(accId);
            Assert.AreEqual(5.0m, accountInfo.InterestRate);
            Assert.AreEqual(AccountType.Deposit, accountInfo.Type);
        }

        [Test]
        public void PerformToppingUp_Success()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId = _manager.CreateDebitAccount(bankId, _client1.Id);
            
            AccountInfo accountInfo = _manager.GetAccountInfo(accId);
            
            Assert.AreEqual(0.0m, accountInfo.Balance);
            
            _manager.PerformToppingUp(accountInfo.Id, 2000.0m);
            accountInfo = _manager.GetAccountInfo(accId);
            
            Assert.AreEqual(2000.0m, accountInfo.Balance);
        }
        
        [Test]
        public void PerformWithdrawal_Success()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId = _manager.CreateDebitAccount(bankId, _client1.Id);
            
            AccountInfo accountInfo = _manager.GetAccountInfo(accId);
            
            _manager.PerformToppingUp(accountInfo.Id, 2000.0m);
            _manager.PerformWithdrawal(accountInfo.Id, 1000.0m);

            accountInfo = _manager.GetAccountInfo(accId);
            Assert.AreEqual(1000.0m, accountInfo.Balance);
        }

        [Test]
        public void PerformTransfer_Success()
        {
            _manager.SaveClient(_client1); 
            Guid bankId = _manager.CreateAndRegisterBank(_config1);
            Guid accId1 = _manager.CreateDebitAccount(bankId, _client1.Id);
            Guid accId2 = _manager.CreateDebitAccount(bankId, _client1.Id);

            _manager.PerformToppingUp(accId1, 2000.0m);
            _manager.PerformTransfer(accId1, accId2, 1000.0m);
            
            AccountInfo accountInfo1 = _manager.GetAccountInfo(accId1);
            AccountInfo accountInfo2 = _manager.GetAccountInfo(accId2);
            
            Assert.AreEqual(accountInfo1.Balance, accountInfo2.Balance);
        }
    }
}