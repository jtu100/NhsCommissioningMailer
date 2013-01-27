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
            var surgeries = new SurgeriesRepository("SampleData\\Surgeries.csv").GetAll();
            foreach (var surgery in surgeries)
            {
                Console.WriteLine(surgery.PracticeCode);
            }
        }

        [Test]
        public void CanLoadAllSurgeryKeyedRecord()
        {
            var rows = new SurgeryKeyedRecordRepository("SampleData\\SUS Extract for Surgeries.csv").GetAll();
            foreach (var row in rows)
            {
                Console.WriteLine(string.Join(",", row));
            }
        }

        [Test]
        public void CanJoinRecords()
        {
            var surgeries = new SurgeriesRepository("SampleData\\Surgeries.csv").GetAll();
            var rows = new SurgeryKeyedRecordRepository("SampleData\\SUS Extract for Surgeries.csv").GetAll();

            var result = CsvWriter.WriteCsvFiles(surgeries, rows);
        }

    }
}
