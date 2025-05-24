using FluentValidation;
using TaskManagment.Core.Interfaces;
using TaskManagment.Core.Service;
using TaskManagment.Core.Specification.EntitySpecification;
using TaskManagment.Core.ViewModel;
namespace TaskManagment.Core.Validations
{
    public class EventValidator : AbstractValidator<EventFormViewModel>
    {
        private readonly IEventService _eventService;
        private readonly IDateTimeService _dateTimeService;
        public EventValidator(IEventService eventService , IDateTimeService dateTimeService)
        {
            _eventService = eventService;
            _dateTimeService = dateTimeService;
            RuleFor(l => l.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("يجب كتابة {PropertyName}.")
                .MinimumLength(1)
                .WithMessage("{PropertyName} يجب أن يكون أطول من {MinLength} أحرف.")
                .MaximumLength(100)
                .WithMessage("{PropertyName} لا يجب أن يتجاوز {MaxLength} حرفًا.");

            RuleFor(l => l.StartDate)
                .Must((model, StartDate) => StartDate > _dateTimeService.GetNowKsa().AddMinutes(-1))
                .WithMessage(model => $"{_dateTimeService.GetNowKsa().AddMinutes(-1)} وقت بداية المهمة يجب أن يكون أكبر من.");

            RuleFor(l => l.EndDate)
                .Must((model, endDate) => endDate > _dateTimeService.GetNowKsa().AddMinutes(-1))
                .WithMessage(model => $"{_dateTimeService.GetNowKsa().AddMinutes(-1)} وقت نهاية المهمة يجب أن يكون أكبر من.")
                .Must((model, endDate) => endDate > model.StartDate)
                .WithMessage(model => $"{model.StartDate} وقت نهاية المهمة يجب أن يكون أكبر من.")
                .When(x => x.StartDate != default);

        }
    }
}