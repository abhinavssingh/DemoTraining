using DemoTraining.Models;
using DemoTraining.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Components.Button.Models;

/// <summary>
/// Used to insert a link which is styled as a button
/// </summary>
[SiteContentType(
    DisplayName = "ButtonBlock",
    GUID = "28EA9C16-6232-4E4B-AB3B-EAEC8FEAE44C",
    Description = "")]
[SiteImageUrl]
public class ButtonBlock : SiteBlockData
{
    [Display(Order = 1, GroupName = SystemTabNames.Content)]
    [Required]
    public virtual string ButtonText { get; set; }

    [Display(Order = 2, GroupName = SystemTabNames.Content)]
    [Required]
    public virtual Url ButtonLink { get; set; }
}

