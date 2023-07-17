namespace ResumeCoverLetterCreator.DataAccess
{
    public class TagGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<GroupOptionsItem> GroupOptions { get; set; } = new List<GroupOptionsItem>();
        public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    }
}
