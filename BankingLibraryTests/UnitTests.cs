using BankingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BankingLibraryTests
{
    [TestClass]
    public class UnitTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            Database.LoadData();
        }

        [TestMethod]
        public void DepositTest()
        {
            var account = Database.Accounts.First();
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file.");
            var initialBalance = account.GetBalance();
            var amount = 12.56M;
            Service.Deposit(DateTime.Now, account, amount);
            var balance = account.GetBalance();
            Assert.AreEqual(initialBalance + amount, balance, "Before and after balance should differ by deposit amount.");
        }

        [TestMethod]
        public void WithdrawTest()
        {
            var account = Database.Accounts.First();
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file.");
            var initialBalance = account.GetBalance();
            var amount = 1.78M;
            Service.Withdraw(DateTime.Now, account, amount);
            var balance = account.GetBalance();
            Assert.AreEqual(initialBalance - amount, balance, "Before and after balance should differ by withdrawal amount.");
        }

        [TestMethod]
        public void WithdrawLimitErrorTest()
        {
            var account = Database.Accounts.FirstOrDefault(a => a.AccountType == AccountTypes.Investment && a.InvestmentAccountType.HasValue && a.InvestmentAccountType.Value == InvestmentAccountTypes.Individual);
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file or the correct type of account does not exist.");
            var amount = 1000;
            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    Service.Withdraw(DateTime.Now, account, amount);
                }
            );
        }

        [TestMethod]
        public void WithdrawBalanceErrorTest()
        {
            var account = Database.Accounts.First();
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file.");
            var amount = 1_000_000_000;
            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    Service.Withdraw(DateTime.Now, account, amount);
                }
            );
        }

        [TestMethod]
        public void TransferLimitErrorTest()
        {
            var account = Database.Accounts.FirstOrDefault(a => a.AccountType == AccountTypes.Investment && a.InvestmentAccountType.HasValue && a.InvestmentAccountType.Value == InvestmentAccountTypes.Individual);
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file or the correct type of account does not exist.");
            var account2 = Database.Accounts.Last();
            Assert.IsNotNull(account2, "account2 is null. Either there are no accounts available or accounts were not loaded correctly.");
            var amount = 1000;
            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    Service.Transfer(DateTime.Now, account, account2, amount);
                }
            );
        }

        [TestMethod]
        public void TransferBalanceErrorTest()
        {
            var account = Database.Accounts.FirstOrDefault(a => a.AccountType == AccountTypes.Investment && a.InvestmentAccountType.HasValue && a.InvestmentAccountType.Value == InvestmentAccountTypes.Individual);
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file or the correct type of account does not exist.");
            var account2 = Database.Accounts.Last();
            Assert.IsNotNull(account2, "account2 is null. Either there are no accounts available or accounts were not loaded correctly.");
            var amount = 1_000_000_000;
            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    Service.Transfer(DateTime.Now, account, account2, amount);
                }
            );
        }

        [TestMethod]
        public void TransferTest()
        {
            var account = Database.Accounts.First();
            Assert.IsNotNull(account, "account is null. Either there are no accounts available or accounts were not loaded correctly from file.");
            var initialBalance = account.GetBalance();
            var amount = 1.78M;
            Service.Withdraw(DateTime.Now, account, amount);
            var balance = account.GetBalance();
            Assert.AreEqual(initialBalance - amount, balance, "Before and after balance should differ by withdrawal amount.");
        }
    }
}