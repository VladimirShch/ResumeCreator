using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator.DataAccess;

namespace ResumeCoverLetterCreator.Pages.Edit
{
    public class TagModel : PageModel
    {
        private readonly ResumeCreatorDbContext _resumeDbContext;

        public TagModel(ResumeCreatorDbContext resumeDbContext)
        {
            _resumeDbContext = resumeDbContext;
        }

        public DocumentTag Tag { get; set; } = new DocumentTag();

        public void OnGet(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Id must be positive number");
            }

            Tag = _resumeDbContext.DocumentTags.Include(t => t.TagGroup).Include(t => t.TagContent).FirstOrDefault(t => id > 0 && t.Id == id) ?? new DocumentTag();
        }

        public IActionResult OnPostChangeContent(int id, string content)
        {
            var contentToChange = _resumeDbContext.TagContent.FirstOrDefault(c => c.Id == id);
            if(contentToChange is not null)
            {
                contentToChange.Content = content;
                _resumeDbContext.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostChangeName(int id, string name)
        {
            DocumentTag? tagToChange = _resumeDbContext.DocumentTags.FirstOrDefault(tg => id > 0 && tg.Id == id);
            if (tagToChange is not null)
            {
                tagToChange.TagName = name;
                _resumeDbContext.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteContent(int id, string content)
        {
            var contentToDelete = _resumeDbContext.TagContent.FirstOrDefault(c => c.Id == id);
            if (contentToDelete is not null)
            {
                _resumeDbContext.TagContent.Remove(contentToDelete);
                _resumeDbContext.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostAddContent(int tagId, string content)
        {
            var contentTag = _resumeDbContext.DocumentTags.FirstOrDefault(t => t.Id == tagId);
            if(contentTag is not null)
            {
                _resumeDbContext.TagContent.Add(new TagContentItem
                {   
                    DocumentTag = contentTag,
                    Content = content
                });

                _resumeDbContext.SaveChanges();
            }

            return RedirectToPage();

        }
    }
}
