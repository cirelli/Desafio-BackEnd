namespace Services.Validators;

public class MotorbikeValidator : AbstractValidator<MotorbikeDTO>
{
    public MotorbikeValidator()
    {
        Include(new MotorbikePacthValidator());

        RuleFor(m => m.Year)
            .NotEmpty()
            .Must(m => m.ToString().Length == 4)
            .WithMessage("use the YYYY format");

        RuleFor(m => m.Model)
            .NotEmpty();
    }
}
