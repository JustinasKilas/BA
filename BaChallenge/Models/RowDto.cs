namespace BaChallenge.Models
{
    public class RowDto
    {
        public int SchoolId { get; }
        public string SchoolName { get; }
        public int TypeId { get; }
        public string TypeLabel { get; }
        public LanguageStruct Language { get; }
        public int ChildrenCount { get; }
        public int FreeSpace { get; }

        public RowDto(string id, string schoolName, string typeId, string typeLabel, string lanId, string languageLabel, string childrenCount, string freeSpace)
        {
            SchoolId = int.Parse(id);
            SchoolName = schoolName.Trim('"');
            TypeId = int.Parse(typeId);
            TypeLabel = typeLabel;
            Language = new LanguageStruct(int.Parse(lanId), languageLabel);
            ChildrenCount = int.Parse(childrenCount);
            FreeSpace = int.Parse(freeSpace);
        }
    }
}