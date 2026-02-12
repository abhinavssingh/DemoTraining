using DemoTraining.Features.Standard.Models;
using DemoTraining.Models;
using Page = DemoTraining.Features.StartPage.Models;

namespace DemoTraining.Features.Administrator.Models;

[ContentType(
    DisplayName = "AdminContentPage",
    GUID = "F8FAFE8D-D8AE-42AA-8B87-682C4FF6E950",
    GroupName = Globals.GroupNames.Specialized,
    Description = "")]
[SiteImageUrl]
[AvailableContentTypes(IncludeOn = new[] { typeof(Page.StartPage) })]
public class AdminContentPage : StandardPage
{

}
