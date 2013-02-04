using System.Collections.Generic;
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

        public IEnumerable<KeyedData> GetAll()
        {
            var excel = new ExcelQueryFactory(_filePath);

            // LinqToExcel doesn't handle most LINQ expressions so we materialize it immediately
            IEnumerable<KeyedData> rowCells = excel.Worksheet().ToArray()
                .Select(row => new KeyedData
                {
                    Data = row.Select(cell => cell.Value.ToString()).ToArray()
                });

            return rowCells;
        }
    }
}