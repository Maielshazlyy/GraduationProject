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

