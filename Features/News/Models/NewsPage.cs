using DemoTraining.Business;
using DemoTraining.Features.Articles.Models;
using DemoTraining.Features.Standard.Models;
using DemoTraining.Models;
using DemoTraining.Models.Blocks;
using EPiServer.Filters;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.News.Models;

[SiteContentType(
    DisplayName = "NewsPage",
    GUID = "85D7D485-3016-46CF-95D2-52DFA8A0090C",
    Description = "", GroupName = Globals.GroupNames.Specialized)]
[SiteImageUrl]
public class NewsPage : StandardPage
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 305)]
    public virtual PageListBlock NewsList { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);

        NewsList.Count = 20;
        NewsList.Heading = ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/newspagetemplate/latestnews");
        NewsList.IncludeIntroduction = true;
        NewsList.IncludePublishDate = true;
        NewsList.Recursive = true;
        NewsList.PageTypeFilter = typeof(ArticlePage).GetPageType();
        NewsList.SortOrder = FilterSortOrder.PublishedDescending;
    }
}
