using FluentValidation;

namespace deha_exam_quanlykhoahoc.ViewModels.ViewModelValidators
{
    public class CommentRequestValidator : AbstractValidator<CommentRequest>
    {
        public CommentRequestValidator() {
            RuleFor(model => model.Content)
               .NotEmpty().WithMessage("Content is required.");
        }
    }
}
