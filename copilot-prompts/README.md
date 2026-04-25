# Copilot Modernization Prompts – Optimizely CMS 13 & Graph

This repository contains **GitHub Copilot @modernize prompts** for safely upgrading
an Optimizely CMS 12 solution to **CMS 13** and migrating
**Optimizely Search & Navigation (EPiServer.Find)** to **Optimizely Graph**.

The prompts are designed to:
- Enforce the correct upgrade order
- Minimize risk
- Preserve business logic
- Produce auditable upgrade artifacts

---

## 📁 Folder Structure

```
copilot-prompts/
cms/
full-upgrade.prompt.md
phase-a-analysis.prompt.md
phase-b-validation.prompt.md
phase-c-platform-upgrade.prompt.md
phase-d-cms-packages.prompt.md
phase-e-to-k-code.prompt.md
graph/
full-migration.prompt.md
phase-a-search-analysis.prompt.md
phase-b-packages.prompt.md
phase-c-code-rewrite.prompt.md
phase-d-config.prompt.md
phase-e-cleanup-and-docs.prompt.md
```
---

## 🚀 Quick Start (Recommended)

### Full CMS Upgrade
```text
@modernize run copilot-prompts/cms/full-upgrade.prompt.md
```
### Full Search → Graph Migration
```
@modernize run copilot-prompts/graph/full-migration.prompt.md
```
---
### 🛠️ Phase-by-Phase Execution
Example: Platform Upgrade First
```
@modernize run copilot-prompts/cms13/phase-c-platform-upgrade.prompt.md
```
Example: CMS Package Upgrade
```
@modernize run copilot-prompts/cms13/phase-d-cms-packages.prompt.md
```
Example: Search Query Rewrite Only
```
@modernize run copilot-prompts/graph/phase-c-code-rewrite.prompt.md
```
---
## 🛑 Important Rules

- Do not skip Phase C (Platform Upgrade) CMS 13 requires a higher .NET runtime than CMS 12.
- Search migration is mandatory.
- EPiServer.Find must be replaced with Optimizely Graph.
- Business logic is preserved Refactors are limited strictly to compatibility changes.

---

## 📄 Generated Artifacts
After a full upgrade, expect:

- CMS13-Upgrade.md
- CMS13-BreakingChanges.md
- CMS13-PostUpgradeChecklist.md
- Graph-Migration.md
- Graph-Manual-Validation.md