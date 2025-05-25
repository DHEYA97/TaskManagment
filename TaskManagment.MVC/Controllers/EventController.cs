using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using System.Security.Claims;
using TaskManagment.Core.Abstractions;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Factore;
using TaskManagment.Core.ViewModel;
using TaskManagment.Mvc.Extensions;
using TaskManagment.Mvc.Filters;

namespace TaskManagment.Mvc.Controllers
{
    [Authorize(Roles = $"{DefaultRoles.Manager},{DefaultRoles.Admin}")]
    public class EventController(IEventService eventService) : Controller
    {
        private readonly IEventService _eventService = eventService;

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> List()
        {
            var events = new List<EventViewModel>
            {
                new EventViewModel()
            };
            return View(events);
        }
        public IActionResult Calendar()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEvent = await _eventService.GetAllEventsAsync(new EventSpecification());
            return Json(new { success = true, message = allEvent });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = ModelState.GetErrorsAsString() });
            try
            {
                model.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                var Event = await _eventService.AddEventAsync(model, new EventSpecification(model.Id));
                return Json(new { success = true ,result = Event.Id});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = ModelState.GetErrorsAsString() });

            try
            {
                EventSpecification eventSpecification = new EventSpecification(model.Id);
                var existingEvent = await _eventService.GetEventByIdAsync(eventSpecification);
                if (existingEvent == null)
                    return Json(new { success = false, message = "الحدث غير موجود." });

                model.UpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                await _eventService.UpdateEventAsync(model,eventSpecification);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                EventSpecification eventSpecification = new EventSpecification(id);
                var eventToDelete = await _eventService.GetEventByIdAsync(eventSpecification);
                if (eventToDelete == null)
                    return Json(new { success = false, message = "الحدث غير موجود." });
                await _eventService.DeleteEventAsync(eventSpecification);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
