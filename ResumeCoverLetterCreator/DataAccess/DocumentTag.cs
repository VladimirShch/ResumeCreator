namespace ResumeCoverLetterCreator.DataAccess
{
    public class DocumentTag
    {
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty;
        public TagGroup TagGroup { get; set; } = new TagGroup();
        public ICollection<TagContentItem> TagContent { get; set; } = new List<TagContentItem>();
    }
}
