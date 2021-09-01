# Banking

This code demonstrates several .NET/C# concepts through an object-oriented Banking library. Requirements include:

1.	This is a simple bank program.
2.	A bank has a name.
3.	A bank also has several accounts.
4.	An account has an owner and a balance.
5.	Account types include: Checking, Investment.
6.	There are two types of Investment accounts: Individual, Corporate.
7.	Individual accounts have a withdrawal limit of 500 dollars.
8.	Transactions are made on accounts.
9.	Transaction types include: Deposit, Withdraw, and Transfer.

I have used used simple List<T> variables to house data and LINQ to query it.
Given more time, I would use an actual RDBMS which would speed up query and other operations.

Also, simply for convenience, I have used a LINQ query to calculate balance and other objects and values on every request.
This is obviously inefficient, but works sufficiently for mostly static data.

The Database class only holds data and provides the means to load from and save to CSV files using a popular helper called CsvHelper.
The Service class coordinates transaction for accounts, between accounts, etc. While it is not an ideal implementation, it shows
my philosophy around separation of concerns.

My implementation of Transactions includes a FromAccount and ToAccount in the same record. Deposit and Withdraw use only one
of those properties, but Transfer uses both. It is also possible to split a Transfer into a lone withdrawal and lone deposit.
The Service.DualOperationTransfer() method demonstrates how I might do that.

Finally, there are tests for every operation, including those that throw exceptions. All tests should pass.
