using EPiServer.Framework.DataAnnotations;

namespace DemoTraining.Models.Media
{
    [ContentType(GroupName = "Training Media Content Type")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,webp")]
    public class ImageFile : ImageData
    {
        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public virtual string Copyright { get; set; }
    }
}
