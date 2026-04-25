# Phase 2: Search Assessment - EPiServer.Find Usage Patterns

**Date**: 2026-02-17  
**Status**: Assessment Complete  
**Migration Target**: Optimizely Graph

## Current Search Implementation Summary

### Components Identified

#### 1. **SearchPageController.cs** (Currently Disabled)
- **Status**: Commented out with TODO CMS13 markers
- **Original Pattern**: 
  - Constructor injection: `IClient _client` (Find search client)
  - Query builder: `.UnifiedSearchFor(q.Q).TermsFacetFor().FilterHits()`
  - Async pattern: `.GetResultAsync().Result` (sync over async - anti-pattern)
  - Result type: `UnifiedSearchResults`
  - Faceting: `TermsFacet`, `.TermsFacetFor()`

**Original Behavior**:
```csharp
// Pseudo-code of disabled pattern
var query = _client
	.UnifiedSearchFor(searchQuery)          // Full-text search
	.TermsFacetFor(x => x.SearchSection)    // Faceting by section
	.TermsFacetFor(x => x.SearchCategories) // Faceting by category
	.Skip((page - 1) * pageSize)            // Paging
	.Take(pageSize);

if (sections?.Count > 0)
	query = query.FilterHits(h => h.SearchSection.In(sections));
if (types?.Count > 0)
	query = query.FilterHits(f => f.SearchCategories.In(types));

var results = query.GetResultAsync().Result; // Sync blocking call
```

#### 2. **SearchContentModel.cs** (Partially Updated)
- **Current State**: Uses `object` placeholders for Find types
- **Properties**:
  - `SearchServiceDisabled = true`
  - `Results = null` (was `UnifiedSearchResults`)
  - `RawFacets = Enumerable.Empty<object>()` (was `IEnumerable<TermsFacet>`)
  - `Facets = new List<FacetBucket>()` (custom facet container)

#### 3. **SearchQuery.cs** (Working)
- **Properties**: Q (query string), Sections (filters), Types (filters), Page, PageSize
- **No changes needed** - generic enough for Graph

#### 4. **Search Views** (Index.cshtml - Partially Disabled)
- Search results loop commented out
- Disabled message shown when `SearchServiceDisabled = true`

### EPiServer.Find API Patterns to Migrate

| **Find API** | **Usage** | **Graph Equivalent** |
|---|---|---|
| `IClient` | Injected search client | Graph Query Client (async) |
| `.UnifiedSearchFor(query)` | Full-text search | `.QueryContent<IContent>()` with `.Filter()` |
| `.TermsFacetFor(x => x.Property)` | Aggregation/faceting | Graph aggregations/facets (TBD) |
| `.FilterHits(x => x.Property.In(values))` | Content filtering | `.Filter(x => x.Property.In(values))` |
| `.Skip(n).Take(m)` | Pagination | Graph `.Limit()` and `.Offset()` or cursor |
| `.GetResultAsync().Result` | Execute query | Proper `.GetAsContentAsync()` or similar |
| `UnifiedSearchResults` | Result type | Graph response model |
| `TermsFacet` | Facet type | Graph facet model |

### Search Behavior to Preserve

1. **Full-text search** on content title, body, metadata
2. **Filtering by section** (content hierarchy category)
3. **Filtering by content type** (page type, block type)
4. **Pagination** with configurable page size (default 20)
5. **Faceted search** showing available sections and types
6. **Result display** with title, excerpt, URL

### Configuration Discovery

- **Current EPiServer.Find config** in EpiserverOptions.cs:
  - `Find.DefaultIndex`
  - `Find.ServiceUrl`
  - **Find service not registered in DI** (Find package removed from csproj)

- **Graph configuration needed**:
  - Gateway URL
  - App key / API key
  - Secret
  - Credentials management strategy

## Migration Strategy

**Phase 4 Approach**:
1. Implement `IGraphSearchService` wrapper to abstract Graph SDK
2. Inject as service in `SearchPageController`
3. Rewrite `SearchPageController.Index()` to:
   - Accept current `SearchQuery` parameters
   - Build Graph query with filters, facets, pagination
   - Execute async with `await`
   - Map Graph results to existing `SearchContentModel`
   - Return view with populated results

**Key Changes**:
- ✅ Constructor: Inject `IGraphSearchService` (async-first)
- ✅ Controller action: Convert to `async Task<ViewResult>`
- ✅ Query building: Use Graph C# SDK query syntax
- ✅ Result mapping: Adapt Graph response to existing model
- ✅ Error handling: Handle Graph API errors gracefully

**Manual Steps Post-Migration**:
- [ ] Configure Graph gateway and credentials
- [ ] Test search queries against Graph API
- [ ] Validate faceting behavior
- [ ] Performance test with production-like data volume
- [ ] User acceptance testing of search results

## Next Steps

- **Phase 3**: Install Graph SDK packages
- **Phase 4**: Implement IGraphSearchService using Graph SDK
- **Phase 5**: Update SearchPageController to async Graph
