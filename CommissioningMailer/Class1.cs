using System;
using System.Collections.Generic;
using System.Linq;
using LinqToExcel;
using NUnit.Framework;

namespace CommissioningMailer
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void CanLoadAllSurgeries()
        {
            var surgeries = new SurgeriesRepo("SampleData\\Surgeries.csv").GetAll();
            foreach (var surgery in surgeries)
            {
                Console.WriteLine(surgery.PracticeCode);
            }
        }
    }

    public class SurgeriesRepo
    {
        private readonly string _csvFilePath;

        public SurgeriesRepo(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public IEnumerable<Surgery> GetAll()
        {
            var excel = new ExcelQueryFactory(_csvFilePath);
            var surgeries = from s in excel.Worksheet<Surgery>()
                                  select s;
            return surgeries;
        }
    }

    public class Surgery
    {
        public string PracticeCode { get; set; }
        public string LeadGp { get; set; }
        public string PracticeName { get; set; }
        public string EmailAddress { get; set; }
    }
}
