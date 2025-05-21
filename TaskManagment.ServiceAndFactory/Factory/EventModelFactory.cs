using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Factore;
using TaskManagment.Core.Service;
using TaskManagment.Core.Specification.EntitySpecification;
using TaskManagment.Core.ViewModel;
namespace TaskManagment.ServiceAndFactore.Factore
{
    public class EventModelFactory() : IEventModelFactory
    {
        public async Task<EventFormViewModel> PrepareEventFormViewModel(Event? Event = null, EventFormViewModel? model = null)
        {
            EventFormViewModel viewModel = model ?? new EventFormViewModel();
            //if (Event != null)
            //{
            //    viewModel = Event.Adapt<EventFormViewModel>();
            //    viewModel.SelectTemplatesId = Event.EventTemplateMaps?.Select(x => x.TemplateId)?.ToList();
            //}
            //else
            //{
            //    viewModel.EventDate = viewModel.EventEndDate = viewModel.EventBeginRegisterDate = DateTime.Now;
            //}
            //var templates = await _templateService.GetAllTemplatesAsync(new TemplateSpecification(includeDeleted: false));
            //viewModel.Templates = templates.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});
            return viewModel;
        }
    }
}
