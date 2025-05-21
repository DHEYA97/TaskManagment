namespace TaskManagment.Core.ViewModel
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { set; get; }
        public DateTime EventDate { get; set; }
        public DateTime EventEndDate { set; get; }
        public DateTime EventBeginRegisterDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
