namespace Services;

public record TokenSettings
{
    public string? Key { get; set; }
    public int ExpirationHours { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}
