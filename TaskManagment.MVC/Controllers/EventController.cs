using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using TaskManagment.Core.Abstractions;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Factore;
using TaskManagment.Core.ViewModel;
using TaskManagment.Mvc.Extensions;
using TaskManagment.Mvc.Filters;

namespace TaskManagment.Mvc.Controllers
{
    [Authorize(Roles = $"{DefaultRoles.Manager},{DefaultRoles.Admin}")]
    public class EventController(
        IEventService eventService, IEventModelFactory eventModelFactory, 
                                 IDataProtectionProvider dataProtector) : Controller
    {
        private readonly IEventService _eventService = eventService;
        private readonly IEventModelFactory _eventModelFactory = eventModelFactory;
        private readonly IDataProtector _dataProtector = dataProtector.CreateProtector("MySecureKey");

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> List()
        {
            var events = new List<EventViewModel>
            {
                new EventViewModel
                {
                    Id = 1,
                    Name = "مؤتمر التقنية",
                    IsDeleted = false,
                    EventDate = new DateTime(2025, 6, 1),
                    EventEndDate = new DateTime(2025, 6, 3),
                    EventBeginRegisterDate = new DateTime(2025, 5, 15),
                    CreatedOn = DateTime.Now.AddMonths(-1),
                    UpdatedOn = DateTime.Now.AddDays(-2)
                },
                new EventViewModel
                {
                    Id = 2,
                    Name = "دورة الذكاء الاصطناعي",
                    IsDeleted = false,
                    EventDate = new DateTime(2025, 7, 10),
                    EventEndDate = new DateTime(2025, 7, 12),
                    EventBeginRegisterDate = new DateTime(2025, 6, 25),
                    CreatedOn = DateTime.Now.AddMonths(-2),
                    UpdatedOn = null
                },
                new EventViewModel
                {
                    Id = 3,
                    Name = "معرض البرمجة",
                    IsDeleted = true,
                    EventDate = new DateTime(2025, 4, 5),
                    EventEndDate = new DateTime(2025, 4, 6),
                    EventBeginRegisterDate = new DateTime(2025, 3, 20),
                    CreatedOn = DateTime.Now.AddMonths(-3),
                    UpdatedOn = DateTime.Now.AddDays(-10)
                }
            };
            return View(events);
        }
        public IActionResult Calendar()
        {
            return View();
        }
        //[HttpGet]
        //[AjaxOnly]
        //public async Task<IActionResult> Create()
        //{
        //    var viewModel = await _eventModelFactory.PrepareEventFormViewModel();
        //    return PartialView("_Form", viewModel);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(EventFormViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorsAsString());
        //    model.EventBeginRegisterDate = TimeZoneInfo.ConvertTimeFromUtc(
        //        model.EventBeginRegisterDate,
        //        TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")
        //    );
        //    model.EventEndDate = TimeZoneInfo.ConvertTimeFromUtc(
        //        model.EventEndDate,
        //        TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")
        //    );
        //    model.EventBeginRegisterDate = model.EventBeginRegisterDate.Date;
        //    var Event = await _eventService.AddEventAsync(model, new EventSpecification(model.Id));
        //    var viewModel = Event.Adapt<EventViewModel>();
        //    return PartialView("_EventRow", viewModel);
        //}
        //[HttpGet]
        //[AjaxOnly]
        //public async Task<IActionResult> Edit(int Id)
        //{
        //    var Event = await _eventService.GetEventEntityByIdAsync(new EventSpecification(Id));
        //    if (Event is null)
        //        return NotFound();
        //    var viewModel = await _eventModelFactory.PrepareEventFormViewModel(Event);
        //    return PartialView("_Form", viewModel);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(EventFormViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorsAsString());
        //    var Event = await _eventService.UpdateEventAsync(model, new EventSpecification(model.Id));
        //    var viewModel = Event.Adapt<EventViewModel>();
        //    return PartialView("_EventRow", viewModel);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int Id)
        //{
        //    var Event = await _eventService.GetEventByIdAsync(new EventSpecification(Id));
        //    if (Event is null)
        //        return NotFound();
        //    await _eventService.DeleteEventAsync(new EventSpecification(Id));
        //    Event = await _eventService.GetEventByIdAsync(new EventSpecification(Id));
        //    return Ok(true);
        //}
    }
}
