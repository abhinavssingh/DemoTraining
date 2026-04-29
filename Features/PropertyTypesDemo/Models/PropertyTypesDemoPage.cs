using DemoTraining.Features.CmsFieldTypes.Business;
using DemoTraining.Models.Pages;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.CmsFieldTypes.Models;

[ContentType(
    DisplayName = "PropertyTypesDemoPage",
    GUID = "4E0BF54A-8953-4303-A949-AA38F052ADDA",
    Description = "Use this page to demonstrate various types used for properties.", GroupName = "Training Field Types")]
public class PropertyTypesDemoPage : SitePageData
{
    [Display(Name = "Rich text", Order = 10, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual XhtmlString RichText { get; set; }

    [CultureSpecific]
    [Display(Name = "Localizable rich text", Order = 20, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual XhtmlString LocalizableRichText { get; set; }

    [Display(Name = "Single line text", Order = 30, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual string SingleLineText { get; set; }

    [StringLength(15, MinimumLength = 5, ErrorMessage = " {0} Must be between 5 and 15 characters.")]
    [Display(Name = "Single line text (5-15 chars)", Order = 40, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual string SingleLineText5to15chars { get; set; }

    [UIHint(UIHint.Textarea)]
    [Display(Name = "Multi-line text", Order = 50, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual string MultilineText { get; set; }

    // TODO CMS13: EPiServer.Framework.Validator is obsolete. Using standard .NET email regex.
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "{0} Must be a valid email address.")]
    [Display(Name = "Email address", Order = 60, GroupName = PropertyTypesDemoPageTabs.Text)]
    public virtual string EmailAddress { get; set; }

    // Numbers

    [Display(Name = "Integer", Order = 10, GroupName = PropertyTypesDemoPageTabs.Numbers)]
    public virtual int Integer { get; set; }

    [Range(18, 65, ErrorMessage = " {0} Must be between 18 and 65.")]
    [Display(Name = "Integer (18-65)", Order = 20, GroupName = PropertyTypesDemoPageTabs.Numbers)]
    public virtual int Integer18to65 { get; set; }

    [Display(Name = "Float", Order = 30, GroupName = PropertyTypesDemoPageTabs.Numbers)]
    [CultureSpecific]
    public virtual double Float { get; set; }

    [Display(Name = "Day of week", Order = 40, GroupName = PropertyTypesDemoPageTabs.Numbers)]
    [Range(0, 6, ErrorMessage = "{0} Must be between 0 (Sunday) and 6 (Saturday).")]
    public virtual DayOfWeek DayOfWeek { get; set; }

    [SelectOne(SelectionFactoryType = typeof(DayOfWeekSelectionFactory))]
    [Display(Name = "Day of week (drop-down)", Order = 50, GroupName = PropertyTypesDemoPageTabs.Numbers)]
    public virtual DayOfWeek DayOfWeekDropDown { get; set; }

    // References

    [Display(Name = "Reference a single content item", Order = 10, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentReference ReferenceContentItem { get; set; }

    [Display(Name = "Reference a single page", Order = 20, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentReference ReferencePage { get; set; }

    [AllowedTypes(typeof(BlockData))]
    [Display(Name = "Reference a single block", Order = 30, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentReference ReferenceBlock { get; set; }

    [UIHint(UIHint.Image)]
    [Display(Name = "Reference a single image", Order = 40, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentReference ReferenceImage { get; set; }

    [Display(Name = "Reference multiple content items", Order = 50, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentArea ReferenceContentItems { get; set; }

    [AllowedTypes(typeof(PageData))]
    [Display(Name = "Reference multiple pages (with a partial template)", Order = 60, GroupName = PropertyTypesDemoPageTabs.References)]
    public virtual ContentArea ReferencePages { get; set; }

    // Misc

    [Display(Name = "Single image URL", Order = 10, GroupName = PropertyTypesDemoPageTabs.Miscellaneous)]
    [CultureSpecific]
    [UIHint(UIHint.Image)]
    public virtual Url SingleImageUrl { get; set; }

    [CultureSpecific]
    [UIHint(UIHint.Video)]
    [Display(Name = "Multiple video URLs", Order = 20, GroupName = PropertyTypesDemoPageTabs.Miscellaneous)]
    public virtual LinkItemCollection MultipleVideoUrls { get; set; }

    [Display(Name = "Active check box", Order = 30, GroupName = PropertyTypesDemoPageTabs.Miscellaneous)]
    [CultureSpecific]
    public virtual bool Active { get; set; }
}
