using CsvHelper.Configuration.Attributes;
using System;

namespace BankingLibrary
{
    public abstract class FileDataClass
    {
        [Index(0)]
        public Guid Id { get; } = Guid.NewGuid();
    }
}