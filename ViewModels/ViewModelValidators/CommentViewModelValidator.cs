using FluentValidation;

namespace deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator()
        {
            RuleFor(model => model.Content)
               .NotEmpty().WithMessage("Content is required.");
        }
    }
}
