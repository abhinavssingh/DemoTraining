@modernize use the skill `modernize-optimizely-search-to-graph`.

Migrate Optimizely Search & Navigation (EPiServer.Find)
to Optimizely Graph.

Perform all phases:
- Search analysis
- Package replacement
- Query rewrite
- Configuration update
- Cleanup and documentation

Rules:
- Search must be migrated, not disabled
- Preserve query intent, ordering, paging, and filters
- Use async Graph APIs
- Do not hardcode secrets

Produce:
- Graph-Migration.md
- Graph-Manual-Validation.md