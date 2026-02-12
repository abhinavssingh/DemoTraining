using DemoTraining.Features.Standard.Models;
using DemoTraining.Models;

namespace DemoTraining.Features.Articles.Models;

/// <summary>
/// Used primarily for publishing news articles on the website
/// </summary>
[SiteContentType(
    DisplayName = "ArticlePage",
    GUID = "04BD2F40-8490-410D-B338-456B84389B68",
    Description = "")]
[SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
public class ArticlePage : StandardPage
{
    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);

        VisibleInMenu = false;
    }
}
