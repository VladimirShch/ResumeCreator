namespace ResumeCoverLetterCreator.Services
{
    // App/Dom
    public interface IDocumentTemplateService
    {
        byte[] GetTemplate(string name);
    }
}
