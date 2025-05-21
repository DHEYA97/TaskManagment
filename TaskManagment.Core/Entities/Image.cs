namespace TaskManagment.Core.Entities
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}