# Optimizely Graph Setup Guide for CMS 13

**Date**: 2026-02-17  
**Purpose**: Document Graph SDK installation and configuration for search migration

## Phase 3: Graph SDK Package Installation

### Current Status
- **Find packages**: Removed from csproj
- **Graph packages**: Placeholders added (commented)
- **Build**: ✅ Succeeds without Graph packages

### Graph SDK Package Checklist

When Optimizely Graph SDK is released for CMS 13, complete the following:

#### 1. Identify Official Package Names
```powershell
# Search NuGet.org for:
dotnet package search Optimizely.Graph
dotnet package search Optimizely.Graph.Cms
```

**Expected packages** (subject to change):
- `Optimizely.Graph.Cms.Query` - Core Graph query API
- `Optimizely.Graph.AspNetCore` - ASP.NET Core integration
- Or unified: `Optimizely.Graph.CMS` or `Optimizely.ContentCloud.Search`

#### 2. Install via csproj Update
Uncomment and update in `DemoTraining.csproj`:
```xml
<PackageReference Include="Optimizely.Graph.Cms.Query" Version="[get-latest-version]" />
<PackageReference Include="Optimizely.Graph.AspNetCore" Version="[get-latest-version]" />
```

Or via CLI:
```powershell
dotnet add package Optimizely.Graph.Cms.Query
dotnet add package Optimizely.Graph.AspNetCore
```

#### 3. Restore and Verify Build
```powershell
dotnet restore
dotnet build
```

### Phase 5: Graph Configuration

#### 1. Update appsettings.json
Add Graph configuration section:

```json
{
  "Optimizely": {
	"Graph": {
	  "Enabled": true,
	  "Gateway": "https://your-graph-gateway.optimizely.com/graphql",
	  "AppKey": "your-app-key",
	  "Secret": "your-app-secret",
	  "Timeout": 30000,
	  "RetryPolicy": "exponential"
	}
  }
}
```

#### 2. Environment-Specific Overrides
Create `appsettings.Production.json`:
```json
{
  "Optimizely": {
	"Graph": {
	  "Gateway": "https://prod-gateway.optimizely.com/graphql",
	  "AppKey": "${OPTIMIZELY_GRAPH_APP_KEY}",
	  "Secret": "${OPTIMIZELY_GRAPH_SECRET}"
	}
  }
}
```

⚠️ **NEVER commit secrets** - use environment variables or Azure Key Vault

#### 3. Update EpiserverOptions.cs
Add Graph configuration class:
```csharp
public class GraphOptions
{
	public bool Enabled { get; set; } = true;
	public string Gateway { get; set; }
	public string AppKey { get; set; }
	public string Secret { get; set; }
	public int Timeout { get; set; } = 30000;
	public string RetryPolicy { get; set; } = "exponential";
}
```

Add to `EpiserverOptions`:
```csharp
public GraphOptions Graph { get; set; }
```

#### 4. Update GraphServiceCollectionExtensions.cs
Uncomment and implement:
```csharp
public static IServiceCollection AddOptimizelyGraph(
	this IServiceCollection services, 
	IConfiguration configuration)
{
	services.AddScoped<IGraphSearchService, GraphSearchService>();

	// Bind configuration
	services.Configure<GraphOptions>(configuration.GetSection("Optimizely:Graph"));

	// Register Graph HTTP client with credentials
	services.AddHttpClient<IGraphQueryClient>()
		.ConfigureHttpClient((sp, client) =>
		{
			var graphConfig = sp.GetRequiredService<IOptions<GraphOptions>>().Value;
			if (!graphConfig.Enabled)
				return;

			client.BaseAddress = new Uri(graphConfig.Gateway);
			client.DefaultRequestHeaders.Add("X-API-Key", graphConfig.AppKey);
			client.DefaultRequestHeaders.Add("X-API-Secret", graphConfig.Secret);
		});

	return services;
}
```

### Phase 4: Update GraphSearchService.cs

Once Graph SDK is installed, implement the TODO:

```csharp
public async Task<GraphSearchResult> SearchAsync(...)
{
	var graphClient = /* injected IGraphQueryClient or similar */;

	var graphQuery = graphClient
		.QueryContent<IContent>()
		.SearchFor(query)  // Full-text search
		.WithDisplayFilters()  // Respect publish state, access control
		.Filter(x => x.Status.Equals("Published"));

	// Apply filters
	if (filters?.Sections?.Count > 0)
		graphQuery = graphQuery.Filter(x => x.SearchSection.In(filters.Sections));

	if (filters?.ContentTypes?.Count > 0)
		graphQuery = graphQuery.Filter(x => x.SearchCategories.In(filters.ContentTypes));

	// Apply pagination
	var skip = (page - 1) * pageSize;
	graphQuery = graphQuery.Offset(skip).Limit(pageSize);

	// Execute async
	var results = await graphQuery.GetAsContentAsync(cancellationToken);

	// Map to GraphSearchResult
	return MapGraphResults(results, page, pageSize);
}
```

### Post-Installation Validation

- [ ] Restore completes without errors
- [ ] Build succeeds with Graph packages
- [ ] GraphSearchService compiles without errors
- [ ] Dependency injection resolves IGraphSearchService
- [ ] Search controller can be wired up to Graph service
- [ ] Graph credentials are configured in appsettings
- [ ] Manual integration test against Graph API

## Files to Update When Graph SDK is Available

1. **DemoTraining.csproj** - Uncomment Graph packages
2. **EpiserverOptions.cs** - Add GraphOptions class
3. **appsettings.json** - Add Optimizely:Graph section
4. **GraphSearchService.cs** - Implement Graph SDK calls
5. **GraphServiceCollectionExtensions.cs** - Implement Graph HTTP client registration
6. **SearchPageController.cs** - Wire up IGraphSearchService (Step 4)

## References

- [Optimizely Graph Documentation](https://docs.optimizely.com)
- [Graph C# SDK](https://docs.optimizely.com) (when available)
- [ASP.NET Core HttpClientFactory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)
