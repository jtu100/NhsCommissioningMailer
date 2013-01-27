using System;
using NUnit.Framework;

namespace CommissioningMailer
{
    [TestFixture]
    public class CommissioningMailerTest
    {
        [Test]
        public void CanLoadAllSurgeries()
        {
            var surgeries = new KeyEmailAddressPairRepository("SampleData\\Surgeries.csv").GetAll();
            foreach (var surgery in surgeries)
            {
                Console.WriteLine(surgery.Key);
            }
        }

        [Test]
        public void CanLoadAllSurgeryKeyedRecord()
        {
            var rows = new KeyedDataRepository("SampleData\\SUS Extract for Surgeries.csv").GetAll();
            foreach (var row in rows)
            {
                Console.WriteLine(string.Join(",", row));
            }
        }

        [Test]
        public void CanJoinRecords()
        {
            var surgeries = new KeyEmailAddressPairRepository("SampleData\\Surgeries.csv").GetAll();
            var rows = new KeyedDataRepository("SampleData\\SUS Extract for Surgeries.csv").GetAll();

            var result = CsvWriter.WriteCsvFiles(surgeries, rows);
        }

    }
}
