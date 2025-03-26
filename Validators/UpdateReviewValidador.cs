using ECommerceCatalog.DTOs;
using FluentValidation;

namespace ECommerceCatalog.Validators
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewDTO>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Rating.HasValue);

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
