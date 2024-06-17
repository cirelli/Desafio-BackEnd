namespace Services.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(m => m.Username)
            .NotEmpty()
            .MaximumLength(14);

        RuleFor(m => m.Password)
            .NotEmpty()
            .Length(6, 20);
    }
}
