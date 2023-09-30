namespace BaChallenge.Models
{
    public readonly struct LanguageStruct
    {
        public int Id { get; }
        public string Name { get; }
        public string Abr => Name.Substring(0, 4);

        public LanguageStruct(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}