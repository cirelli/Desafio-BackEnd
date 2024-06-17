namespace Services.Validators;

public class DriverValidator : AbstractValidator<DriverDTO>
{
    public DriverValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(m => m.Password)
            .NotEmpty()
            .Length(6, 20);

        RuleFor(m => m.Cnpj)
            .NotEmpty()
            .Length(14);

        RuleFor(m => m.BirthDate)
            .NotEmpty();

        RuleFor(m => m.Cnh)
            .NotEmpty()
            .Length(11);

        RuleFor(m => m.CnhType)
            .NotEmpty()
            .IsInEnum();
    }
}
