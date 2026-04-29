using DemoTraining.Features.StartPage.Models;
using DemoTraining.Models.ViewModels;
using EPiServer.Data;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace DemoTraining.Business
{
    [ServiceConfiguration]
    public class PageViewContextFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly IDatabaseMode _databaseMode;
        private readonly CookieAuthenticationOptions _cookieAuthenticationOptions;

        public PageViewContextFactory(
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            IDatabaseMode databaseMode,
            IOptionsMonitor<CookieAuthenticationOptions> optionMonitor)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _databaseMode = databaseMode;
            _cookieAuthenticationOptions = optionMonitor.Get(IdentityConstants.ApplicationScheme);
        }

        public virtual LayoutModel CreateLayoutModel(ContentReference currentContentLink, HttpContext httpContext)
        {
            // TODO CMS13: SiteDefinition.Current is deprecated. Use IApplicationResolver instead.
            // This is tracked in the CMS 13 deprecation analysis.
            var startPageContentLink = SiteDefinition.Current.StartPage;

            // Use the content link with version information when editing the startpage,
            // otherwise the published version will be used when rendering the props below.
            if (currentContentLink.CompareToIgnoreWorkID(startPageContentLink))
            {
                startPageContentLink = currentContentLink;
            }

            var startPage = _contentLoader.Get<StartPage>(startPageContentLink);

            return new LayoutModel
            {
                Logotype = startPage.SiteLogotype,
                LogotypeLinkUrl = new HtmlString(_urlResolver.GetUrl(SiteDefinition.Current.StartPage)),
                ProductPages = startPage.ProductPageLinks,
                ResourcePages = startPage.ResourcePageLinks,
                CompanyPages = startPage.CompanyPageLinks,
                SupportPages = startPage.SupportPageLinks,
                LoggedIn = httpContext.User.Identity.IsAuthenticated,
                LoginUrl = new HtmlString(GetLoginUrl(currentContentLink)),
                SearchActionUrl = new HtmlString(UrlResolver.Current.GetUrl(startPage.SearchPageLink)),
                IsInReadonlyMode = _databaseMode.DatabaseMode == DatabaseMode.ReadOnly
            };
        }

        private string GetLoginUrl(ContentReference returnToContentLink)
        {
            return $"{_cookieAuthenticationOptions?.LoginPath.Value ?? Globals.LoginPath}?ReturnUrl={_urlResolver.GetUrl(returnToContentLink)}";
        }

        public virtual IContent GetSection(ContentReference contentLink)
        {
            var currentContent = _contentLoader.Get<IContent>(contentLink);

            // TODO CMS13: SiteDefinition.Current and .RootPage are deprecated in CMS 13.
            // Should be replaced with IApplicationResolver and SystemDefinition.
            static bool isSectionRoot(ContentReference contentReference) =>
               ContentReference.IsNullOrEmpty(contentReference) ||
               contentReference.Equals(SiteDefinition.Current.StartPage) ||
               contentReference.Equals(SiteDefinition.Current.RootPage);

            if (isSectionRoot(currentContent.ParentLink))
            {
                return currentContent;
            }

            return _contentLoader.GetAncestors(contentLink)
                .OfType<PageData>()
                .SkipWhile(x => !isSectionRoot(x.ParentLink))
                .FirstOrDefault();
        }
    }

}
