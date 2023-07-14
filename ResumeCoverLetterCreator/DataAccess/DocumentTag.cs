namespace ResumeCoverLetterCreator.DataAccess
{
    public class DocumentTag
    {
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty;
        public ICollection<TagContentItem> TagContent { get; set; } = new List<TagContentItem>();
    }
}
