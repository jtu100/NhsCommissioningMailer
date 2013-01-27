using System.Collections.Generic;
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
            var excel = new ExcelQueryFactory(_csvFilePath);
            var surgeries = (from s in excel.Worksheet<Surgery>()
                             select s).ToList();
            return surgeries;
        }

    }
}