# Phase C: NuGet Package Update — CRITICAL DECISION REQUIRED

**Date**: 2026-02-16  
**Status**: ⚠️ **BLOCKING DECISION NEEDED**

---

## 🚨 Critical Discovery

### CMS 13 Framework Requirement vs Current Project

| Aspect | Current State | CMS 13 Requirement |
|--------|---------------|-------------------|
| **Project Target Framework** | `.NET 8` | Must upgrade to `.NET 10` |
| **EPiServer CMS Latest** | 12.34.3 (for .NET 8) | 13.0.0 (requires .NET 10) |
| **Status** | ✅ Confirmed running | ❌ **INCOMPATIBLE** |

### Error Message from NuGet
```
NU1202: Package EPiServer.Cms 13.0.0 is not compatible with net8.0 (.NETCoreApp,Version=v8.0). 
Package EPiServer.Cms 13.0.0 supports: net10.0 (.NETCoreApp,Version=v10.0)
```

---

## 🎯 What This Means

**Your current upgrade path cannot proceed as initially planned** because:

1. ✅ **CMS 12 to CMS 13** requires transitioning from .NET 8 → .NET 10
2. ❌ Your project targets .NET 8 (confirmed in DemoTraining.csproj)
3. ❌ EPiServer CMS 13 only supports .NET 10.0

### The Correct Upgrade Sequence

To successfully upgrade to CMS 13, you must **FIRST** upgrade .NET from 8 → 10:

```
CMS 12 (.NET 8)
	↓
CMS 12 (.NET 10)  ← Intermediate step required
	↓
CMS 13 (.NET 10)  ← Final state
```

---

## 📋 Available Paths Forward

### **Option A: Proceed with CMS 12 → 13 + .NET 8 → 10 Upgrade** ✅ RECOMMENDED
- Scope: Comprehensive, addresses both framework and CMS updates
- Timeline: Longer (2 major upgrades)
- Complexity: High (need to handle both .NET 10 API changes + CMS 13 API changes)
- **Decision**: This makes the task significantly larger

### **Option B: Stay on CMS 12.x for .NET 8** (Not CMS 13)
- Scope: Limited, no CMS upgrade
- Timeline: Short
- Complexity: Low
- **Tradeoff**: Misses CMS 13 features and improvements
- **Not Recommended**: Defeats the purpose of your upgrade request

### **Option C: Upgrade to .NET 10 First, Then Plan CMS 13 Separately**
- Scope: Two separate phases
- Timeline: Medium
- Complexity: Medium (easier to debug each upgrade independently)
- **Tradeoff**: Requires two separate workflows/branches

---

## 💡 Recommendation

**I recommend Option A** — proceed with a **two-phase upgrade**:

### **Phase I: .NET 8 → .NET 10 Upgrade**
1. Update `<TargetFramework>net8.0</TargetFramework>` → `net10.0`
2. Run `dotnet build` and fix any .NET 10 API compatibility issues
3. Update dependencies that may have .NET 10 requirements
4. Test and validate .NET 10 build succeeds

### **Phase II: CMS 12 → CMS 13 + Update Packages**
1. Update EPiServer.CMS to 13.x
2. Update all dependent packages (Forms, TagHelpers, Find, etc.)
3. Address CMS 13 API breaking changes (ServiceLocator, IConfigurableModule, etc.)
4. Test and validate CMS 13 upgrade succeeds

---

## 🔧 Updated Plan Structure

Your original 10-phase plan should be **revised**:

### **New Sequence** (13 phases total)
1. **Phase A**: Repository Analysis ✅ DONE
2. **Phase B**: Environment Validation ✅ DONE
3. **Phase C (NEW)**: Upgrade to .NET 10
   - Update target framework
   - Fix .NET 10 API compatibility
   - Run build and tests
4. **Phase D (Original Phase C)**: Update NuGet Packages to CMS 13
   - Update EPiServer.CMS to 13.x
   - Update satellite packages
   - Validate versions align
5. **Phase E (Original Phase D)**: Update Hosting Model
   - Update Program.cs for CMS 13
   - Update Startup.cs middleware
6. ... (remaining phases same)

---

## ❓ Questions for You

**Before proceeding, please confirm:**

1. **Do you want to upgrade to CMS 13?**  
   → If YES, you MUST also upgrade to .NET 10 (no choice)  
   → If NO, stay on CMS 12.x for .NET 8 (keep current state)

2. **Are you comfortable upgrading to .NET 10?**  
   → .NET 10 is very new (2024-2025)  
   → Production readiness: Check your infrastructure support  
   → CI/CD pipelines: May need .NET 10 SDK

3. **Should I proceed with the two-phase upgrade (A: .NET 8→10, then B: CMS 12→13)?**  
   → YES: Let me restructure the plan and execute both upgrades  
   → NO: Let me help you with an alternative approach

---

## 📌 What I Did (Rollback)

I attempted to add `EPiServer.CMS 13.0.0` to your project to test, but it failed due to the .NET 8 incompatibility. I **reverted the change** — your csproj is back to the original state:

```
git checkout -- DemoTraining.csproj
```

**Your current project remains**: CMS 12.34.2, .NET 8, unchanged

---

## ⏭️ Next Steps (Awaiting Your Input)

Please answer the three questions above, and I will:

1. **If YES to CMS 13 + .NET 10**: Restructure the plan to include .NET 10 upgrade, then proceed with both
2. **If NO to CMS 13**: Help you with alternative optimizations for CMS 12.x on .NET 8
3. **If uncertain**: Provide more details on .NET 10 and CMS 13 benefits/risks

---

## 📚 Reference Links

- [Optimizely CMS 13 Documentation](https://docs.developers.optimizely.com/content-management-system/v13.0.0-CMS/docs)
- [.NET 10 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/10.0.0/10.0.0.md)
- [Migration from .NET 8 to .NET 10](https://learn.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant)
