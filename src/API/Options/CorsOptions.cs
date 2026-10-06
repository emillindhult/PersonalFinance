namespace API.Options;

internal sealed class CorsOptions
{
    public const string SectionName = "Cors";
    public const string DefaultPolicyName = "FinanceManager";

    public string PolicyName { get; init; } = DefaultPolicyName;

    public string[]? AllowedOrigins { get; init; }
}
