using TaskManagment.Core.Entities.Identity;

namespace TaskManagment.Core.Entities
{
    public class Event : BaseEntity
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Location { set; get; }
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
        public string? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
