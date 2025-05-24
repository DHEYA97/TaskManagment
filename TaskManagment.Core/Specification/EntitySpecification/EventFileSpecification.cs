using TaskManagment.Core.Entities;

namespace TaskManagment.Core.Specification.EntitySpecification
{
    public class EventFileSpecification : BaseSpecification<EventFile>
    {
        public EventFileSpecification()
        {

        }
        public EventFileSpecification(int id) : base(x => x.Id == id)
        {

        }
        public EventFileSpecification(int Eventid,int id = 0) : base(x => x.EventId == Eventid)
        {

        }
    }
}
