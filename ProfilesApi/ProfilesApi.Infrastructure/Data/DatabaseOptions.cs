namespace ProfilesApi.Infrastructure.Data;

public sealed class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";
    public string ProfilesDb { get; set; } = string.Empty;
}
