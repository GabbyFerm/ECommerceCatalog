using FluentValidation;
using ECommerceCatalog.DTOs;

namespace ECommerceCatalog.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator() 
        { 
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(product => product.CategoryName)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");
        }
    }
}
