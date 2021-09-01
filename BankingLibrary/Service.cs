using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingLibrary
{
    public static class Service
    {
        public static void Deposit(DateTime dateTime, Account account, decimal amount)
        {
            try
            {
                var transaction = new Transaction(dateTime, null, account, amount, TransactionTypes.Deposit);
                Database.Transactions.Add(transaction);
            }
            catch(Exception ex)
            {
                //log exception with lib
                Console.Write(ex);

                throw;
            }
        }

        public static void Withdraw(DateTime dateTime, Account account, decimal amount)
        {
            try
            {
                var limit = account.GetWithdrawalLimit();
                if (limit > 0 && amount > limit)
                    throw new InvalidOperationException("The specified amount is greater than the accout withdrawal limit.");
                var balance = account.GetBalance();
                if (balance < amount)
                    throw new InvalidOperationException("The specified account does not have sufficient balance to withdraw the specified amount.");

                var transaction = new Transaction(dateTime, account, null, amount, TransactionTypes.Withdrawal);
                Database.Transactions.Add(transaction);
            }
            catch (Exception ex)
            {
                //log exception with lib
                Console.Write(ex);

                throw;
            }
        }

        public static void Transfer(DateTime dateTime, Account fromAccount, Account toAccount, decimal amount)
        {
            try
            {
                var fromLimit = fromAccount.GetWithdrawalLimit();
                if (fromLimit > 0 && amount > fromLimit)
                    throw new InvalidOperationException("The specified amount is greater than the account withdrawal limit.");
                var fromBalance = fromAccount.GetBalance();
                if (fromBalance < amount)
                    throw new InvalidOperationException("The specified fromAccount does not have sufficient balance to transfer the specified amount.");

                var transaction = new Transaction(dateTime, fromAccount, toAccount, amount, TransactionTypes.Transfer);
                Database.Transactions.Add(transaction);
            }
            catch (Exception ex)
            {
                //log exception with lib
                Console.Write(ex);

                throw;
            }
        }

        public static void DualOperationTransfer(DateTime dateTime, Account fromAccount, Account toAccount, decimal amount)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var fromLimit = fromAccount.GetWithdrawalLimit();
                    if (fromLimit > 0 && amount > fromLimit)
                        throw new InvalidOperationException("The specified amount is greater than the accout withdrawal limit.");
                    var fromBalance = fromAccount.GetBalance();
                    if (fromBalance < amount)
                        throw new InvalidOperationException("The specified fromAccount does not have sufficient balance to transfer the specified amount.");

                    var transaction1 = new Transaction(dateTime, fromAccount, null, amount, TransactionTypes.Withdrawal);
                    Database.Transactions.Add(transaction1);
                    var transaction2 = new Transaction(dateTime, null, toAccount, amount, TransactionTypes.Deposit);
                    Database.Transactions.Add(transaction1);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                //log exception with lib
                Console.Write(ex);

                throw;
            }
        }
    }
}