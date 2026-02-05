using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.HomePage.Models;

[ContentType(
    DisplayName = "Home Page",
    GUID = "4B7ADF4C-160D-4D9E-9805-FC2C6247BCE0",
    Description = "Use this page for the home page of the website",
    GroupName = "Training", Order = 10)]
public class HomePage : PageData
{
    [CultureSpecific]
    [Display(
        Name = "Heading",
        Description = "Page heading",
        GroupName = SystemTabNames.Content,
        Order = 10,
        Prompt = "Enter the heading for the page")]
    public virtual string Heading { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Description",
        Description = "The Description will be shown in the content area of the page",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual XhtmlString Description { get; set; }

    [Display(Name = "Multiple Links",
        Description = "The link to another page or external URL", GroupName = SystemTabNames.Content,
        Order = 30, Prompt = "Enter a link to another page or external URL")]
    public virtual LinkItemCollection Links { get; set; }
}
