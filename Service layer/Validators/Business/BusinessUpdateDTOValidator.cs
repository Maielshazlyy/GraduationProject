using FluentValidation;
using Service_layer.DTOS.Business;

namespace Service_layer.Validators.Business
{
    public class BusinessUpdateDTOValidator : AbstractValidator<BusinessUpdateDTO>
    {
        public BusinessUpdateDTOValidator()
        {
            // Basic Information (Optional in Update)
            RuleFor(x => x.Name)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Name))
                .WithMessage("Business name cannot exceed 200 characters.");

            RuleFor(x => x.Type)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Type))
                .WithMessage("Business type cannot exceed 100 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Address))
                .WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone number cannot exceed 20 characters.");

            // Contact Information (Optional)
            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Invalid email format.")
                .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.");

            RuleFor(x => x.Website)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.Website))
                .WithMessage("Invalid website URL format.")
                .MaximumLength(500).WithMessage("Website URL cannot exceed 500 characters.");

            RuleFor(x => x.FacebookUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.FacebookUrl))
                .WithMessage("Invalid Facebook URL format.")
                .MaximumLength(500).WithMessage("Facebook URL cannot exceed 500 characters.");

            RuleFor(x => x.InstagramUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.InstagramUrl))
                .WithMessage("Invalid Instagram URL format.")
                .MaximumLength(500).WithMessage("Instagram URL cannot exceed 500 characters.");

            // Location (Optional)
            RuleFor(x => x.City)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.City))
                .WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Country))
                .WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue)
                .WithMessage("Longitude must be between -180 and 180.");

            // Restaurant Information (Optional)
            RuleFor(x => x.Description)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description cannot exceed 2000 characters.");

            RuleFor(x => x.CuisineType)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.CuisineType))
                .WithMessage("Cuisine type cannot exceed 100 characters.");

            RuleFor(x => x.PriceRange)
                .MaximumLength(10).When(x => !string.IsNullOrWhiteSpace(x.PriceRange))
                .WithMessage("Price range cannot exceed 10 characters.");

            RuleFor(x => x.LogoUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.LogoUrl))
                .WithMessage("Invalid logo URL format.")
                .MaximumLength(500).WithMessage("Logo URL cannot exceed 500 characters.");

            RuleFor(x => x.CoverImageUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrWhiteSpace(x.CoverImageUrl))
                .WithMessage("Invalid cover image URL format.")
                .MaximumLength(500).WithMessage("Cover image URL cannot exceed 500 characters.");

            // Payment Methods (Optional)
            RuleFor(x => x.PaymentMethods)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.PaymentMethods))
                .WithMessage("Payment methods cannot exceed 200 characters.");

            // Working Hours Validation
            RuleForEach(x => x.WorkingHours)
                .ChildRules(wh =>
                {
                    wh.RuleFor(h => h.DayOfWeek)
                        .InclusiveBetween(0, 6).When(h => h != null)
                        .WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday).");
                });
        }
    }
}

