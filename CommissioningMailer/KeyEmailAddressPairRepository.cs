using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinqToExcel;

namespace CommissioningMailer
{
    public class KeyEmailAddressPairRepository
    {
        private readonly string _filePath;
        public KeyEmailAddressPairRepository(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Gets all key and email address from the first and second spreadsheet columns respectively
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyEmailAddressPair> GetAll()
        {
            const int keyColumnIndex = 0;
            const int emailAddressColumnIndex = 1;

            // Full path needed to stop crash when run from UI
            var fullPath = Path.Combine(Environment.CurrentDirectory, _filePath);
            var excel = new ExcelQueryFactory(fullPath);
            var surgeries = (from row in excel.Worksheet().ToArray()
                             select new KeyEmailAddressPair
                                        {
                                            Key = row[keyColumnIndex].ToString(),
                                            EmailAddress = row[emailAddressColumnIndex].ToString()
                                        }
                             );
            return surgeries;
        }

    }
}