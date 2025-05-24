namespace TaskManagment.Core.Entities
{
    public class EventFile : BaseEntity
    {
        public string FilePath { get; set; }          
        public string ContentType { get; set; }        
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}