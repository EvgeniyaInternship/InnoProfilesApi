namespace ProfilesApi.Infrastructure.Data;

public static class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";
    public static string ProfilesDb { get; set; } = string.Empty;
}
