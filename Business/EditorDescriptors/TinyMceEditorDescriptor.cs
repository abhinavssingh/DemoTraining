using EPiServer.Cms.TinyMce.Core;
using EPiServer.Security;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace DemoTraining.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(XhtmlString),
       EditorDescriptorBehavior = EditorDescriptorBehavior.PlaceLast)]
    public class TinyMceEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (PrincipalInfo.CurrentPrincipal.IsInRole("HtmlSourceEditors"))
            {
                TinyMceSettings settings = metadata.EditorConfiguration["settings"] as TinyMceSettings;

                settings.AppendToolbar("code");
            }
        }
    }
}
