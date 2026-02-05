using DemoTraining.Models;
using DemoTraining.Models.Blocks;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Components.SitelogoType.Models;

[SiteContentType(
    DisplayName = "SiteLogotypeBlock",
    GUID = "BDAE44AE-F58E-4B06-AD35-268B02CA44A0",
    Description = "")]
[SiteImageUrl]
public class SiteLogotypeBlock : SiteBlockData
{
    /// <summary>
    /// Gets the site logotype URL
    /// </summary>
    /// <remarks>If not specified a default logotype will be used</remarks>
    [DefaultDragAndDropTarget]
    [UIHint(UIHint.Image)]
    public virtual Url Url
    {
        get
        {
            var url = this.GetPropertyValue(b => b.Url);

            return url == null || url.IsEmpty()
                ? new Url("/gfx/logotype.png")
                : url;
        }
        set => this.SetPropertyValue(b => b.Url, value);
    }

    [CultureSpecific]
    public virtual string Title { get; set; }
}
