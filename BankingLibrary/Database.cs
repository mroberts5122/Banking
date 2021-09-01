using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BankingLibrary
{
    public static class Database
    {
        private const string BanksFilename = "data_banks.csv";
        private const string AccountsFilename = "data_accounts.csv";
        private const string TransactionsFilename = "data_transactions.csv";

        private static readonly string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private static readonly string BanksFullPath = Path.Combine(MyDocumentsPath, BanksFilename);
        private static readonly string AccountsFullPath = Path.Combine(MyDocumentsPath, AccountsFilename);
        private static readonly string TransactionsFullPath = Path.Combine(MyDocumentsPath, TransactionsFilename);

        public static List<Bank> Banks { get; } = new List<Bank>();

        public static List<Account> Accounts { get; } = new List<Account>();

        public static List<Transaction> Transactions { get; } = new List<Transaction>();

        public static void LoadData()
        {
            Bank b1 = null;
            Bank b2 = null;
            Account a1 = null;
            Account a2 = null;

            if (File.Exists(BanksFullPath))
            {
                using (var streamReader = new StreamReader(BanksFullPath))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    Banks.AddRange(csvReader.GetRecords<Bank>());
                }
            }
            else
            {
                b1 = new Bank("bank1");
                Banks.Add(b1);
                b2 = new Bank("bank2");
                Banks.Add(b2);
            }

            if (File.Exists(AccountsFullPath))
            {
                using (var streamReader = new StreamReader(AccountsFullPath))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    Accounts.AddRange(csvReader.GetRecords<Account>());
                }
            }
            else
            {
                a1 = new Account(b1, "owner1");
                Accounts.Add(a1);
                a2 = new Account(b1, "owner1", AccountTypes.Investment, InvestmentAccountTypes.Individual);
                Accounts.Add(a2);
            }

            if (File.Exists(TransactionsFullPath))
            {
                using (var streamReader = new StreamReader(TransactionsFullPath))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    Transactions.AddRange(csvReader.GetRecords<Transaction>());
                }
            }
            else
            {
                var t1 = new Transaction(DateTime.Now, null, a1, 123.45M, TransactionTypes.Deposit);
                Transactions.Add(t1);
            }
        }

        public static void SaveData()
        {
            using (var streamWriter = new StreamWriter(BanksFullPath))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                //csvWriter.WriteRecords(Banks);
            }

            using (var streamWriter = new StreamWriter(AccountsFullPath))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                //csvWriter.WriteRecords(Accounts);
            }

            using (var streamWriter = new StreamWriter(TransactionsFullPath))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                //csvWriter.WriteRecords(Transactions);
            }
        }
    }
}