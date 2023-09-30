using System;
using System.Globalization;
using System.IO;
using System.Linq;
using BaChallenge.Helpers;
using BaChallenge.Models;

namespace BaChallenge
{
    public class Challenge3
    {
        private readonly ICsvReader _csvReader;

        public Challenge3()
        {
            _csvReader = new CsvReader();
        }

        public Challenge3(ICsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        public void Execute(string csvFilePath)
        {
            //set lt culture for numbers format when parsing decimal
            using (new CultureScope(new CultureInfo("lt-LT")))
            {
                var rows = _csvReader.ReadFile(csvFilePath);

                var kindergartens = rows
                    .GroupBy(
                        x => new { x.SchoolId, x.SchoolName },
                        x => new Kindergarten.Group(x.TypeId, x.TypeLabel, x.Language, x.ChildrenCount, x.FreeSpace))
                    .Select(x => new Kindergarten(x.Key.SchoolId, x.Key.SchoolName, x.ToList()))
                    .ToList();

                var statistics = KindergartensStatistics.CreateStatistics(kindergartens);
                WriteToStream(statistics, Console.Out, new StreamWriter("Result.txt"));
            }
        }

        private static void WriteToStream(KindergartensStatistics statistics, params TextWriter[] streams)
        {
            streams.WriteLine($"Kindergartens with max value of {statistics.GroupsWithMostChildren.Key}:");
            foreach (var item in statistics.GroupsWithMostChildren)
            {
                streams.WriteLine(item.GetAbbreviation());
            }
            streams.WriteLine();
            streams.WriteLine($"Kindergartens with min value of {statistics.GroupsWithLeastChildren.Key}:");
            foreach (var item in statistics.GroupsWithLeastChildren)
            {
                streams.WriteLine(item.GetAbbreviation());
            }

            streams.WriteLine();
            streams.WriteLine("Free spots by language:");
            foreach (var kvPair in statistics.GroupsByLanguage)
            {
                var language = kvPair.Key;
                var groups = kvPair.Value;
                streams.WriteLine($"{language.Name}  {Math.Round(groups.Sum(x => x.VacanciesCount) * 1.0 / statistics.TotalVacancies * 100, 2)}%");
            }

            streams.WriteLine();
            streams.WriteLine("Schools with 2 to 4 free spots:");
            foreach (var x in statistics.KindergartensHaving2To5Vacancies.OrderByDescending(x => x.Name))
            {
                streams.WriteLine(x.Name);
            }
        }
    }
}