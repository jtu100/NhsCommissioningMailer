using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommissioningMailer
{
    public class CsvWriter
    {
        public static List<MailInfo> WriteCsvFiles(IEnumerable<Surgery> surgeries, string[][] rows)
        {
            var surgeryGroups = surgeries.GroupBy(ks => ks.PracticeCode);
            var emailFileMap = new List<MailInfo>();

            foreach (var surgeryGroup in surgeryGroups)
            {
                var splitOnKey = surgeryGroup;
                var matchingRows = rows.Where(row => row[0] == splitOnKey.Key);
                var filePath = GenerateFilePath(surgeryGroup.Key);

                WriteCsvFile(filePath, matchingRows);

                emailFileMap.AddRange(
                    surgeryGroup.Select(surgery => new MailInfo
                                                       {
                                                           EmailAddress = surgery.EmailAddress,
                                                           AttachmentPath = filePath
                                                       }));
            }

            return emailFileMap;
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