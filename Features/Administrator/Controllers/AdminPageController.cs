using DemoTraining.Controllers;
using DemoTraining.Features.Administrator.Models;
using DemoTraining.Features.Components.Teaser.Models;
using DemoTraining.Features.Standard.Models;
using DemoTraining.Models.ViewModels;
using EPiServer.DataAbstraction.Activities;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Administrator.Controllers;

[TemplateDescriptor(Inherited = true)]
public class AdminPageController : PageControllerBase<AdminContentPage>
{
    private readonly IContentRepository repo = null;
    private readonly IConfiguration configuration;
    private readonly IContentLoader contentLoader;
    private readonly ISiteDefinitionRepository siteDefinitionRepository;
    private readonly ILanguageBranchRepository languageBranchRepository;
    private readonly IActivityQueryService activityQueryService;

    public AdminPageController(IContentRepository repo, IConfiguration configuration, IContentLoader contentLoader, ISiteDefinitionRepository siteDefinitionRepository,
        ILanguageBranchRepository languageBranchRepository, IActivityQueryService activityQueryService)
    {
        this.repo = repo;
        this.configuration = configuration;
        this.contentLoader = contentLoader;
        this.siteDefinitionRepository = siteDefinitionRepository;
        this.languageBranchRepository = languageBranchRepository;
        this.activityQueryService = activityQueryService;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateBlock(AdminContentPage currentPage, string heading, string text, ContentReference image, PageReference link)
    {
        try
        {
            // there are 3 types of folders in Episerver: Global, Site and Page. For blocks, we usually want to use Site or Global folders.
            // if you want to create under Global folder, you can directly use ContentReference.GlobalBlockFolder as parent when getting default content.
            // else if you want to create under Site folder, you can get all folders under ContentReference.SiteBlockFolder and find the one you want to use as parent.
            // In this example, we will look for a folder with a specific name defined in configuration, if not found, we will fallback to GlobalBlockFolder.
            var allFolders = GetAllBlockFolders(ContentReference.SiteBlockFolder);
            var customFolderName = configuration?.GetValue<string>("episerver:CustomBlockFolder");
            var targetFolder = allFolders
                .FirstOrDefault(f => string.Equals(f.Name, customFolderName, StringComparison.OrdinalIgnoreCase)) ?? repo.Get<ContentFolder>(ContentReference.GlobalBlockFolder);

            var block = repo.GetDefault<TeaserBlock>(targetFolder.ContentLink);
            var newBlock = block as IContent;
            newBlock.Name = heading;
            block.Heading = heading;
            block.Text = text;
            block.Image = image ?? ContentReference.EmptyReference;
            block.Link = link ?? PageReference.EmptyReference;

            repo.Save(newBlock, SaveAction.Publish, AccessLevel.NoAccess);
            TempData["message"] = $"Block '{newBlock.Name}' was created.";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error creating block: {ex.Message}";
        }

        var redirectUrl = UrlResolver.Current.GetUrl(currentPage.ContentLink);
        return Redirect(redirectUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateBlock(AdminContentPage currentPage, ContentReference contentReference, string heading, string text, ContentReference image, PageReference link)
    {
        try
        {
            if (contentReference == null || ContentReference.IsNullOrEmpty(contentReference))
            {
                TempData["message"] = "No content reference provided for update.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var block = repo.Get<BlockData>(contentReference) as TeaserBlock;
            block = block?.CreateWritableClone() as TeaserBlock;
            var blockContent = block as IContent;

            if (block != null)
            {
                blockContent.Name = heading ?? blockContent.Name;
                block.Heading = heading ?? block.Heading;
                block.Text = text ?? block.Text;
                if (image != null && !ContentReference.IsNullOrEmpty(image)) block.Image = image;
                if (link != null && !PageReference.IsNullOrEmpty(link)) block.Link = link;

                repo.Save(blockContent, SaveAction.Publish, AccessLevel.NoAccess);
                TempData["message"] = "Block was updated.";
            }
            else
            {
                TempData["message"] = "Block not found or wrong type.";
            }
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error updating block: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    public ActionResult Index(AdminContentPage currentPage)
    {
        var viewmodel = PageViewModel.Create(currentPage);
        return View("~/Features/Administrator/Views/Index.cshtml", viewmodel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AdminContentPage currentPage, string name)
    {
        try
        {
            var newPage = repo.GetDefault<StandardPage>(currentPage.ContentLink);
            newPage.Name = name;
            repo.Save(newPage, SaveAction.Publish, AccessLevel.NoAccess);
            TempData["message"] = $"'{newPage.Name}' was created.";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error creating page: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Update(AdminContentPage currentPage, ContentReference contentReference, string newName)
    {
        try
        {
            if (contentReference == null || ContentReference.IsNullOrEmpty(contentReference))
            {
                TempData["message"] = "No content reference provided for update.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var content = repo.Get<StandardPage>(contentReference).CreateWritableClone();
            if (content != null)
            {
                content.Name = newName;
                repo.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
                TempData["message"] = $"'{content.Name}' was saved.";
            }
            else
            {
                TempData["message"] = "Content not found.";
            }
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error updating page: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(AdminContentPage currentPage,
        ContentReference contentReference, string hardDelete)
    {
        try
        {
            var content = repo.Get<IContent>(contentReference);
            var name = content?.Name ?? string.Empty;

            if (hardDelete == "on")
            {
                repo.Delete(contentReference, forceDelete: true,
                    access: AccessLevel.NoAccess);

                TempData["message"] = $"'{name}' was deleted permanently.";
            }
            else
            {
                repo.Move(contentReference, destination: ContentReference.WasteBasket,
                    requiredSourceAccess: AccessLevel.NoAccess,
                    requiredDestinationAccess: AccessLevel.NoAccess);

                TempData["message"] = $"'{name}' was moved to trash.";
            }

            var redirectUrl = UrlResolver.Current.GetUrl(currentPage.ContentLink);
            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error deleting content: {ex.Message}";
            return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateSiteDefinition(AdminContentPage currentPage, string siteName, string hostName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(siteName) || string.IsNullOrWhiteSpace(hostName))
            {
                TempData["message"] = "Site name and host name are required.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            // Check for existing site definitions by name or host
            var existing = siteDefinitionRepository.List()
                .FirstOrDefault(sd => string.Equals(sd.Name, siteName, StringComparison.OrdinalIgnoreCase)
                    || sd.Hosts.Any(h => string.Equals(h.Name, hostName, StringComparison.OrdinalIgnoreCase)));

            if (existing != null)
            {
                TempData["message"] = "A site definition with that name or host already exists.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var siteDef = new SiteDefinition
            {
                Name = siteName,
                SiteUrl = new Uri($"http://{hostName}"),
                StartPage = currentPage.ContentLink
            };

            // Add host definition
            var hostDef = new HostDefinition { Name = hostName };
            siteDef.Hosts.Add(hostDef);

            siteDefinitionRepository.Save(siteDef);

            TempData["message"] = $"Site definition '{siteName}' created for host '{hostName}'.";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error creating site definition: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteSiteDefinition(AdminContentPage currentPage, string siteName, string hostName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(siteName) && string.IsNullOrWhiteSpace(hostName))
            {
                TempData["message"] = "Provide site name or host name to delete a site definition.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var existing = siteDefinitionRepository.List()
                .FirstOrDefault(sd => (!string.IsNullOrWhiteSpace(siteName) && string.Equals(sd.Name, siteName, StringComparison.OrdinalIgnoreCase))
                    || (sd.Hosts != null && !string.IsNullOrWhiteSpace(hostName) && sd.Hosts.Any(h => string.Equals(h.Name, hostName, StringComparison.OrdinalIgnoreCase))));

            if (existing == null)
            {
                TempData["message"] = "No matching site definition found to delete.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            siteDefinitionRepository.Delete(existing.Id);
            TempData["message"] = $"Site definition '{existing.Name}' was deleted.";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error deleting site definition: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EnableLanguage(AdminContentPage currentPage, string languageCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                TempData["message"] = "Language code is required.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }
            // Check for existing language
            var langExist = IsLanguageEnabled(languageCode);

            if (langExist == null)
            {
                TempData["message"] = $"Language '{languageCode}' doesn't exist.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var lang = langExist.CreateWritableClone();
            var status = lang.Enabled;
            if (!status)
            {
                lang.Enabled = true;
            }
            else
            {
                TempData["message"] = $"Language '{languageCode}' is already enabled.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            languageBranchRepository.Save(lang);

            TempData["message"] = $"Language '{languageCode}' enabled.";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error enabling language: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult TranslatePage(AdminContentPage currentPage, ContentReference contentReference, string targetLanguage)
    {
        try
        {
            if (contentReference == null || ContentReference.IsNullOrEmpty(contentReference) || string.IsNullOrWhiteSpace(targetLanguage))
            {
                TempData["message"] = "Content reference and target language are required for translation.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            // Check for existing language
            var langExist = IsLanguageEnabled(targetLanguage);

            if (langExist == null)
            {
                TempData["message"] = $"Language '{targetLanguage}' doesn't exist.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            var status = langExist.Enabled;
            if (status)
            {
                var content = repo.Get<IContent>(contentReference);
                if (content == null)
                {
                    TempData["message"] = "Content not found for translation.";
                    return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
                }
                var translated = repo.CreateLanguageBranch<PageData>(contentReference, new System.Globalization.CultureInfo(targetLanguage));
                if (translated != null)
                {
                    translated.Name = $"{content.Name} - {targetLanguage}";

                    repo.Save(translated, SaveAction.Publish, AccessLevel.NoAccess);
                    TempData["message"] = $"Content '{content.Name}' was translated to '{targetLanguage}'.";
                }
                else
                {
                    TempData["message"] = $"Failed to create translation for content '{content.Name}' to '{targetLanguage}'.";
                }


            }
            else
            {
                TempData["message"] = $"Language '{targetLanguage}' is not enabled.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }


        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error translating content: {ex.Message}";
        }
        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ActivityCounts(AdminContentPage currentPage, DateTime? startDate, DateTime? endDate, string activityType)
    {
        try
        {
            if (activityQueryService == null)
            {
                TempData["message"] = "Activity query service is not available.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            if (string.IsNullOrWhiteSpace(activityType))
            {
                TempData["message"] = "No activity type selected.";
                return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
            }

            // Default to last 15 days if not provided
            var start = startDate ?? DateTime.UtcNow.AddDays(-15);
            var end = endDate ?? DateTime.UtcNow;
            ActivityQuery query;

            if (string.Equals(activityType, "ItemAdded", StringComparison.OrdinalIgnoreCase))
            {
                query = new ActivityQuery
                {
                    ActivityType = ActivityType.Content,
                    Action = (int)ContentActionType.Create,
                    CreatedAfter = start,
                    CreatedBefore = end,
                    IncludeArchived = false,
                    MaxResults = 1000
                };

                var results = await activityQueryService.ListActivitiesAsync(query);

                if (results == null || !results.Any())
                {
                    TempData["message"] = "No activities found for the selected activity type or error retrieving activities.";
                }
                else
                {
                    var count = results.Count();
                    TempData["message"] = $"Found {count} activities of type '{activityType}' between {start:u} and {end:u}.";
                }
            }
            else if (string.Equals(activityType, "Publish", StringComparison.OrdinalIgnoreCase))
            {
                query = new ActivityQuery
                {
                    ActivityType = ActivityType.Content,
                    Action = (int)ContentActionType.Publish,
                    CreatedAfter = start,
                    CreatedBefore = end,
                    IncludeArchived = false,
                    MaxResults = 1000
                };

                var results = await activityQueryService.ListActivitiesAsync(query);

                if (results == null || !results.Any())
                {
                    TempData["message"] = "No activities found for the selected activity type or error retrieving activities.";
                }
                else
                {
                    var count = results.Count();
                    TempData["message"] = $"Found {count} activities of type '{activityType}' between {start:u} and {end:u}.";
                }
            }
            else if (string.Equals(activityType, "Deleted", StringComparison.OrdinalIgnoreCase))
            {
                query = new ActivityQuery
                {
                    ActivityType = ActivityType.Content,
                    Action = (int)ContentActionType.Delete,
                    CreatedAfter = start,
                    CreatedBefore = end,
                    IncludeArchived = false,
                    MaxResults = 1000
                };

                var results = await activityQueryService.ListActivitiesAsync(query);

                if (results == null || !results.Any())
                {
                    TempData["message"] = "No activities found for the selected activity type or error retrieving activities.";
                }
                else
                {
                    var count = results.Count();
                    TempData["message"] = $"Found {count} activities of type '{activityType}' between {start:u} and {end:u}.";
                }
            }

        }
        catch (Exception ex)
        {
            TempData["message"] = $"Error querying activities: {ex.Message}";
        }

        return Redirect(UrlResolver.Current.GetUrl(currentPage.ContentLink));
    }

    private LanguageBranch IsLanguageEnabled(string languageCode)
    {
        var lang = languageBranchRepository.Load(languageCode);
        if (lang != null)
        {
            return lang;
        }
        return null;
    }

    private IEnumerable<ContentFolder> GetAllBlockFolders(ContentReference root)
    {
        var allDescendants = contentLoader.GetDescendents(root)
                                   .Select(contentRef => contentLoader.Get<IContent>(contentRef))
                                   .OfType<ContentFolder>();
        return allDescendants;
    }

}
