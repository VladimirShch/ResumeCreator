using OfficeIMO.Word;

namespace ResumeCoverLetterCreator.Services
{
    public class DocumentProcessingService : IDocumentProcessingService
    {
        private readonly ITextProcessingService _textProcessingService;

        public DocumentProcessingService(ITextProcessingService textProcessingService)
        {
            _textProcessingService = textProcessingService;
        }

        public byte[] ProcessDocument(byte[] documentContent, Dictionary<string, string> tags)
        {
            byte[] contentCopy = new byte[documentContent.Length];
            documentContent.CopyTo(contentCopy, 0);

            byte[] resultBytes = Array.Empty<byte>();
            using (var documentContentStream = new MemoryStream(contentCopy))
            {
                var document = WordDocument.Load(documentContentStream);

                foreach(WordParagraph paragraph in document.Paragraphs)
                {
                    paragraph.Text = _textProcessingService.ProcessText(paragraph.Text, tags);

                }
                using MemoryStream resultStream = new();
                document.Save(resultStream);
                resultBytes = resultStream.ToArray();
            }
            return resultBytes;

            //using (var documentContentStream = new MemoryStream(contentCopy))
            //{
            //    using (var document = WordprocessingDocument.Open(documentContentStream, true))
            //    {
            //        //var body = document.MainDocumentPart.Document.Body;
            //        //var paras = body.Elements<Paragraph>();

            //        //foreach (var para in paras)
            //        //{
            //        //    foreach (var run in para.Elements<Run>())
            //        //    {
            //        //        foreach (var text in run.Elements<Text>())
            //        //        {
            //        //            if (text.Text.Contains("$position"))
            //        //            {
            //        //                text.Text = text.Text.Replace("$position", "инженерама");
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //    }
            //}
            
                      
            //return documentContent;

            //document.Save();

            //var resultStream = new MemoryStream();
            //document.MainDocumentPart.GetStream().CopyTo(resultStream);
            //return resultStream.ToArray();
        }
    }
}
