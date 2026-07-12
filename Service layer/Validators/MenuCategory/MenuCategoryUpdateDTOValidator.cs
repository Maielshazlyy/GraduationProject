using FluentValidation;
using Service_layer.DTOS.MenuCategory;

namespace Service_layer.Validators.MenuCategory
{
    public class MenuCategoryUpdateDTOValidator : AbstractValidator<MenuCategoryUpdateDTO>
    {
        public MenuCategoryUpdateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
        }
    }
}

