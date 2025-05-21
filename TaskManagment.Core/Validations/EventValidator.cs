using FluentValidation;
using TaskManagment.Core.Service;
using TaskManagment.Core.Specification.EntitySpecification;
using TaskManagment.Core.ViewModel;
namespace TaskManagment.Core.Validations
{
    public class EventValidator : AbstractValidator<EventFormViewModel>
    {
        private readonly IEventService _eventService;
        public EventValidator(IEventService eventService)
        {
            _eventService = eventService;
            RuleFor(l => l.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("يجب كتابة {PropertyName}.")
                .MinimumLength(1)
                .WithMessage("{PropertyName} يجب أن يكون أطول من {MinLength} أحرف.")
                .MaximumLength(100)
                .WithMessage("{PropertyName} لا يجب أن يتجاوز {MaxLength} حرفًا.");

            RuleFor(l => l.EventDate)
            .GreaterThan(DateTime.Today.AddDays(-1))
            .WithMessage("تاريخ الحدث يجب أن يكون اكبر من تاريخ اليوم.")
            .When(x => x.Id < 1);

            RuleFor(l => l.EventBeginRegisterDate)
                .GreaterThan(DateTime.Today.AddDays(-1))
                .WithMessage("تاريخ بداية التسجيل يجب أن يكون اكبر من تاريخ اليوم.")
                .When(x => x.Id < 1)
                .LessThanOrEqualTo(l => l.EventDate)
                .WithMessage("تاريخ بداية التسجيل يجب أن يكون أقل من تاريخ الحدث.")
                .When(x => x.EventDate != default);

            RuleFor(l => l.EventEndDate)
                .GreaterThan(DateTime.Today.AddDays(-1))
                .WithMessage("تاريخ نهاية الحدث  يجب أن يكون اكبر من تاريخ اليوم.")
                .When(x => x.EventDate != default)
                .GreaterThanOrEqualTo(l => l.EventDate)
                .WithMessage("تاريخ نهاية الحدث يجب أن يكون اكبر من تاريخ بداية الحدث.")
                .When(x => x.EventDate != default);

            RuleFor(l => l.SelectTemplatesId)
            .NotEmpty()
            .WithMessage("يجب اختيار قوالب.")
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage("لا يمكن أن تحتوي القوالب المختارة على قيم مكررة.");

        }
    }
}