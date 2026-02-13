using DemoTraining.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.CmsProperties.Models;

[ContentType(
    DisplayName = "CmsPropertiesPage",
    GUID = "5A34952C-D1B6-4E90-94D9-2792C2F07CF0",
    Description = "this is used to display CMS properties",
    GroupName = "Training",
    Order = 20)]
public class CmsPropertiesPage : SitePageData
{
    [CultureSpecific]
    [Display(
        Name = "Cms Property Name",
        Description = "My property description",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string CmsPropertyName { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Cms Property Value",
        Description = "My property description",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual string CmsPropertyValue { get; set; }
}
