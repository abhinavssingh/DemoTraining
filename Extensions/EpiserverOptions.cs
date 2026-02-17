namespace DemoTraining.Extensions
{
    public class EpiserverOptions
    {
        public CmsOptions Cms { get; set; }

        public MediaImportOptions MediaImport { get; set; }

        public string CustomBlockFolder { get; set; }

        public FindOptions Find { get; set; }
    }

    public class CmsOptions
    {
        public MappedRolesOptions MappedRoles { get; set; }
    }

    public class MappedRolesOptions
    {
        public Dictionary<string, MappedRoleItem> Items { get; set; }
    }

    public class MappedRoleItem
    {
        public string[] MappedRoles { get; set; }
    }

    public class MediaImportOptions
    {
        public string ToImportFolder { get; set; }
        public string ImportedFolder { get; set; }
        public string ImportAssetsFolder { get; set; }
    }

    public class FindOptions
    {
        public string DefaultIndex { get; set; }
        public string ServiceUrl { get; set; }
    }
}
