using System.Linq;
using LinqToExcel;

namespace CommissioningMailer
{
    public class SurgeryKeyedRecordRepository
    {
        private readonly string _csvFilePath;
        public SurgeryKeyedRecordRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public string[][] GetAll()
        {
            var excel = new ExcelQueryFactory(_csvFilePath);

            string[][] rowCells = (from row in excel.Worksheet().ToArray()
                                       select row)
                .Select(row => row.GetRange(0, row.Count - 1)
                                  .Select(cell => cell.Value.ToString())
                                  .ToArray()
                        ).ToArray();
            
            return rowCells;
        }
    }
}