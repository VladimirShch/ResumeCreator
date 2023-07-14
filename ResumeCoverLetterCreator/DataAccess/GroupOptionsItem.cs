namespace ResumeCoverLetterCreator.DataAccess
{
    public class GroupOptionsItem
    {
        public int Id { get; set; }
        public TagGroup TagGroup { get; set; } = new TagGroup();
        public string Name { get; set; } = string.Empty;
        // Every option may content multiple tags, but strictly one impelentation of each tag
        public ICollection<TagContentItem> TagContent { get; set; } = new List<TagContentItem>();
    }
}
