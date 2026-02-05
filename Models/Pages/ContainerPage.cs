using DemoTraining.Business.Rendering;

namespace DemoTraining.Models.Pages;

[ContentType(
    DisplayName = "ContainerPage",
    GUID = "E69A08A1-2BF6-437E-B0E4-897BA6D6A410",
    Description = "", GroupName = Globals.GroupNames.Specialized)]
[SiteImageUrl]
public class ContainerPage : SitePageData, IContainerPage
{
}
