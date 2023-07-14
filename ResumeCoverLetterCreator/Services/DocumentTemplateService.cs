namespace ResumeCoverLetterCreator.Services
{
    //DatAc
    public class DocumentTemplateService : IDocumentTemplateService
    {
        private readonly string _searchRoot;
        public DocumentTemplateService(string? searchRoot)
        {
            _searchRoot = searchRoot ?? string.Empty;
        }

        public byte[] GetTemplate(string name)
        {
            byte[] templateContent = File.ReadAllBytes($"{_searchRoot}//{name}");
            return templateContent;
        }
    }
}
