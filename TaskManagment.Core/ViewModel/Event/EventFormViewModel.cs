using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagment.Core.Abstractions.Const;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.Core.ViewModel
{
    public class EventFormViewModel
    {
        public EventFormViewModel()
        {
            SelectTemplatesId = new List<int>();
            Templates = new List<SelectListItem>();
        }
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MaxLength), Display(Name = "Event")]
        public string Name { get; set; } = null!;
        [Display(Name = "Template")]
        public IList<int> SelectTemplatesId { get; set; }
        public IEnumerable<SelectListItem> Templates { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventEndDate { set; get; }
        public DateTime EventBeginRegisterDate { get; set; }
    }
}