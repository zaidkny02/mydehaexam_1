using FluentValidation;

namespace deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators
{
    public class LessonViewModelValidator : AbstractValidator<LessonViewModel>
    {
        public LessonViewModelValidator() {
            RuleFor(model => model.Title)
               .NotEmpty().WithMessage("Title is required.")
               .Must(mytitlevalidator).WithMessage("Title's length must > 7");



            RuleFor(model => model.Introduction)
                .NotEmpty().WithMessage("Introduction is required.")
                .MinimumLength(2).WithMessage("Must be at least 2 characters.");

            RuleFor(model => model.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(model => model.DateCreated)
                .NotEmpty().WithMessage("DateCreated is required.");

            RuleFor(model => model.ClassID)
                .NotEmpty().WithMessage("Class is required.");
        }

        private bool mytitlevalidator(string title)
        {
            return title.Count() > 7;
        }
    }
}
