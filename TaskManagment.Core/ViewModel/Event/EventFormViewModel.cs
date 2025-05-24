using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskManagment.Core.Abstractions.Const;
using TaskManagment.Core.Entities.Identity;

namespace TaskManagment.Core.ViewModel
{
    public class EventFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Name { get; set; } = null!;
        public string? Description { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string? Location { set; get; }
        public string? CreatedById { get; set; }
        public string? UpdatedById { get; set; }
    }
}