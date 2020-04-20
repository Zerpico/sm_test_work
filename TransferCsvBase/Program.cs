using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TimeSeries.Common.Models;
using TransferCsvBase.DAL;

namespace TransferCsvBase
{
    static class Program
    {

        static void Main(string[] args)
        {
            using (var context = new DAL.DbTimeSeriesContext())
            {

                
              

                Console.WriteLine("Подключение к БД");
                //создадим БД если отсутсвует
                context.Database.EnsureCreated();

                //Переносим Countries
                TransferCounties(context, Properties.Resources.Country);
                //Переносим Indicators
                TransferIndicators(context, Properties.Resources.Indicator);
                //Переносим Series
                TransferSeries(context, Properties.Resources.Series);
                //Переносим Observables
                TransferObservables(context, Properties.Resources.Observables);
            }

            Console.WriteLine("All work done!");
            Console.ReadKey(true);
        }

        #region MethodsUgly
        static void TransferCounties(DbTimeSeriesContext context, string csvContent)
        {
            Console.Write("Трансфер Country => SQL \t...");

            context.Countries.RemoveRange(context.Countries);
            var counties = TransferService.CsvTransfer.GetFileTableCSV(csvContent, ',');
            foreach (DataRow row in counties.Rows)
            {
                context.Countries.Add
                    (
                        new Country() { CountryId = Convert.ToInt32(row["Key"]), CountryName = row["Country Name"].ToString() }
                    );
            }
            context.SaveChanges();
            Console.WriteLine(" \t Done! Transfer: " + counties.Rows.Count);
        }

        static void TransferIndicators(DbTimeSeriesContext context, string csvContent)
        {
            Console.Write("Трансфер Indicator => SQL \t...");

            context.Indicators.RemoveRange(context.Indicators);
            var indicators = TransferService.CsvTransfer.GetFileTableCSV(csvContent, ',');
            foreach (DataRow row in indicators.Rows)
            {
                context.Indicators.Add
                    (
                        new Indicator() { IndicatorId = Convert.ToInt32(row["Key"]), IndicatorName = row["Indicator Name"].ToString() }
                    );
            }
            context.SaveChanges();
            Console.WriteLine(" \t Done! Transfer: " + indicators.Rows.Count);
        }

        static void TransferSeries(DbTimeSeriesContext context, string csvContent)
        {
            Console.Write("Трансфер Indicator => SQL \t...");

            context.Series.RemoveRange(context.Series);
            var series = TransferService.CsvTransfer.GetFileTableCSV(csvContent, ',');
            foreach (DataRow row in series.Rows)
            {
                context.Series.Add
                    (
                        new Serie()
                        {
                            SerieId = Convert.ToInt32(row["Key"]),
                            Country = context.Countries.Where(c => c.CountryId == Convert.ToInt32(row["Country"])).FirstOrDefault(),
                            Indicator = context.Indicators.Where(c => c.IndicatorId == Convert.ToInt32(row["Indicator"])).FirstOrDefault(),
                            Comment = row["Comment"].ToString()
                        }
                    );
            }
            context.SaveChanges();
            Console.WriteLine(" \t Done! Transfer: " + series.Rows.Count);
        }

        static void TransferObservables(DbTimeSeriesContext context, string csvContent)
        {
            Console.Write("Трансфер Observables => SQL \t...");

            context.Observables.RemoveRange(context.Observables);
            var observables = TransferService.CsvTransfer.GetFileTableCSV(csvContent, ',');
            foreach (DataRow row in observables.Rows)
            {
                var tt = row["Time"];
                var dd = row["Value"];
                context.Observables.Add
                    (
                        new Observable()
                        {
                            Serie = context.Series.Where(s => s.SerieId == Convert.ToInt32(row["SerieKey"])).FirstOrDefault(),
                            Time = new DateTime(Convert.ToInt32(row["Time"]), 1, 1),
                            ObservableValue = double.Parse(row["Value"].ToString(), CultureInfo.InvariantCulture)
                        }
                    );
            }
            context.SaveChanges();
            Console.WriteLine(" \t Done! Transfer: " + observables.Rows.Count);
        }

        #endregion
    }
}
