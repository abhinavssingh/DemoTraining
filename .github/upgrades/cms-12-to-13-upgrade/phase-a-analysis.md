# Phase A: Repository Analysis — CMS 12 → CMS 13 Upgrade

**Date**: 2026-02-16  
**Branch**: `cms-12-to-13-upgrade`  
**Framework**: .NET 8 (confirmed)  
**Current CMS Version**: Optimizely CMS 12.x

---

## 1. Repository Structure

```
C:\Optimizely\DemoTraining\
├── Program.cs                    (Host builder configuration)
├── Startup.cs                    (ConfigureServices, Configure pipeline)
├── Globals.cs                    (Group names, system constants)
│
├── Models/
│   ├── SiteContentType.cs       (Custom ContentTypeAttribute base)
│   ├── Pages/
│   │   └── SitePageData.cs      (Abstract page base class with Display attributes)
│   ├── Blocks/
│   │   ├── SiteBlockData.cs     (Abstract block base)
│   │   └── ContactBlock.cs      (Custom block implementation)
│   ├── ViewModels/
│   │   └── PageViewModel.cs     (Generic page view model)
│   └── Media/
│       └── GenericFile.cs
│
├── Business/
│   ├── Initialization/
│   │   └── CustomizedRenderingInitialization.cs  (IConfigurableModule implementation)
│   ├── Channels/
│   │   ├── DisplayResolutionBase.cs    (IDisplayResolution base class)
│   │   ├── DisplayResolutions.cs       (Display resolution implementations)
│   │   └── MobileChannel.cs            (Mobile channel definition)
│   ├── Rendering/
│   │   ├── TemplateCoordinator.cs      (Template resolution event handling)
│   │   ├── ErrorHandlingContentRenderer.cs
│   │   └── ContentAreaItemRenderer.cs
│   ├── ContentLocator.cs               (IContentLoader, PageCriteriaQueryService usage)
│   ├── PageViewContextFactory.cs       (PageDataFactory, Context creation)
│   ├── PageContextActionFilter.cs      (MVC action filter)
│   └── UIDescriptors/
│       └── ContainerPageUIDescriptor.cs
│
├── Features/
│   ├── Standard/Models/StandardPage.cs (PageData subclass)
│   ├── StartPage/Models/StartPage.cs   (PageData subclass with [Display] attributes)
│   ├── Product/Models/ProductPage.cs
│   ├── News/Models/
│   ├── Components/
│   │   ├── SitelogoType/Models/SiteLogotypeBlock.cs (BlockData subclass)
│   │   ├── Button/Models/ButtonBlock.cs (BlockData subclass)
│   │   ├── Teaser/Models/TeaserBlock.cs (BlockData subclass)
│   │   ├── Editorial/
│   │   └── Jumbotron/
│   ├── PropertyTypesDemo/Models/PropertyTypesDemoPage.cs
│   ├── FieldValidation/
│   ├── Contact/Models/
│   ├── Landing/Models/
│   ├── Search/
│   └── HomePage/
│
├── Controllers/
│   └── DefaultPageController.cs  (IContentLoader<T> usage)
│
├── Extensions/
│   ├── TinyMceConfigurationExtensions.cs
│   └── EpiserverOptions.cs       (Custom config options class)
│
├── Helpers/
│   ├── HtmlHelpers.cs           (HTML rendering helpers)
│   ├── UrlHelpers.cs            (URL generation)
│   └── CategorizableExtensions.cs
│
├── Resources/
│   └── Translations/
│
├── Views/
│   ├── Shared/
│   │   ├── Layouts/_Root.cshtml
│   │   ├── DisplayTemplates/
│   │   └── Components/
│   ├── Preview/
│   └── [Content-specific views]
│
└── wwwroot/
	├── css/
	├── js/
	└── gfx/
```

---

## 2. CMS Footprint Analysis

### Current NuGet Dependencies
```
- EPiServer.CMS (12.34.2)              ← Main CMS package
- EPiServer.CMS.AspNetCore.TagHelpers (12.23.1)  ← Tag helpers
- EPiServer.Find.Cms (16.7.0)          ← Search indexing
- EPiServer.Forms (5.10.6)             ← Forms addon
- EPiServer.Labs.GridView (1.2.0)      ← Lab feature
- Microsoft.Extensions.Configuration (10.0.2)   ← Config provider
- Wangkanai.Detection (8.20.0)         ← Device detection (third-party)
```

**Target Versions for CMS 13:**
- `EPiServer.CMS` → `13.x` (breaking changes expected)
- `EPiServer.CMS.AspNetCore.TagHelpers` → `13.x`
- `EPiServer.Find.Cms` → will need compatibility check (may stay 16.x)
- `EPiServer.Forms` → likely `6.x` or `7.x` for CMS 13 compatibility
- `EPiServer.Labs.GridView` → check availability for CMS 13

### Key Patterns & Patterns to Migrate

#### 1. **IConfigurableModule Pattern** ✅ Found
**File**: `Business/Initialization/CustomizedRenderingInitialization.cs`

```csharp
[ModuleDependency(typeof(InitializationModule))]
public class CustomizedRenderingInitialization : IConfigurableModule
{
	public void ConfigureContainer(ServiceConfigurationContext context) { ... }
	public void Initialize(InitializationEngine context) { ... }
	public void Uninitialize(InitializationEngine context) { ... }
}
```

**CMS 13 Migration Status**: ⚠️ **BREAKING CHANGE**
- CMS 13 deprecated `IConfigurableModule` and `InitializationEngine`
- Must convert to automatic DI registration via extension methods on `IServiceCollection`
- `ServiceLocator` pattern removed from CMS initialization
- Event handlers must use constructor injection instead

#### 2. **ServiceConfiguration Attribute** ✅ Found
**File**: `Business/ContentLocator.cs`

```csharp
[ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton)]
public class ContentLocator { ... }
```

**CMS 13 Migration Status**: ⚠️ **DEPRECATED**
- `ServiceConfiguration` attribute is deprecated in CMS 13
- Must register services in DI container via extension methods in Startup.cs
- Pattern: Move `[ServiceConfiguration]` classes to extension methods on `IServiceCollection`

#### 3. **Content Type Definitions** ✅ Found
**Files**:
- `Models/Pages/SitePageData.cs` (base page class with `[Display]`, `[CultureSpecific]`, `[BackingType]`, `[UIHint]`)
- `Features/*/Models/*.cs` (various PageData/BlockData subclasses)
- `Models/SiteContentType.cs` (custom `ContentTypeAttribute`)

**CMS 13 Migration Status**: ⚠️ **PARTIAL BREAKING CHANGES**
- `[Display]` attributes → still supported
- `[CultureSpecific]` → may be replaced/updated in CMS 13
- `[BackingType(typeof(PropertyStringList))]` → CMS 13 uses new property backing patterns
- `[UIHint(...)]` → some hints may change; verify each custom UIHint
- `ContentTypeAttribute` → base class; check for new attributes in CMS 13

#### 4. **Template Resolution** ✅ Found
**File**: `Business/Rendering/TemplateCoordinator.cs`

```csharp
context.Locate.Advanced.GetInstance<ITemplateResolverEvents>()
	.TemplateResolved += TemplateCoordinator.OnTemplateResolved;
```

**CMS 13 Migration Status**: ⚠️ **BREAKING CHANGE**
- `InitializationEngine.Locate` (ServiceLocator) removed
- `ITemplateResolverEvents` pattern may change
- Must use event handling via DI-registered event subscribers

#### 5. **Content Rendering** ✅ Found
**File**: `Business/Rendering/ErrorHandlingContentRenderer.cs`, `ContentAreaItemRenderer.cs`

**Pattern**: Custom `IContentRenderer` implementation registered in `ConfigureContainer`

**CMS 13 Migration Status**: ⚠️ **API UPDATE LIKELY**
- `IContentRenderer` interface may have breaking changes
- Custom implementations must match new interface signature

#### 6. **Content Loading** ✅ Found
**Files**:
- `Business/ContentLocator.cs` (uses `IContentLoader`, `IPageCriteriaQueryService`)
- `Business/PageViewContextFactory.cs` (PageData factory patterns)
- `Controllers/DefaultPageController.cs` (IContentLoader<T> usage)

**CMS 13 Migration Status**: ✅ **LIKELY COMPATIBLE**
- `IContentLoader` API expected to be stable in CMS 13
- `PageData` base class stable (but check for deprecated properties)

#### 7. **MVC/Razor Pages** ✅ Found
**Files**:
- `Controllers/DefaultPageController.cs`
- `Views/Shared/Layouts/*.cshtml`
- Razor views throughout Features/

**Pattern**: ASP.NET Core MVC with Razor Pages/Views

**CMS 13 Migration Status**: ✅ **COMPATIBLE**
- ASP.NET Core MVC patterns stable
- Razor view rendering not affected by CMS 13
- Note: Workspace context mentions Razor Pages project

#### 8. **Custom Attributes** ✅ Found
**File**: `Models/SiteContentType.cs`

```csharp
public class SiteContentType : ContentTypeAttribute
{
	public SiteContentType() { GroupName = Globals.GroupNames.Default; }
}
```

**CMS 13 Migration Status**: ⚠️ **VERIFY COMPATIBILITY**
- Inheriting from `ContentTypeAttribute` likely still works
- Check if any base class properties changed in CMS 13

#### 9. **Display Resolutions** ✅ Found
**Files**: `Business/Channels/DisplayResolutionBase.cs`, `Business/Channels/DisplayResolutions.cs`

**Pattern**: `IDisplayResolution` implementations for responsive design

**CMS 13 Migration Status**: ✅ **LIKELY COMPATIBLE**
- Display resolution API expected stable in CMS 13

#### 10. **Configuration** ✅ Found
**File**: `appsettings.json`, `Extensions/EpiserverOptions.cs`

**CMS 13 Migration Status**: ⚠️ **NEEDS VALIDATION**
- Check if CMS 13 introduces new required configuration sections
- Verify EPiServer logging, authentication, media import settings align

---

## 3. Hosting Model Analysis

**Current Model**: Classic Startup-based hosting (Program.cs + Startup.cs)

```csharp
// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureCmsDefaults()
		.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
```

**CMS 13 Hosting Changes**: ⚠️ **SIGNIFICANT CHANGES EXPECTED**
- `ConfigureCmsDefaults()` method name/signature may change in CMS 13
- `UseStartup<Startup>()` is .NET 5+ pattern (✅ compatible with .NET 8)
- Middleware registration order may differ in CMS 13
- New CMS 13 middleware may be required

**Target Pattern for CMS 13**:
- Keep Startup.cs pattern (still supported in .NET 8)
- Update `ConfigureCmsDefaults()` call to CMS 13 equivalent
- Update middleware registration in `Configure()` method to match CMS 13 requirements

---

## 4. Deprecated APIs & Breaking Changes (CMS 12 → 13)

| Category | Breaking Change | Mitigation |
|----------|-----------------|-----------|
| **Initialization** | `IConfigurableModule` deprecated | Convert to DI extension methods |
| **Initialization** | `InitializationEngine` & `ServiceLocator` removed | Use constructor injection |
| **Content Types** | `[BackingType]` pattern may change | Verify with CMS 13 docs |
| **Content Types** | Some `[UIHint]` values changed | Check each custom UIHint |
| **Rendering** | `IContentRenderer` interface may change | Verify & adapt custom implementations |
| **Events** | `ITemplateResolverEvents` pattern may change | Update event subscription pattern |
| **Hosting** | `ConfigureCmsDefaults()` signature likely changes | Update Program.cs |
| **Middleware** | New/removed middleware in CMS 13 | Update Startup.Configure() |
| **Properties** | Some `PageData` properties may be deprecated | Scan for warnings post-upgrade |

---

## 5. Third-Party Packages

| Package | Current | Status | Action |
|---------|---------|--------|--------|
| **Wangkanai.Detection** | 8.20.0 | ✅ No CMS dependency | Keep or update freely |
| **Microsoft.Extensions.Configuration** | 10.0.2 | ✅ Framework package | Will update with .NET 8 |

---

## 6. Summary of Required Changes

### **Must Fix** (Blocking)
1. ✅ Update all `EPiServer.CMS*` packages to v13.x
2. ✅ Convert `IConfigurableModule` to DI-based pattern
3. ✅ Replace `ServiceLocator` usage with constructor injection
4. ✅ Update `Program.cs` for CMS 13 API changes
5. ✅ Update `Startup.Configure()` middleware registration

### **Should Fix** (Recommended)
1. Verify all `[UIHint]` custom hints work with CMS 13
2. Verify `[Display]` attributes still work as-is
3. Update `IContentRenderer` implementations if signature changed
4. Validate `appsettings.json` for CMS 13 requirements
5. Test custom `IDisplayResolution` implementations

### **Nice to Have** (Optimization)
1. Remove deprecated `[ServiceConfiguration]` attributes entirely
2. Modernize any remaining `ServiceLocator` calls
3. Consider newer CMS 13 features (e.g., async content loading)

---

## 7. Next Steps

→ **Phase B**: Validate environment (SDK, database)  
→ **Phase C**: Update NuGet packages to CMS 13 versions  
→ **Phase D**: Update hosting model (Program.cs, Startup.cs)  
→ **Phase E**: Convert initialization patterns to DI-based  
→ **Phase F**: Update content API usage  
→ **Phase G**: Validate controllers & features  
→ **Phase H**: Confirm configuration compatibility  
→ **Phase I**: Fix compilation errors  
→ **Phase J**: Document changes & validation checklist
