# Orchestrated CMS 13 Upgrade — Task Hierarchy

## Stage 1: CMS 13 Upgrade (Platform + CMS)
**Status**: ✅ COMPLETE
**Gate**: Must complete before Stage 2 begins

### Task 01.01-platform-runtime-upgrade
- **Description**: Upgrade .NET runtime from current version to .NET 10
- **Status**: ✅ COMPLETE
- **Dependencies**: None
- **Owner**: Core infrastructure
- **Result**: net10.0 targeting verified; SDK 10.0.300-preview available

### Task 01.02-cms-packages-upgrade
- **Description**: Upgrade all EPiServer.* and Optimizely.* CMS packages to v13
- **Status**: ✅ COMPLETE
- **Dependencies**: Task 01.01 must complete
- **Owner**: CMS platform
- **Result**: All CMS 13.0.2 packages present; EPiServer.Find removed

### Task 01.03-cms-api-migration
- **Description**: Migrate breaking API changes (service registration, Applications model, admin validation)
- **Status**: ✅ COMPLETE
- **Dependencies**: Task 01.02 must complete
- **Owner**: CMS integration
- **Result**: ConfigureCmsDefaults() used; Graph services registered

### Task 01.04-cms-verification
- **Description**: Validate CMS 13 deployment (admin UI, application model, database compatibility)
- **Status**: ✅ COMPLETE
- **Dependencies**: Task 01.03 must complete
- **Owner**: QA/Validation
- **Result**: Build successful (0 errors, 2 acceptable warnings); Documentation generated

---

## Stage 2: Search & Navigation → Graph Migration
**Status**: ⏳ READY TO START (Stage 1 complete)
**Gate**: Stage 1 complete ✅

### Task 02.01-search-analysis
- **Description**: Analyze Search & Navigation (EPiServer.Find) usage across codebase
- **Status**: Pending
- **Dependencies**: Stage 1 completion
- **Owner**: Search specialist
- **Note**: EPiServer.Find already removed; search controller prepared for Graph

### Task 02.02-graph-sdk-setup
- **Description**: Register Optimizely Graph services and configure C# Graph SDK
- **Status**: Pending
- **Dependencies**: Task 02.01 must complete
- **Owner**: Graph implementation
- **Note**: IGraphSearchService abstraction exists; Graph SDK packages present

### Task 02.03-search-migration
- **Description**: Migrate Search & Navigation queries to Graph async APIs
- **Status**: Pending
- **Dependencies**: Task 02.02 must complete
- **Owner**: Search implementation
- **Note**: Placeholder implementation ready; awaiting Graph SDK

### Task 02.04-search-validation
- **Description**: Validate search behavior (filtering, paging, sorting preserved)
- **Status**: Pending
- **Dependencies**: Task 02.03 must complete
- **Owner**: QA/Search

---

## Stage 3: Final Verification & Audit
**Status**: Blocked (waiting for Stage 2)  
**Gate**: Stage 2 must complete successfully

### Task 03.01-final-audit
- **Description**: Final audit and compliance check
- **Status**: Pending
- **Dependencies**: Stage 2 completion
- **Owner**: Architecture/Lead

