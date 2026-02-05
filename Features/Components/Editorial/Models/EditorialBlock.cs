using DemoTraining.Models;
using DemoTraining.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Components.Editorial.Models;

[SiteContentType(
    DisplayName = "EditorialBlock",
    GUID = "7DD9F1DF-3C06-4F84-8097-BC4B9D7A6D56",
    Description = "", GroupName = SystemTabNames.Content)]
[SiteImageUrl]
public class EditorialBlock : SiteBlockData
{
    [Display(GroupName = SystemTabNames.Content)]
    [CultureSpecific]
    public virtual XhtmlString MainBody { get; set; }
}
