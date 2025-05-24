namespace TaskManagment.Core.ViewModel
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string? Description { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string? Location { set; get; }
    }
}
