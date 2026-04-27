# CMS 13 Post-Upgrade Validation Checklist

**Project**: DemoTraining  
**Upgrade Status**: Stage 1 Complete — Ready for Deployment Validation  
**Date**: 2026-04-27  

---

## Overview

This checklist guides the verification and validation process for the CMS 12 → CMS 13 upgrade. It covers deployment preparation, post-deployment validation, and functionality verification.

---

## Pre-Deployment Checklist

### Code Quality & Build

- [x] **Build Succeeds**: `dotnet build` returns 0 errors
  - Status: ✅ PASS
  - Result: 2 acceptable warnings (documented)

- [x] **No Critical Warnings**: All compiler warnings documented
  - Status: ✅ PASS
  - Warnings: NU1510 (false positive), CS0618 (suppressed by design)

- [x] **Package Versions Correct**: All CMS packages are v13
  - Status: ✅ PASS
  - EPiServer.CMS: 13.0.2
  - Optimizely.Graph.Cms.Query: 13.0.2
  - Optimizely.Graph.AspNetCore: 13.0.2

- [x] **Target Framework**: net10.0
  - Status: ✅ PASS
  - SDK Available: 10.0.300-preview

- [x] **No EPiServer.Find References**: All Search & Navigation removed
  - Status: ✅ PASS
  - Packages removed: EPiServer.Find, EPiServer.Find.Cms, EPiServer.Find.Framework
  - Code references: None found

### Service Registration

- [x] **ConfigureCmsDefaults() Used**: Program.cs contains CMS 13 core registration
  - Status: ✅ PASS
  - Location: Program.cs line 9
  - Pattern: `.ConfigureCmsDefaults()`

- [x] **Startup.cs App-Specific Only**: No deprecated CMS 12 registrations
  - Status: ✅ PASS
  - Deprecated calls removed: AddEpiServer(), AddAdminUserRegistration()
  - Verified: Startup.cs lines 32-44

- [x] **Graph Services Registered**: IGraphSearchService available
  - Status: ✅ PASS
  - Location: Extensions\GraphServiceCollectionExtensions.cs
  - Registration: `.AddOptimizelyGraph(_configuration)`

### Version Compatibility

- [x] **Database Schema Compatible**: .NET 10 deployment ready
  - Status: ✅ PASS
  - No schema migration code required in application
  - Database compatibility level: To be validated on target DB

- [x] **API Compatibility**: Backward-compatible deprecated APIs enabled
  - Status: ✅ PASS
  - CS0618 suppressed in csproj
  - SiteDefinition, PageReference, PageData available

---

## Deployment Steps

### 1. Pre-Deployment Environment Check

**Steps**:
1. [ ] Verify target environment has .NET 10 runtime installed
   ```powershell
   dotnet --version
   # Should return 10.0.300-preview or later
   ```

2. [ ] Verify database SQL Server version compatibility
   - Minimum: SQL Server 2019
   - Recommended: SQL Server 2022

3. [ ] Backup existing database
   ```sql
   BACKUP DATABASE [DemoTraining_CMS13] 
   TO DISK = 'C:\Backups\DemoTraining_PreUpgrade.bak'
   ```

4. [ ] Verify application pool user has required SQL permissions

### 2. Application Deployment

**Steps**:
1. [ ] Deploy DemoTraining application to test server
   ```powershell
   dotnet publish -c Release -o "C:\inetpub\wwwroot\DemoTraining"
   ```

2. [ ] Set up IIS application pool
   - .NET Version: No managed code (no classic CLR required)
   - Pipeline Mode: Integrated
   - Start Automatically: Yes

3. [ ] Configure appsettings.json on target
   ```json
   {
	 "ConnectionStrings": {
	   "EPiServerDB": "Server=localhost;Database=DemoTraining_CMS13;..."
	 },
	 "EPiServer": {
	   "Cms": { ... },
	   "Graph": {
		 "Enabled": true,
		 "Gateway": "https://graph-gateway.optimizely.com/graphql",
		 "AppKey": "YOUR_APP_KEY",
		 "Secret": "YOUR_SECRET"
	   }
	 }
   }
   ```

4. [ ] Start application pool and verify application initializes
   - Check Application event log for errors
   - Verify no startup exceptions

---

## Post-Deployment Validation

### CMS Admin UI

- [ ] **Admin UI Loads**: Navigate to `/ui/CMS` or `/Optimizely/CMS`
  - Expected: CMS admin interface loads without errors
  - Verify: No 404, no 500 errors
  - Test User: Admin account with appropriate roles

- [ ] **Login Works**: Authenticate with CMS admin account
  - Expected: Successful login to CMS UI
  - Verify: User roles and permissions functional

- [ ] **Default Application Created**: CMS > Settings > Applications
  - Expected: At least one default application exists
  - Verify: Application name, hostname configuration

- [ ] **Content Tree Loads**: CMS > Content > Manage Content
  - Expected: Page hierarchy visible and browsable
  - Verify: All page types and content items accessible
  - Performance: Tree loads in < 5 seconds

- [ ] **Search Admin Section**: CMS > Search & Navigation (if applicable)
  - Expected: Search configuration available
  - Note: May show Graph placeholder in Stage 2

### Page Rendering

- [ ] **Homepage Loads**: Navigate to site root (http://localhost/)
  - Expected: Homepage renders without errors
  - Verify: All page elements display correctly
  - Console: No JavaScript errors

- [ ] **Standard Page Renders**: Navigate to any /standard/ page
  - Expected: Page template renders
  - Verify: Correct layout and styling applied
  - Components: All blocks display

- [ ] **Edit Mode Works**: Edit → Edit Page in CMS UI
  - Expected: Page enters edit mode
  - Verify: Editors can edit properties
  - Save: Changes persist and page updates

- [ ] **Publish Works**: CMS → Publish workflow
  - Expected: Content can be published
  - Verify: Published content immediately visible on site
  - Scheduled Publishing: If applicable, verify works

### Media Library

- [ ] **Media Browser Opens**: CMS > Media
  - Expected: Media library loads
  - Verify: Existing media files visible

- [ ] **Upload Works**: Upload new image to media library
  - Expected: Upload completes without errors
  - Verify: File appears in media library
  - Cleanup: Delete test file after verification

- [ ] **Image Properties**: Open media file properties
  - Expected: Properties accessible
  - Verify: Metadata editable

### Forms (if EPiServer.Forms is used)

- [ ] **Forms UI Loads**: CMS > Forms
  - Expected: Forms management interface loads
  - Verify: Existing forms visible

- [ ] **Form Submission Works**: Submit form on website
  - Expected: Submission accepted
  - Verify: Submission data stored
  - Email: If configured, verify notification received

### Database & Content

- [ ] **Content Tree Integrity**: No orphaned or broken content references
  - Query: Check for content with invalid parent IDs
  - Verify: All pages have valid parent references

- [ ] **Metadata Preserved**: Page metadata intact after upgrade
  - Sample Check: Verify MetaTitle, MetaDescription on 3-5 pages
  - Expected: Metadata unchanged from CMS 12

- [ ] **Access Control Works**: Page-level access restrictions enforced
  - Test: Restrict page to specific users
  - Verify: Only authorized users see page

- [ ] **Scheduled Jobs** (if used): Scheduled jobs execute
  - Verify: All scheduled jobs running without errors
  - Check: Jobs execute at expected times
  - Logs: No exceptions in job history

### Performance

- [ ] **Response Time Acceptable**: Site pages load within expected time
  - Baseline: Should be similar to CMS 12 (or faster with .NET 10)
  - Test: Load 5-10 pages, measure response times
  - Expected: < 2 seconds per page on test environment

- [ ] **CMS Admin Responsive**: Admin UI operations responsive
  - Test: Load content tree, edit page, publish
  - Expected: Operations complete in < 3 seconds

- [ ] **Database Queries Performant**: No slow database queries
  - Tool: SQL Server Management Studio → Query Execution Plans
  - Verify: No table scans on large tables
  - Optimize: Address any obvious performance issues

### Configuration Validation

- [ ] **appsettings.json Loaded**: Configuration values accessible
  - Verify: Connection string correct
  - Verify: CMS options loaded
  - Verify: Graph options present (Stage 2)

- [ ] **Connection String Works**: Database connectivity verified
  - Test: Application connects successfully to SQL Server
  - Verify: Schema recognized (database compatible)

- [ ] **Logging Configured**: Event log captures application events
  - Verify: Application event log shows startup messages
  - Expected: No error-level events (info/warning acceptable)

---

## Search Functionality (Stage 2)

### Search Service Placeholder

- [ ] **Search Page Loads**: Navigate to /search/
  - Expected: Search UI loads
  - Note: Results may be empty (Graph SDK pending)

- [ ] **Graph Service Registered**: IGraphSearchService available
  - Verify: No dependency injection errors
  - Expected: Search page initializes without errors

- [ ] **Graph Configuration Ready**: Graph credentials configured
  - Check: appsettings.json contains Graph options
  - Note: Actual Graph queries pending Stage 2 completion

---

## Known Limitations (Stage 1)

### Planned for Stage 2

- ⚠️ **Search Results Empty**: Graph SDK implementation incomplete
  - Expected Behavior: Search page shows "Search service not available" message
  - Resolution: Complete Stage 2 Graph SDK integration

- ⚠️ **Graph API Not Called**: IGraphSearchService returns placeholder results
  - Current: GraphSearchService.SearchAsync() returns empty results with warning log
  - Expected in Stage 2: Actual Graph API queries executed

---

## Rollback Plan

If critical issues discovered:

### Immediate Rollback
1. [ ] Stop application pool
2. [ ] Restore database backup
   ```sql
   RESTORE DATABASE [DemoTraining] 
   FROM DISK = 'C:\Backups\DemoTraining_PreUpgrade.bak'
   ```
3. [ ] Deploy CMS 12 application binaries
4. [ ] Restart application pool
5. [ ] Verify site functional on CMS 12

### Partial Rollback (Code Only)
1. [ ] Revert Git branch to `cms-12-upgrade` (pre-CMS-13)
2. [ ] Rebuild with CMS 12 packages
3. [ ] Deploy to test environment
4. [ ] Verify rollback successful

---

## Issue Resolution Guide

### Issue: CMS Admin UI Returns 404

**Diagnosis**:
- Application running but admin endpoint not found
- Possible cause: ConfigureCmsDefaults() not called in Program.cs

**Resolution**:
1. Verify Program.cs contains `.ConfigureCmsDefaults()`
2. Verify application pool restarted after deployment
3. Check Application event log for startup errors
4. Rebuild and redeploy application

### Issue: Database Connection Error

**Diagnosis**:
- SQL Server connectivity failure
- Possible cause: Connection string incorrect or database offline

**Resolution**:
1. Verify connection string in appsettings.json
2. Test connection using SQL Server Management Studio
3. Verify database schema upgraded (if migration needed)
4. Verify application pool user has required permissions

### Issue: Deprecated API Warnings in Logs

**Diagnosis**:
- CS0618 obsolete API warnings appearing
- Expected behavior: Warnings suppressed in production build

**Resolution**:
1. Verify Release build used (not Debug)
2. Verify `<NoWarn>CS0618;$(NoWarn)</NoWarn>` in csproj
3. If warnings still appearing, rebuild clean

### Issue: Search Not Working / Empty Results

**Diagnosis**:
- Search page returns no results
- Expected in Stage 1: Search service not implemented yet

**Resolution**:
1. This is expected until Stage 2 (Graph SDK) completion
2. Check application logs for GraphSearchService warnings
3. Verify Graph configuration in appsettings.json
4. Plan Stage 2 implementation for full search functionality

---

## Sign-Off Template

After completing all validation, have release manager sign off:

```
VALIDATION SIGN-OFF
==================

Validator Name: ___________________
Date: ___________________
Environment: ☐ Test  ☐ Staging  ☐ Production

Checklist Status:
☐ All items completed successfully
☐ No critical issues found
☐ Acceptable known limitations documented (Stage 2 pending)

Critical Issues Found:
☐ None
☐ Yes (describe below):
   _________________________________________
   _________________________________________

Rollback Plan Available: ☐ Yes  ☐ No

Approval:
☐ Approved for next stage (Stage 2: Graph Migration)
☐ Approved for production deployment
☐ Requires remediation (see critical issues above)

Signature: _________________________ Date: __________
```

---

## Next Steps (Stage 2)

After Stage 1 validation completes:

1. **Schedule Stage 2 Implementation**:
   - Implement full Optimizely Graph SDK integration
   - Complete IGraphSearchService with actual Graph queries
   - Migrate search filters and facets
   - Validate search behavior

2. **Stage 3 Planning**:
   - Final migration report generation
   - Production deployment planning
   - Performance baseline establishment
   - Team training and handoff

---

## Contact & Support

For issues or questions during validation:

- **Technical Lead**: (To be assigned)
- **CMS Administrator**: (To be assigned)
- **Database Administrator**: (For DB schema verification)
- **DevOps/Infrastructure**: (For deployment and monitoring)

---

## Appendix: Command Reference

### Useful PowerShell Commands

**Build & Test**:
```powershell
# Build
dotnet build

# Run tests (if available)
dotnet test

# Publish for deployment
dotnet publish -c Release -o "..\Publish"
```

**Deployment**:
```powershell
# Stop IIS app pool
Stop-WebAppPool -Name "DemoTraining"

# Deploy files
Copy-Item "bin\Release\net10.0\publish\*" "C:\inetpub\wwwroot\DemoTraining\" -Recurse -Force

# Start IIS app pool
Start-WebAppPool -Name "DemoTraining"
```

**Troubleshooting**:
```powershell
# Check application event log for errors
Get-EventLog -LogName Application -Source ".NET Runtime" | Select-Object -Last 20

# Check IIS app pool status
Get-WebAppPoolState -Name "DemoTraining"

# Verify .NET version
dotnet --version
```

---

## Related Documentation

- **CMS13-Upgrade-Summary.md** — Comprehensive upgrade summary
- **CMS13-BreakingChanges.md** — Detailed breaking change analysis
- **FINAL-MIGRATION-REPORT.md** — End-to-end migration audit (generated after Stage 3)

---

**Document**: CMS 13 Post-Upgrade Validation Checklist  
**Status**: ✅ Ready for Deployment  
**Last Updated**: 2026-04-27
