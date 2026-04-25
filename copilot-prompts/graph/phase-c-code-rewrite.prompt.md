@modernize migrate search code from Find to Graph.

Rewrite rules:
- Search<T>() → QueryContent<T>()
- For() → SearchFor()
- Filter / FilterForVisitor() → WithDisplayFilters()
- Take() → Limit()
- GetContentResult() → GetAsContentAsync()

Rules:
- Preserve behavior exactly
- Convert to async/await
- Add TODOs only for manual verification