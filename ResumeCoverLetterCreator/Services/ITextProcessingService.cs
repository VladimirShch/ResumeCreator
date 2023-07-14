namespace ResumeCoverLetterCreator.Services
{
    public interface ITextProcessingService
    {
        string ProcessText(string text, Dictionary<string, string> tags);
    }
}
