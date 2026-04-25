namespace DemoTraining.Extensions
{
    public class EpiserverOptions
    {
        public CmsOptions Cms { get; set; }

        public MediaImportOptions MediaImport { get; set; }

        public string CustomBlockFolder { get; set; }

        public FindOptions Find { get; set; }

        /// <summary>
        /// STAGE 2 CMS13: Optimizely Graph search configuration
        /// </summary>
        public GraphOptions Graph { get; set; }
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
        /// <summary>
        /// DEPRECATED CMS13: EPiServer.Find is deprecated in CMS 13
        /// These properties are maintained for backward compatibility only
        /// New search functionality uses Optimizely Graph (see GraphOptions)
        /// </summary>
        public string DefaultIndex { get; set; }
        public string ServiceUrl { get; set; }
    }

    /// <summary>
    /// STAGE 2 CMS13: Optimizely Graph SDK configuration
    /// Configure in appsettings.json under Optimizely:Graph
    /// </summary>
    public class GraphOptions
    {
        /// <summary>
        /// Enable/disable Graph search functionality
        /// Set to false to disable search without removing configuration
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Graph API gateway URL
        /// Example: https://graph-gateway.optimizely.com/graphql
        /// </summary>
        public string Gateway { get; set; }

        /// <summary>
        /// Graph App Key for authentication
        /// Should be retrieved from environment variables in production
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// Graph App Secret for authentication
        /// IMPORTANT: Never commit to source control - use Key Vault or env vars
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// HTTP request timeout in milliseconds
        /// Default: 30 seconds (30000 ms)
        /// </summary>
        public int Timeout { get; set; } = 30000;

        /// <summary>
        /// Retry policy for failed requests
        /// Options: "none", "linear", "exponential"
        /// Default: exponential backoff
        /// </summary>
        public string RetryPolicy { get; set; } = "exponential";

        /// <summary>
        /// Maximum retry attempts
        /// Default: 3
        /// </summary>
        public int MaxRetries { get; set; } = 3;
    }
}

