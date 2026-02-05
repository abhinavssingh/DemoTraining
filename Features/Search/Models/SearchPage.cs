using DemoTraining.Features.Components.Jumbotron.Models;
using DemoTraining.Models;
using DemoTraining.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Search.Models;

[SiteContentType(
    DisplayName = "SearchPage",
    GUID = "86F764BA-9E5B-4814-ACA2-46DAEB0BEC9B",
    Description = "", GroupName = Globals.GroupNames.Specialized)]
[SiteImageUrl]
public class SearchPage : SitePageData, IHasRelatedContent, ISearchPage
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 310)]
    [CultureSpecific]
    [AllowedTypes([typeof(IContentData)], [typeof(JumbotronBlock)])]
    public virtual ContentArea RelatedContentArea { get; set; }
}
