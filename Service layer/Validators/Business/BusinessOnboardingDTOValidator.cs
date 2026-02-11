using FluentValidation;
using Service_layer.DTOS.Business;

namespace Service_layer.Validators.Business
{
    public class BusinessOnboardingDTOValidator : AbstractValidator<BusinessOnboardingDTO>
    {
        public BusinessOnboardingDTOValidator()
        {
            // Business Information
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Business name is required.")
                .MaximumLength(200).WithMessage("Business name cannot exceed 200 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Business type is required.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

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

            // Working Hours Validation (Optional)
            RuleForEach(x => x.WorkingHours)
                .ChildRules(wh =>
                {
                    wh.RuleFor(h => h.DayOfWeek)
                        .InclusiveBetween(0, 6).WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday).");
                });

            // Agent Configuration
            RuleFor(x => x.AgentName)
                .NotEmpty().WithMessage("Agent name is required.")
                .MaximumLength(100).WithMessage("Agent name cannot exceed 100 characters.");

            RuleFor(x => x.AgentTone)
                .NotEmpty().WithMessage("Agent tone is required.")
                .MaximumLength(50).WithMessage("Agent tone cannot exceed 50 characters.");

            RuleFor(x => x.WelcomeMessage)
                .NotEmpty().WithMessage("Welcome message is required.")
                .MaximumLength(500).WithMessage("Welcome message cannot exceed 500 characters.");

            // Knowledge Base
            RuleFor(x => x.KnowledgeBaseItems)
                .NotNull().WithMessage("Knowledge base items cannot be null.")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("At least one knowledge base item is required.");

            RuleForEach(x => x.KnowledgeBaseItems)
                .SetValidator(new KnowledgeBaseItemDTOValidator());

            // Menu Categories Validation (Optional)
            RuleForEach(x => x.MenuCategories)
                .ChildRules(category =>
                {
                    category.RuleFor(c => c.Name)
                        .NotEmpty().When(c => !string.IsNullOrWhiteSpace(c.Name))
                        .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
                    
                    category.RuleFor(c => c.Description)
                        .MaximumLength(500).When(c => !string.IsNullOrWhiteSpace(c.Description))
                        .WithMessage("Category description cannot exceed 500 characters.");
                });

            // Menu Items Validation (Optional)
            RuleForEach(x => x.MenuItems)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.Name)
                        .NotEmpty().When(i => !string.IsNullOrWhiteSpace(i.Name))
                        .MaximumLength(200).WithMessage("Menu item name cannot exceed 200 characters.");
                    
                    item.RuleFor(i => i.Description)
                        .MaximumLength(1000).When(i => !string.IsNullOrWhiteSpace(i.Description))
                        .WithMessage("Menu item description cannot exceed 1000 characters.");
                    
                    item.RuleFor(i => i.Price)
                        .GreaterThan(0).When(i => i.Price > 0)
                        .WithMessage("Price must be greater than 0.");
                });

            // Subscription
            RuleFor(x => x.PlanName)
                .NotEmpty().WithMessage("Plan name is required.")
                .Must(plan => plan.ToLower() == "monthly" || plan.ToLower() == "yearly")
                .WithMessage("Plan name must be either 'Monthly' or 'Yearly'.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            // Payment Card Details
            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Card holder name is required.")
                .MaximumLength(100).WithMessage("Card holder name cannot exceed 100 characters.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Matches(@"^\d{13,19}$").WithMessage("Card number must be between 13 and 19 digits.");

            RuleFor(x => x.CardExpiryMonth)
                .InclusiveBetween(1, 12).WithMessage("Card expiry month must be between 1 and 12.");

            RuleFor(x => x.CardExpiryYear)
                .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Card expiry year must be current year or later.");

            RuleFor(x => x.CardCvv)
                .NotEmpty().WithMessage("CVV is required.")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits.");
        }
    }

    public class KnowledgeBaseItemDTOValidator : AbstractValidator<KnowledgeBaseItemDTO>
    {
        public KnowledgeBaseItemDTOValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required.")
                .MaximumLength(500).WithMessage("Question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("Answer is required.")
                .MaximumLength(2000).WithMessage("Answer cannot exceed 2000 characters.");
        }
    }
}

