using DemoTraining.Business.Rendering;
using DemoTraining.Models;
using DemoTraining.Models.Pages;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Contact.Models;

[SiteContentType(
    DisplayName = "ContactPage",
    GUID = "F725D0FA-50B3-4F0D-9643-827111E508EA",
    Description = "", GroupName = Globals.GroupNames.Specialized)]
[SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-contact.png")]
public class ContactPage : SitePageData, IContainerPage
{
    [Display(GroupName = Globals.GroupNames.Contact)]
    [UIHint(UIHint.Image)]
    public virtual ContentReference Image { get; set; }

    [Display(GroupName = Globals.GroupNames.Contact)]
    public virtual string Phone { get; set; }

    [Display(GroupName = Globals.GroupNames.Contact)]
    [EmailAddress]
    public virtual string Email { get; set; }
}

