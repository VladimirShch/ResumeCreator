namespace ResumeCoverLetterCreator.Services
{
    public interface IDocumentProcessingService
    {
        byte[] ProcessDocument(byte[] documentContent, Dictionary<string, string> tags);
    }
}
