using DemoTraining.Controllers;
using DemoTraining.Features.Administrator.Models;
using DemoTraining.Features.Components.Teaser.Models;
using DemoTraining.Features.Standard.Models;
using DemoTraining.Models.ViewModels;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Administrator.Controllers;

[TemplateDescriptor(Inherited = true)]
public class AdminPageController : PageControllerBase<AdminContentPage>
{
    private readonly IContentRepository repo = null;
    private readonly IConfiguration configuration;
    private readonly IContentLoader contentLoader;

    public AdminPageController(IContentRepository repo, IConfiguration configuration, IContentLoader contentLoader)
    {
        this.repo = repo;
        this.configuration = configuration;
        this.contentLoader = contentLoader;
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

    private IEnumerable<ContentFolder> GetAllBlockFolders(ContentReference root)
    {
        var allDescendants = contentLoader.GetDescendents(root)
                                   .Select(contentRef => contentLoader.Get<IContent>(contentRef))
                                   .OfType<ContentFolder>();
        return allDescendants;
    }

}
