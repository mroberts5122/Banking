using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingLibrary
{
    public class Bank : FileDataClass
    {
        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "You must specify a non-empty name.");

            Name = name;
        }

        [Index(1)]
        public string Name { get; }

        public IEnumerable<Account> GetAccounts()
        {
            return Database.Accounts.Where(a => a.BankId == this.Id);
        }

        public IOrderedEnumerable<Transaction> GetTransactions()
        {
            return Database.Transactions.Where(t => t.GetFromAccount()?.BankId == this.Id || t.GetToAccount()?.BankId == this.Id).OrderBy(t => t.DateTime);
        }

        public decimal GetBalance()
        {
            return GetAccounts().Sum(a => a.GetBalance());
        }
    }
}