namespace ProfilesApi.Infrastructure.Data;

public class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";
    public string ProfilesDb { get; set; } = string.Empty;
}
