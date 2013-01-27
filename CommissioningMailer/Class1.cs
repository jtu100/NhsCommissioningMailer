using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CommissioningMailer
{
    [TestFixture]
    public class Class1
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
            var surgeryGroups = new SurgeriesRepository("SampleData\\Surgeries.csv").GetAll()
                .GroupBy( ks => ks.PracticeCode);

            var rows = new SurgeryKeyedRecordRepository("SampleData\\SUS Extract for Surgeries.csv").GetAll();
            
            var tempPath = Path.GetTempPath();
            foreach (var surgeryGroup in surgeryGroups)
            {
                var surgeryEmailAddressCount = 0;
                foreach (var surgery in surgeryGroup)
                {
                    var fileName = string.Format("{0}-{1:yyyyMMdd}-{2}.csv", 
                        surgery.PracticeCode, 
                        DateTime.Now,
                        surgeryEmailAddressCount);

                    var path = Path.Combine(tempPath, fileName);
                    using (var writer = new StreamWriter(path, true))
                    {
                        var surgery1 = surgery;
                        var matchingPractices = rows.Where(row => row[0] == surgery1.PracticeCode);
                        foreach (var cell in matchingPractices)
                        {
                            writer.WriteLine(string.Join(",", cell));
                        }
                    }
                    surgeryEmailAddressCount++;
                }
                
            }
        }
    }
}
