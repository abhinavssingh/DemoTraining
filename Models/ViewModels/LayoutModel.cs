using DemoTraining.Features.Components.SitelogoType.Models;
using EPiServer.SpecializedProperties;
using Microsoft.AspNetCore.Html;

namespace DemoTraining.Models.ViewModels
{
    public class LayoutModel
    {
        public SiteLogotypeBlock Logotype { get; set; }

        public IHtmlContent LogotypeLinkUrl { get; set; }

        public bool HideHeader { get; set; }

        public bool HideFooter { get; set; }

        public LinkItemCollection ProductPages { get; set; }

        public LinkItemCollection ResourcePages { get; set; }

        public LinkItemCollection CompanyPages { get; set; }

        public LinkItemCollection SupportPages { get; set; }

        public bool LoggedIn { get; set; }

        public HtmlString LoginUrl { get; set; }

        public HtmlString LogOutUrl { get; set; }

        public HtmlString SearchActionUrl { get; set; }

        public bool IsInReadonlyMode { get; set; }
    }

}
