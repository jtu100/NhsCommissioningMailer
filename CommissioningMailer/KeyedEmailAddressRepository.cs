using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinqToExcel;

namespace CommissioningMailer
{
    public class KeyedEmailAddressRepository
    {
        private readonly string _filePath;
        public KeyedEmailAddressRepository(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Gets all keyed email addresses, from the first and second spreadsheet columns respectively
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyedEmailAddress> GetAll()
        {
            const int keyColumnIndex = 0;
            const int emailAddressColumnIndex = 1;

            // Needs to be verified but I think full path needed otherwise we get a crash when run from UI
            var fullPath = Path.Combine(Environment.CurrentDirectory, _filePath);
            var excel = new ExcelQueryFactory(fullPath);
            var keyedEmailAddresses = (from row in excel.Worksheet().ToArray()
                             select new KeyedEmailAddress
                                        {
                                            Key = row[keyColumnIndex].ToString(),
                                            EmailAddress = row[emailAddressColumnIndex].ToString()
                                        }
                             );
            return keyedEmailAddresses;
        }

    }
}