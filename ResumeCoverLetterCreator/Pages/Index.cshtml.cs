using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator.DataAccess;
using ResumeCoverLetterCreator.Services;
using System.Globalization;
using System.IO.Compression;

namespace ResumeCoverLetterCreator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ResumeCreatorDbContext _resumeDbContext;
        private readonly IDocumentTemplateService _documentTemplateService;
        private readonly IDocumentProcessingService _documentProcessingService;

        public IndexModel(ResumeCreatorDbContext resumeDbContext, 
            IDocumentTemplateService documentTemplateService,
            IDocumentProcessingService documentProcessingService)
        {
            _resumeDbContext = resumeDbContext;
            _documentTemplateService = documentTemplateService;
            _documentProcessingService = documentProcessingService;
        }

        public IEnumerable<TagGroup> TagGroups { get; set; } = Enumerable.Empty<TagGroup>();
        
        public void OnGet()
        {
            TagGroups = _resumeDbContext.TagGroups.Include(tg => tg.GroupOptions).ThenInclude(go => go.TagContent).ThenInclude(tc => tc.DocumentTag);
        }

        public IActionResult OnPostPrepareDocuments(string fullName, string companyFor, string positionFor, string hiringManager, Dictionary<string,string> values)
        {
            var selectedOptions = new List<int>();
            foreach(var inputValue in values)
            {
                if(int.TryParse(inputValue.Key, out int key) && int.TryParse(inputValue.Value, out int value))
                {
                    selectedOptions.Add(value);
                }
            }
            var tagPairs = _resumeDbContext.GroupOptions.Where(go => selectedOptions.Any(t => t == go.Id)).Include(go => go.TagContent).ThenInclude(tc => tc.DocumentTag)
                .SelectMany(go => go.TagContent.Select(tc => KeyValuePair.Create(tc.DocumentTag.TagName, tc.Content)));

            var tags = new Dictionary<string, string>(tagPairs);
            fullName = tags["$name_surname"] = fullName ?? "Name Surname";
            tags["$company_name"] = companyFor ?? string.Empty;
            tags["$position_name"] = positionFor ?? string.Empty;
            tags["$hiring_manager"] = hiringManager ?? "Hiring Manager";

            var currendDate = DateTime.Now;
            var dateTimeFormat = new CultureInfo("en-US").DateTimeFormat;
            tags["$month_date"] = currendDate.ToString("M", dateTimeFormat);
            tags["$year"] = currendDate.Year.ToString();

            var turkeyLivingStartDate = new DateTime(2022, 10, 10);
            int howLongInTurkeyInMonths = (currendDate.Year - turkeyLivingStartDate.Year) * 12 + currendDate.Month - turkeyLivingStartDate.Month - (currendDate.Month == turkeyLivingStartDate.Month && currendDate.Day < turkeyLivingStartDate.Day ? 1 : 0);
            tags["$month_quantity"] = NumberToWord(howLongInTurkeyInMonths);
            //---

            byte[] cvTemplateContent = _documentTemplateService.GetTemplate("CV_Template.docx");
            byte[] cvResultContent = _documentProcessingService.ProcessDocument(cvTemplateContent, tags);

            byte[] letterTemplateContent = _documentTemplateService.GetTemplate("Cover_Letter_Template.docx");
            byte[] letterResultContent = _documentProcessingService.ProcessDocument(letterTemplateContent, tags);

            byte[] res = Array.Empty<byte>();
            using (MemoryStream archiveStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    var cvEntry = zipArchive.CreateEntry(fullName + "_CV.docx");
                    using (var zipStream = cvEntry.Open())
                    {
                        zipStream.Write(cvResultContent, 0, cvResultContent.Length);
                    }

                    var letterEntry = zipArchive.CreateEntry(fullName + "_Cover_Letter.docx");
                    using (var zipStream = letterEntry.Open())
                    {
                        zipStream.Write(letterResultContent, 0, letterResultContent.Length);
                    }

                }
                res = archiveStream.ToArray();
            }
            
            return File(res.ToArray(), "application/zip", (companyFor ?? fullName) + ".zip");
        }

        // Move to separate service
        private string NumberToWord(int number)
        {
            return number switch
            {
                2 => "two",
                3 => "three",
                4 => "four",
                5 => "five",
                6 => "six",
                7 => "seven",
                8 => "eight",
                9 => "nine",
                10 => "ten",
                11 => "eleven",
                12 => "twelve",
                13 => "thirteen",
                14 => "fourteen",
                15 => "fifteen",
                16 => "sixteen",
                17 => "seventeen",
                18 => "eighteen",
                19 => "nineteen",
                20 => "twenty",
                _ => "several"
            };
        }
    }
}
