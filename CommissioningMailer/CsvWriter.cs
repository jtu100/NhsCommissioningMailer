using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commissioning.Data
{
    public static class CsvWriter
    {
        public static FileInfo WriteFile(IEnumerable<KeyedData> dataForSingleKey)
        {
            // Method expects all KeyedData instances to have the same key
            var key = dataForSingleKey.First().Key;
            var csvfilePath = GenerateFilePath(key);
            WriteCsvFile(csvfilePath, dataForSingleKey);
            return new FileInfo(csvfilePath);
        }

        private static string GenerateFilePath(string key)
        {
            var fileName = String.Format("{0}-{1:yyyyMMdd}.csv",
                                         key,
                                         DateTime.Now);

            var path = Path.Combine(Path.GetTempPath(), fileName);
            return path;
        }

        private static void WriteCsvFile(string path, IEnumerable<KeyedData> keyedDatas)
        {
            const bool appendToExistingFile = false;

            using (var writer = new StreamWriter(path, appendToExistingFile))
            {
                foreach (var keyedData in keyedDatas)
                {
                    writer.WriteLine(String.Join(",", keyedData.Data));
                }
            }
        }
    }
}