using System.Collections.Generic;
using System.Linq;

namespace BaChallenge.Models
{
    public class KindergartensStatistics
    {
        private readonly List<Kindergarten> _kindergartens;
        private readonly List<Kindergarten.Group> _groups;

        public IGrouping<int, Kindergarten.Group> GroupsWithMostChildren { get; }
        public IGrouping<int, Kindergarten.Group> GroupsWithLeastChildren { get; }
        public int TotalVacancies => _groups.Sum(x => x.VacanciesCount);

        public Dictionary<LanguageStruct, List<Kindergarten.Group>> GroupsByLanguage
            => _groups
                .GroupBy(x => x.Language)
                .ToDictionary(x => x.Key, x => x.ToList());

        public IEnumerable<Kindergarten> KindergartensHaving2To5Vacancies
            => _kindergartens
                .Where(x => x.Groups.Any(xx => xx.VacanciesCount >= 2 && xx.VacanciesCount < 5));

        private KindergartensStatistics(IEnumerable<Kindergarten> kindergartens)
        {
            _kindergartens = kindergartens.ToList();
            _groups = _kindergartens.SelectMany(x => x.Groups).ToList();

            var byCount = _groups
                .GroupBy(x => x.ChildrenCount)
                .OrderBy(x => x.Key)
                .ToList();
            GroupsWithLeastChildren = byCount.First();
            GroupsWithMostChildren = byCount.Last();
        }

        public static KindergartensStatistics CreateStatistics(IEnumerable<Kindergarten> kindergartens)
        {
            var stats = new KindergartensStatistics(kindergartens);

            return stats;
        }
    }
}