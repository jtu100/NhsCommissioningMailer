using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinqToExcel;

namespace CommissioningMailer
{
    public class SurgeriesRepository
    {
        private readonly string _csvFilePath;

        public SurgeriesRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public IEnumerable<Surgery> GetAll()
        {
            var fullPath = Path.Combine(Environment.CurrentDirectory, _csvFilePath);
            var excel = new ExcelQueryFactory(fullPath);
            var surgeries = (from s in excel.Worksheet<Surgery>()
                             select s).ToList();
            return surgeries;
        }

    }
}