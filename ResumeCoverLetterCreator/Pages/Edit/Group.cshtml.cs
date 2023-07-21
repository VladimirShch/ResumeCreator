using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator.DataAccess;

namespace ResumeCoverLetterCreator.Pages.Edit
{
    public class GroupModel : PageModel
    {
        private readonly ResumeCreatorDbContext _resumeDbContext;

        public GroupModel(ResumeCreatorDbContext resumeDbContext)
        {
            _resumeDbContext = resumeDbContext;
        }

        public TagGroup Group { get; set; } = new TagGroup();

        public void OnGet(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Invalid Id");
            }
            Group = _resumeDbContext.TagGroups.Include(tg => tg.DocumentTags).Include(tg => tg.GroupOptions).FirstOrDefault(tg => tg.Id == id) ?? new TagGroup();
        }

        public IActionResult OnPostChangeName(int id, string newName)
        {
            TagGroup? groupToChange = _resumeDbContext.TagGroups.FirstOrDefault(tg => id > 0 && tg.Id == id);
            if(groupToChange is not null)
            {
                groupToChange.Name = newName;
                _resumeDbContext.SaveChanges();
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostAddTag(int groupId, string name)
        {
            if(!string.IsNullOrEmpty(name) && name.Length > 1 && name.StartsWith('$'))
            {
                var group = _resumeDbContext.TagGroups.FirstOrDefault(g => g.Id == groupId);
                if(group is not null)
                {
                    _resumeDbContext.DocumentTags.Add(new DocumentTag
                    {
                        TagGroup = group,
                        TagName = name
                    }) ;

                    _resumeDbContext.SaveChanges();
                }                   
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteTag(int id)
        {

            DocumentTag? tagToDelete = _resumeDbContext.DocumentTags.FirstOrDefault(tg => id > 0 && tg.Id == id);
            if (tagToDelete is not null)
            {
                _resumeDbContext.DocumentTags.Remove(tagToDelete);
                _resumeDbContext.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteOption(int id)
        {

            GroupOptionsItem? optionToDelete = _resumeDbContext.GroupOptions.FirstOrDefault(tg => id > 0 && tg.Id == id);
            if (optionToDelete is not null)
            {
                _resumeDbContext.GroupOptions.Remove(optionToDelete);
                _resumeDbContext.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
