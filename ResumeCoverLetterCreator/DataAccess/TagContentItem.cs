namespace ResumeCoverLetterCreator.DataAccess
{
    public class TagContentItem
    {
        public int Id { get; set; }
        public DocumentTag DocumentTag { get; set; } = new DocumentTag();
        public string Content { get; set; } = string.Empty;
        // Every tag implementation may belong to multiple group options.
        public ICollection<GroupOptionsItem> GroupOptions { get; set; } = new List<GroupOptionsItem>();
    }
}
