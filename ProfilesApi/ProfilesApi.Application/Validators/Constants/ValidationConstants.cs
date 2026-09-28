namespace ProfilesApi.Application.Validators.Constants;

public static class ValidationConstants
{
    public const string NationalInsuranceNumberRegex = @"^(?i)(?!(?:BG|GB|KN|NK|NT|TN|ZZ))[A-CEGHJ-PR-TW-Z][A-NP-Z](?:\s*\d{2}){3}\s*[A-D]$";
    public const string PhoneNumberRegex = @"^\+?[1-9]\d{1,14}$";
}
