namespace Services.Validators;

public class OrderValidator : AbstractValidator<OrderDTO>
{
    public OrderValidator()
    {
        RuleFor(m => m.Value)
            .NotEmpty();
    }
}
