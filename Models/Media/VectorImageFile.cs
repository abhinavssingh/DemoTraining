using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;

namespace DemoTraining.Models.Media;

[ContentType(
    DisplayName = "VectorImageFile",
    GUID = "62650F05-FBE0-49D2-9D0E-0B1BDD53A93B",
    Description = "")]
[MediaDescriptor(ExtensionString = "svg")]
public class VectorImageFile : ImageData
{
    /// <summary>
    /// Gets the generated thumbnail for this media.
    /// </summary>
    public override Blob Thumbnail => BinaryData;
}

