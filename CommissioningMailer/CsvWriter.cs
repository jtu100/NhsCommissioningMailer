using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommissioningMailer
{
    public class CsvWriter
    {
        public static List<MailInfo> WriteCsvFiles(
            IEnumerable<KeyEmailAddressPair> keyEmailAddressPairs, 
            string[][] rows)
        {
            var pairsGroupedByKey = keyEmailAddressPairs.GroupBy(ks => ks.Key);
            var mailerInfo = new List<MailInfo>();

            foreach (var pairsGroup in pairsGroupedByKey)
            {
                var @group = pairsGroup;
                var matchingRows = rows.Where(row => row[0] == @group.Key);

                var csvfilePath = GenerateFilePath(pairsGroup.Key);
                WriteCsvFile(csvfilePath, matchingRows);

                mailerInfo.AddRange(
                    pairsGroup.Select(keyEmailAddressPair => new MailInfo
                                    {
                                        Key = keyEmailAddressPair.Key,
                                        EmailAddress = keyEmailAddressPair.EmailAddress,
                                        AttachmentPath = csvfilePath
                                    }));
            }

            return mailerInfo;
        }

        private static string GenerateFilePath(string surgeryCode)
        {
            var fileName = string.Format("{0}-{1:yyyyMMdd}.csv",
                                         surgeryCode,
                                         DateTime.Now);

            var path = Path.Combine(Path.GetTempPath(), fileName);
            return path;
        }

        private static void WriteCsvFile(string path, IEnumerable<string[]> rowsOfCells)
        {
            bool appendToExistingFile = false;
            using (var writer = new StreamWriter(path, appendToExistingFile))
            {
                foreach (var cells in rowsOfCells)
                {
                    writer.WriteLine(string.Join(",", cells));
                }
            }
        }
    }
}