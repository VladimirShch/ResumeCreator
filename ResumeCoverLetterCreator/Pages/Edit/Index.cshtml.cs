using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResumeCoverLetterCreator.DataAccess;

namespace ResumeCoverLetterCreator.Pages.Edit
{
    public class IndexModel : PageModel
    {
        private readonly ResumeCreatorDbContext _resumeDbContext;

        public IndexModel(ResumeCreatorDbContext resumeDbContext)
        {
            _resumeDbContext = resumeDbContext;
        }

        public IEnumerable<TagGroup> TagGroups { get; set; } = Enumerable.Empty<TagGroup>();

        public void OnGet()
        {
            TagGroups = _resumeDbContext.TagGroups;
        }
        
        public IActionResult OnPostAddGroup(string groupName = "New Group")
        {
            _resumeDbContext.TagGroups.Add(new TagGroup { Name = groupName });
            _resumeDbContext.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteGroup(int id)
        {
            TagGroup? groupToDelete = _resumeDbContext.TagGroups.FirstOrDefault(tg => id > 0 && tg.Id == id);
            if(groupToDelete is not null)
            {
                _resumeDbContext.TagGroups.Remove(groupToDelete);
                _resumeDbContext.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
