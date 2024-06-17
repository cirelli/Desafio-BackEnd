using System.Text.RegularExpressions;

namespace Services.Validators;

public partial class MotorbikePacthValidator : AbstractValidator<MotorbikePatchDTO>
{
    [GeneratedRegex("[a-z]{3}[0-9]{1}[a-z]{1}[0-9]{2}", RegexOptions.IgnoreCase)]
    private static partial Regex MercosulPlateFormatRegex();

    [GeneratedRegex("[a-z]{3}[0-9]{4}", RegexOptions.IgnoreCase)]
    private static partial Regex OldPlateFormatRegex();

    public MotorbikePacthValidator()
    {
        RuleFor(m => m.Plate)
            .NotEmpty()
            .Length(7)
            .Must(BeValidFormat)
            .WithMessage("invalid format!");
    }

    private static bool BeValidFormat(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate)) return false;

        return IsMercosulPlateFormat(plate) || IsOldPlateFormat(plate);
    }

    private static bool IsMercosulPlateFormat(string plate)
        => MercosulPlateFormatRegex().IsMatch(plate);

    private static bool IsOldPlateFormat(string plate)
        => OldPlateFormatRegex().IsMatch(plate);
}
