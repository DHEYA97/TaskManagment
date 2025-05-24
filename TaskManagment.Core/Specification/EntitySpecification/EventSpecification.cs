using Microsoft.EntityFrameworkCore;
using TaskManagment.Core.Entities;

namespace TaskManagment.Core.Specification.EntitySpecification
{
    public class EventSpecification : BaseSpecification<Event>
    {
        public EventSpecification()
        {
            Includes.Add(e => e.EventFiles);
        }
        public EventSpecification(int id) : base(x => x.Id == id)
        {
            Includes.Add(e => e.EventFiles);
        }
    }
}
