using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BaChallenge.Models
{
    public class Kindergarten
    {
        public Kindergarten(int id, string name, List<Group> groups)
        {
            Id = id;
            Name = name;
            Groups = groups;
            Groups.ForEach(x => x.Kindergarten = this);
        }

        public int Id { get; }
        public string Name { get; }
        public string NameAbr => Name.Substring(0, 3);
        public List<Group> Groups { get; }

        public class Group
        {
            public Kindergarten Kindergarten { get; set; }
            public LanguageStruct Language { get; }
            public int AgeGroupTypeId { get; }
            public string AgeGroupTypeName { get; }
            public decimal? AgeFrom { get; }
            public decimal? AgeTo { get; }
            public int ChildrenCount { get; }
            public int VacanciesCount { get; }

            public Group(int ageGroupTypeId, string ageGroupTypeName, LanguageStruct language, int childrenCount, int vacanciesCount)
            {
                AgeGroupTypeId = ageGroupTypeId;
                AgeGroupTypeName = ageGroupTypeName;

                var matches = Regex.Matches(AgeGroupTypeName, @"[\d,]+");
                AgeFrom = decimal.TryParse(matches[0].Value, out var ageFrom) ? (decimal?)ageFrom : null;
                if (matches.Count > 1)
                {
                    AgeTo = decimal.TryParse(matches[1].Value, out var ageTo) ? (decimal?)ageTo : null;
                }

                Language = language;
                ChildrenCount = childrenCount;
                VacanciesCount = vacanciesCount;
            }

            public string GetAbbreviation()
            {
                var ageRange = $"{AgeFrom}-{AgeTo}".TrimEnd('-');
                return $"{Kindergarten.NameAbr}_{ageRange}_{Language.Abr}";
            }
        }
    }
}