namespace Services.Validators;

public class RentPlanValidator : AbstractValidator<RentPlanDTO>
{
    public RentPlanValidator()
    {
        RuleFor(m => m.Days)
            .NotEmpty();

        RuleFor(m => m.Price)
            .NotEmpty();

        RuleFor(m => m.Fee)
            .NotEmpty();

        RuleFor(m => m.AdditionalDailyPrice)
            .NotNull();
    }
}
