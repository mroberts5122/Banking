using CsvHelper.Configuration.Attributes;
using System;
using System.Linq;

namespace BankingLibrary
{
    public class Transaction : FileDataClass
    {
        public Transaction(DateTime dateTime, Account fromAccount, Account toAccount, decimal amount, TransactionTypes transactionType)
        {
            if (fromAccount == null && toAccount == null)
                throw new ArgumentNullException("You must specify fromAccount, toAccount, or both.");

            DateTime = dateTime;
            FromAccountId = fromAccount != null ? fromAccount.Id : (Guid?)null;
            ToAccountId = toAccount != null ? toAccount.Id : (Guid?)null;
            Amount = amount;
            TransactionType = transactionType;
        }

        public Transaction(DateTime dateTime, Guid? fromAccountId, Guid? toAccountId, decimal amount, TransactionTypes transactionType)
        {
            if (fromAccountId == null && toAccountId == null)
                throw new ArgumentNullException("You must specify fromAccountId, toAccountId, or both.");

            DateTime = dateTime;
            FromAccountId = fromAccountId.HasValue ? fromAccountId.Value : (Guid?)null;
            ToAccountId = toAccountId.HasValue ? toAccountId.Value : (Guid?)null;
            Amount = amount;
            TransactionType = transactionType;
        }

        [Index(1)]
        public DateTime DateTime { get; }

        [Index(2)]
        public Guid? FromAccountId { get; }

        public Account GetFromAccount()
        {
            return Database.Accounts.SingleOrDefault(a => a.Id == FromAccountId);
        }

        [Index(3)]
        public Guid? ToAccountId { get; }

        public Account GetToAccount()
        {
            return Database.Accounts.SingleOrDefault(a => a.Id == ToAccountId);
        }

        [Index(4)]
        public decimal Amount { get; }

        [Index(5)]
        public TransactionTypes TransactionType { get; }
    }
}