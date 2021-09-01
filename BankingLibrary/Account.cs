using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingLibrary
{
    public class Account : FileDataClass
    {
        public Account(Bank bank, string owner, AccountTypes accountType = AccountTypes.Checking, InvestmentAccountTypes? investmentAccountType = null)
        {
            if (bank == null)
                throw new ArgumentNullException("bank", "You must specify bank.");
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentNullException("owner", "You must specify a non-empty owner.");
            if ((accountType == AccountTypes.Checking && investmentAccountType.HasValue) || (accountType == AccountTypes.Investment && !investmentAccountType.HasValue))
                throw new ArgumentException("investmentAccountType is only valid for Investment AccountTypes and is required for Investment AccountTypes.");

            BankId = bank.Id;
            Owner = owner;
            AccountType = accountType;
            InvestmentAccountType = investmentAccountType;
        }

        public Account(Guid bankId, string owner, AccountTypes accountType = AccountTypes.Checking, InvestmentAccountTypes? investmentAccountType = null)
        {
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentNullException("owner", "You must specify a non-empty owner.");
            if ((accountType == AccountTypes.Checking && investmentAccountType.HasValue) || (accountType == AccountTypes.Investment && !investmentAccountType.HasValue))
                throw new ArgumentException("investmentAccountType is only valid for Investment AccountTypes and is required for Investment AccountTypes.");

            BankId = bankId;
            Owner = owner;
            AccountType = accountType;
            InvestmentAccountType = investmentAccountType;
        }

        [Index(1)]
        public Guid BankId { get; }

        public Bank GetBank()
        {
            return Database.Banks.SingleOrDefault(b => b.Id == BankId);
        }

        [Index(2)]
        public string Owner { get; }

        [Index(3)]
        public AccountTypes AccountType { get; }

        [Index(4)]
        public InvestmentAccountTypes? InvestmentAccountType { get; }

        public decimal GetBalance()
        {
            var depositTotal = Database.Transactions.Where(t => t.TransactionType == TransactionTypes.Deposit && t.ToAccountId == this.Id).Sum(t => t.Amount);
            var withdrawalTotal = Database.Transactions.Where(t => t.TransactionType == TransactionTypes.Withdrawal && t.FromAccountId == this.Id).Sum(t => t.Amount);
            var transferFromTotal = Database.Transactions.Where(t => t.TransactionType == TransactionTypes.Transfer && t.FromAccountId == this.Id).Sum(t => t.Amount);
            var transferToTotal = Database.Transactions.Where(t => t.TransactionType == TransactionTypes.Transfer && t.ToAccountId == this.Id).Sum(t => t.Amount);
            var total = depositTotal - withdrawalTotal - transferFromTotal + transferToTotal;
            return total;
        }

        public decimal GetWithdrawalLimit()
        {
            return AccountType == AccountTypes.Investment && InvestmentAccountType.HasValue && InvestmentAccountType.Value == InvestmentAccountTypes.Individual ? 500 : 0;
        }

        public IOrderedEnumerable<Transaction> GetTransactions()
        {
            return Database.Transactions.Where(t => t.FromAccountId == this.Id || t.ToAccountId == this.Id).OrderBy(t => t.DateTime);
        }
    }
}