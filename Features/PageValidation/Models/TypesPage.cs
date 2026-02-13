using DemoTraining.Features.PageValidation.Business;
using DemoTraining.Models.Pages;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.PageValidation.Models;

[ContentType(
    DisplayName = "TypesPage",
    GUID = "68401453-E8B9-49ED-A4D9-205B0D1EEB35",
    Description = "Use this to explore property types.")]
public class TypesPage : SitePageData
{
    #region Supported .NET types

    public virtual string ShortText { get; set; }

    [UIHint(UIHint.Textarea)]
    public virtual string LongText { get; set; }

    [StringLength(65, MinimumLength = 5)]
    public override string MetaTitle { get; set; }

    [RegularExpression(@"[a-z0-9\.]+@[a-z0-9\.]+", ErrorMessage = "Enter a valid email address.")]
    public virtual string ContactAddress { get; set; }

    [SelectOne(SelectionFactoryType = typeof(WorkStatusSelectionFactory))]
    public virtual string WorkStatus { get; set; }

    [SelectMany(SelectionFactoryType = typeof(WorkStatusSelectionFactory))]
    public virtual string WorkStatuses { get; set; }

    [Display(GroupName = ".NET Types")]
    public virtual bool OnOrOff { get; set; }

    [Display(GroupName = ".NET Types")]
    public virtual int WholeNumber { get; set; }

    [Display(GroupName = ".NET Types")]
    public virtual double RealNumber { get; set; }

    [Display(GroupName = ".NET Types")]
    public virtual DateTime When { get; set; }

    #endregion

    #region Episerver types

    [Display(GroupName = "Episerver Types")]
    public virtual XhtmlString RichText { get; set; }

    [Display(GroupName = "Episerver Types")]
    public virtual Url LinkToAnything { get; set; }

    [Display(GroupName = "Episerver Types")]
    [UIHint(UIHint.Image)]
    public virtual Url LinkToImage { get; set; }

    [Display(GroupName = "Episerver Types")]
    public virtual LinkItemCollection LinksToAnything { get; set; }


    [Display(GroupName = "Episerver Types")]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<Person>))]
    public virtual IList<Person> People { get; set; }

    #endregion

    #region Content reference types

    [Display(GroupName = "Content Reference Types")]
    public virtual ContentReference SingleContentItem { get; set; }

    [Display(GroupName = "Content Reference Types")]
    [UIHint(UIHint.Image)]
    public virtual ContentReference SingleImage { get; set; }

    [Display(GroupName = "Content Reference Types")]
    [UIHint(UIHint.MediaFile)]
    public virtual ContentReference SingleMediaFile { get; set; }

    [Display(GroupName = "Content Reference Types")]
    public virtual PageReference SinglePage { get; set; }

    [Display(GroupName = "Content Reference Types")]
    [AllowedTypes(typeof(HomePage.Models.HomePage), typeof(TypesPage))]
    public virtual PageReference SingleStartOrTypesPage { get; set; }

    [Display(GroupName = "Content Reference Types")]
    public virtual ContentArea MultipleContentItems { get; set; }

    [Display(GroupName = "Content Reference Types")]
    [AllowedTypes(typeof(PageData))]
    public virtual ContentArea MultiplePages { get; set; }

    [Display(GroupName = "Content Reference Types", Order = 1)]
    [AllowedTypes(typeof(PageData))]
    public virtual IList<ContentReference> MultiplePagesList { get; set; }

    #endregion
}
