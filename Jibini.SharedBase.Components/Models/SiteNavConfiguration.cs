namespace Jibini.SharedBase.Data.Models;

public class SiteNavBranding
{
    public string BrandName { get; set; } = "";
    public string BrandImage { get; set; } = "";
}

public class SiteNavPage
{
    public string NavIcon { get; set; } = "";
    public string NavTitle { get; set; } = "";
    public string NavTooltip { get; set; } = "";
    public string NavPath { get; set; } = "";
}

public class SiteNavConfiguration
{
    public SiteNavBranding Branding = new();
    public List<SiteNavPage> Pages = new();
}