namespace Services.Validators;

public class RentValidator : AbstractValidator<RentDTO>
{
    public RentValidator()
    {
        RuleFor(m => m.RentPlanId)
            .NotEmpty();
    }
}
