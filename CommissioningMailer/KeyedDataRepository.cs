using System.Linq;
using LinqToExcel;

namespace CommissioningMailer
{
    public class KeyedDataRepository
    {
        private readonly string _filePath;
        public KeyedDataRepository(string filePath)
        {
            _filePath = filePath;
        }

        public string[][] GetAll()
        {
            var excel = new ExcelQueryFactory(_filePath);

            string[][] rowCells =
                excel.Worksheet().ToArray()
                .Select(row => row.GetRange(0, row.Count - 1)
                                  .Select(cell => cell.Value.ToString())
                                    .ToArray()
                        ).ToArray();

            return rowCells;
        }
    }
}