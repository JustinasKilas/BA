using System.Collections.Generic;
using System.IO;
using BaChallenge.Models;

namespace BaChallenge.Helpers
{
    public interface ICsvReader
    {
        List<RowDto> ReadFile(string filePath);
    }

    public class CsvReader : ICsvReader
    {
        public List<RowDto> ReadFile(string filePath)
        {
            var items = new List<RowDto>();

            //csv reader could be used instead
            using (var reader = new StreamReader(filePath))
            {
                //Ignores first header line
                _ = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    items.Add(new RowDto(values[0], values[1].Trim('"'),
                        values[2], values[3], values[4], values[5],
                        values[6], values[7]));
                }
            }

            return items;
        }
    }
}