using DemoTraining.Extensions;
using DemoTraining.Models.Media;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.Security;
using Microsoft.Extensions.Options;

namespace DemoTraining.Business.ScheduledJobs;

[ScheduledPlugIn(
    DisplayName = "ImportImagesScheduledJob",
    Description = "",
    GUID = "E710ECEB-84EA-49EE-A787-10E4711180C0",
    IntervalType = ScheduledIntervalType.Hours,
    IntervalLength = 12,
    DefaultEnabled = true,
    // TODO CMS13: SortIndex property removed from ScheduledPlugIn attribute in CMS 13
    Restartable = true)]
public class ImportImagesScheduledJob : ScheduledJobBase
{
    public const string ScheduledJobName = "Import Images";

    private readonly string[] patterns = new[] { "*.png", "*.jpeg", "*.jpg", "*.webp" };

    private readonly IContentRepository contentRepository;
    private readonly IBlobFactory blobFactory;
    private readonly IOptions<MediaImportOptions> mediaImportOptions;

    private bool _stopSignaled;

    public ImportImagesScheduledJob()
    {
        IsStoppable = true;
    }

    public ImportImagesScheduledJob(
        IContentRepository contentRepository,
        IBlobFactory blobFactory,
        IOptions<MediaImportOptions> mediaImportOptions) : this()
    {
        this.contentRepository = contentRepository;
        this.blobFactory = blobFactory;
        this.mediaImportOptions = mediaImportOptions;
    }

    public override void Stop()
    {
        _stopSignaled = true;
    }

    private IEnumerable<string> GetImageFilenames(string path)
    {
        IEnumerable<string> files = null;
        foreach (string pattern in patterns)
        {
            IEnumerable<string> moreFiles = Directory.GetFiles(path, pattern);
            if (moreFiles.Any())
            {
                files = files == null ? moreFiles : files.Concat(moreFiles);
            }
        }
        return files;
    }

    public override string Execute()
    {
        // Read configuration from bound options
        string toImportFolder = mediaImportOptions?.Value?.ToImportFolder;
        string importedFolder = mediaImportOptions?.Value?.ImportedFolder;
        var assetsFolderValue = mediaImportOptions?.Value?.ImportAssetsFolder;
        var assetsFolder = new ContentReference(assetsFolderValue);

        IEnumerable<string> images = GetImageFilenames(toImportFolder);
        int toImportCount = 0;
        int importedCount = 0;
        int remainingCount = 0;
        if (images != null)
        {
            toImportCount = images.Count();
            remainingCount = toImportCount;
        }

        OnStatusChanged($"Starting {ScheduledJobName}. {toImportCount} images to import. Please wait...");

        while (remainingCount > 0)
        {
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            string nextImage = images.First();

            var asset = contentRepository.GetDefault<ImageFile>(parentLink: assetsFolder);
            asset.Name = Path.GetFileName(nextImage);
            asset.Copyright = $"Copyright © 2018 Episerver Education";

            Blob blob = blobFactory.CreateBlob(id: asset.BinaryDataContainer,
                extension: Path.GetExtension(nextImage));
            blob.WriteAllBytes(File.ReadAllBytes(nextImage));
            asset.BinaryData = blob;

            contentRepository.Save(asset, SaveAction.Publish, AccessLevel.NoAccess);

            File.Move(nextImage, Path.Combine(
                importedFolder, Path.GetFileName(nextImage)));

            Thread.Sleep(2500); // slow it down
            importedCount++;

            OnStatusChanged($"Imported {importedCount} of {toImportCount} images. Please wait...");

            images = GetImageFilenames(toImportFolder);
            if (images != null)
            {
                remainingCount = images.Count();
            }
            else { remainingCount = 0; }

        }

        return $"Successfully imported {importedCount} images.";
    }
}