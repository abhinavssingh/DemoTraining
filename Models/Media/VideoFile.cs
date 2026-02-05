using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Models.Media
{
    [ContentType(GroupName = "Training Media Content Type")]
    [MediaDescriptor(ExtensionString = "mp4, avi, mov, wmv, mkv,flv,webm")]
    public class VideoFile : VideoData
    {
        // <summary>
        /// Gets or sets the copyright.
        /// </summary>
        public virtual string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the URL to the preview image.
        /// </summary>
        [UIHint(UIHint.Image)]
        public virtual ContentReference PreviewImage { get; set; }
    }
}
