using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace TransferCsvBase.TransferService
{
    public class CsvTransfer
    {
        public static DataTable GetFileTableCSV(string csvContent, char separator)
        {
            
            var allLines = csvContent.Split(new[] { '\r', '\n' });

            //var enc = Encoding.Default;
            //var allLines = File.ReadAllLines(fileCsvPath, enc);

            allLines = allLines.Where(line => !String.IsNullOrEmpty(line)).ToArray();

            //считываем заголовки и вставляем колонки
            var columns = allLines[0].Split(new[] { separator });
            var dt = new DataTable();
            var cols = columns.GetLength(0);

            for (var i = 0; i < cols; i++)
            {
                dt.Columns.Add(columns[i]);
            }

            //считываем и вставляем поля
            for (var i = 1; i < allLines.GetLength(0); i++)
            {
                var fields = allLines[i].Split(new[] { separator });
                var row = dt.NewRow();

                for (var j = 0; j < cols; j++)
                {
                    row[j] = fields[j].Trim();
                }

                dt.Rows.Add(row);
            }
            //наша отпарсенная DataTable
            return dt;
        }
    }
}
