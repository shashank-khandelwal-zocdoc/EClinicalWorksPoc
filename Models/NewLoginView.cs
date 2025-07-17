using EClinicalWorksPoc.Parsing;

namespace EClinicalWorksPoc.Models;

public class NewLoginView
{
    [XPathSelector("//meta[@name='_csrf']/@content")]
    public string CsrfToken { get; set; } = string.Empty;

    [XPathSelector("//script[contains(text(), 'var rsaPubKey')]")]
    public string RawTextWithRsaPubKey { get; set; } = string.Empty;

    [XPathSelector("//div[@class='logdiv clearfix']/div[@class='font-bold']")]
    public string Version { get; set; } = string.Empty;
} 