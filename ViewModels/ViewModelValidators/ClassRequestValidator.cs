using FluentValidation;

namespace deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators
{
    public class ClassRequestValidator : AbstractValidator<ClassRequest>
    {
        public ClassRequestValidator()
        {
            RuleFor(model => model.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(8).WithMessage("Title must be at least 8 characters.");

            RuleFor(model => model.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");
            RuleFor(model => model.AuthorID)
               .NotEmpty().WithMessage("Author is required.");

        }
    }
}
