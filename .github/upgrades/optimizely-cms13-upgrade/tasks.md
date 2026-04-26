# Orchestrated CMS 13 Upgrade — Task Hierarchy

## Stage 1: CMS 13 Upgrade (Platform + CMS)
**Status**: Not started  
**Gate**: Must complete before Stage 2 begins

### Task 01.01-platform-runtime-upgrade
- **Description**: Upgrade .NET runtime from current version to .NET 10
- **Status**: Pending
- **Dependencies**: None
- **Owner**: Core infrastructure

### Task 01.02-cms-packages-upgrade
- **Description**: Upgrade all EPiServer.* and Optimizely.* CMS packages to v13
- **Status**: Pending
- **Dependencies**: Task 01.01 must complete
- **Owner**: CMS platform

### Task 01.03-cms-api-migration
- **Description**: Migrate breaking API changes (service registration, Applications model, admin validation)
- **Status**: Pending
- **Dependencies**: Task 01.02 must complete
- **Owner**: CMS integration

### Task 01.04-cms-verification
- **Description**: Validate CMS 13 deployment (admin UI, application model, database compatibility)
- **Status**: Pending
- **Dependencies**: Task 01.03 must complete
- **Owner**: QA/Validation

---

## Stage 2: Search & Navigation → Graph Migration
**Status**: Blocked (waiting for Stage 1)  
**Gate**: Stage 1 must complete successfully

### Task 02.01-search-analysis
- **Description**: Analyze Search & Navigation (EPiServer.Find) usage across codebase
- **Status**: Pending
- **Dependencies**: Stage 1 completion
- **Owner**: Search specialist

### Task 02.02-graph-sdk-setup
- **Description**: Register Optimizely Graph services and configure C# Graph SDK
- **Status**: Pending
- **Dependencies**: Task 02.01 must complete
- **Owner**: Graph implementation

### Task 02.03-search-migration
- **Description**: Migrate Search & Navigation queries to Graph async APIs
- **Status**: Pending
- **Dependencies**: Task 02.02 must complete
- **Owner**: Search implementation

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

