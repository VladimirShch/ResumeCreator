using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator.DataAccess;

namespace ResumeCoverLetterCreator.Pages.Edit
{
    public class OptionModel : PageModel
    {
        private readonly ResumeCreatorDbContext _resumeContext;

        public OptionModel(ResumeCreatorDbContext resumeContext)
        {
            _resumeContext = resumeContext;
        }

        public GroupOptionsItem Option { get; set; } = new GroupOptionsItem();

        public void OnGet(int? id, int groupId)
        {
            var optionToEdit = _resumeContext.GroupOptions
                .Include(o => o.TagGroup)
                .ThenInclude(g => g.DocumentTags)
                .ThenInclude(t => t.TagContent)
                .Include(o => o.TagContent)
                .FirstOrDefault(o => id.HasValue && o.Id == id);
            
            if(optionToEdit is not null)
            {
                Option = optionToEdit;
            }
            else {
                var parentGroup = _resumeContext.TagGroups
                    .Include(g => g.DocumentTags)
                    .ThenInclude(t => t.TagContent)
                    .FirstOrDefault(g => groupId > 0 && g.Id == groupId);
                if(parentGroup is not null)
                {
                    Option = new GroupOptionsItem { TagGroup = parentGroup };
                }
            }
        }

        public IActionResult OnPostSave(int id, int groupId, string name, Dictionary<int,int> tagImplementations)
        {
            // TODO: correct validation and processing
            if (!((id > 0 || groupId > 0) && !string.IsNullOrEmpty(name)))
                return RedirectToPage();

            GroupOptionsItem? optionToAddOrEdit = null;
            // Option is a new one
            if (id <= 0 && groupId > 0)
            {
                var parentGroup = _resumeContext.TagGroups.FirstOrDefault(g => g.Id == groupId);
                if(parentGroup is not null)
                {
                    optionToAddOrEdit = new GroupOptionsItem
                    {
                        TagGroup = parentGroup,
                        Name = name
                    };

                    _resumeContext.GroupOptions.Add(optionToAddOrEdit);
                }   
            }
            // Existing
            else if (id > 0)
            {
                optionToAddOrEdit = _resumeContext.GroupOptions
                   .Include(o => o.TagContent)
                   .FirstOrDefault(o => o.Id == id);               
            }

            if(optionToAddOrEdit is not null)
            {
                var implementations = _resumeContext.TagContent.Where(c => tagImplementations.Select(i => i.Value).Contains(c.Id)).ToList();
                optionToAddOrEdit.TagContent = implementations;
                _resumeContext.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
