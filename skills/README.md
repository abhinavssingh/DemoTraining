# Orchestrated Optimizely CMS 12 → CMS 13 Upgrade (with Graph Search)

## Overview

This repository includes a **Copilot modernization orchestrator skill** that performs a
**safe, auditable, end‑to‑end upgrade** from **Optimizely CMS 12 to CMS 13**, including the
**mandatory migration from Optimizely Search & Navigation (EPiServer.Find) to Optimizely Graph**.

The orchestrator enforces:
- Correct phase ordering
- Gated execution
- Mandatory search migration (no feature disabling)
- Full audit and upgrade documentation

---

## Why an Orchestrator Skill?

Upgrading to CMS 13 is not a single action. It involves **two fundamentally different migrations**:

1. **CMS Platform Upgrade**
   - Runtime & solution upgrade
   - CMS package alignment
   - Hosting, initialization, and code refactoring

2. **Search Migration**
   - EPiServer.Find is **not supported** in CMS 13
   - Search must be migrated to **Optimizely Graph**
   - Requires async API and query rewrites

The orchestrator guarantees that:
- CMS upgrade completes successfully **before** search migration starts
- Search is **migrated, not disabled**
- Partial upgrades are impossible

---

## Skills Used

### 1. CMS Upgrade Skill
**Skill name**
`modernize-optimizely-cms12-to-cms13`


Purpose:
- Upgrade solution platform
- Upgrade CMS dependencies
- Refactor required CMS APIs
- Stabilize and document the upgrade

---

### 2. Search Migration Skill
**Skill name**
`modernize-optimizely-search-to-graph`


Purpose:
- Replace EPiServer.Find with Optimizely Graph
- Rewrite search queries
- Enforce async Graph APIs
- Preserve query behavior

---

## Orchestrator Skill

**Skill name**
`modernize-optimizely-cms13-orchestrator`

### What it does

✅ Runs CMS upgrade **first**  
✅ Stops immediately if CMS upgrade fails  
✅ Runs Graph search migration **only after CMS success**  
✅ Blocks completion if search is disabled or incomplete  
✅ Produces consolidated audit artifacts  

---

## Execution Stages

### Stage 1 — CMS Platform Upgrade (GATED)
- Executes `modernize-optimizely-cms12-to-cms13`
- Must compile and stabilize
- No search migration yet

### Stage 2 — Search Migration (MANDATORY)
- Executes `modernize-optimizely-search-to-graph`
- Rewrites all EPiServer.Find usage
- Ensures functional Graph‑based search

### Stage 3 — Validation & Audit
- Verifies no legacy CMS or Find packages remain
- Ensures search compiles and runs
- Generates final reports

---

## Output Artifacts

After successful orchestration, the following files are generated:

- `CMS13-Upgrade.md`  
- `CMS13-Graph-Upgrade.md`  
- `Final-Migration-Report.md`

These artifacts are required for audit and sign‑off.

---

## How to Run

### Full End‑to‑End Upgrade
```text
@modernize run end-to-end Optimizely CMS 12 to CMS 13 upgrade
using the orchestrated CMS + Graph skill.
```
CMS Upgrade Only
```text
@modernize run cms-platform-upgrade stage only
```

Resume at Search Migration
```text
@modernize resume orchestrated upgrade from search-migration stage
```
