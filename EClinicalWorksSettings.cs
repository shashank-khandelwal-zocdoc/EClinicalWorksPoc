namespace EClinicalWorksPoc;

public class EClinicalWorksSettings
{
    public const string SectionName = "EClinicalWorks";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}