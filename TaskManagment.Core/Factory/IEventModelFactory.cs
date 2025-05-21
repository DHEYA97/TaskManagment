using TaskManagment.Core.Entities;
using TaskManagment.Core.ViewModel;

namespace TaskManagment.Core.Factore
{
    public interface IEventModelFactory
    {
        Task<EventFormViewModel> PrepareEventFormViewModel(Event? Event = null, EventFormViewModel? model = null);
    }
}
