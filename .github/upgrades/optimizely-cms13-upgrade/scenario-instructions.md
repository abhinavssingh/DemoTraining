# Optimizely CMS 12 → CMS 13 Orchestrated Upgrade

## Scenario Overview
Orchestrated modernization of an Optimizely CMS 12 solution across three mandatory stages:
1. **CMS 13 Upgrade** (platform runtime + CMS packages)
2. **Search & Navigation → Graph Migration** (EPiServer.Find → Optimizely Graph)
3. **Final Verification & Audit**

Each stage must complete successfully before the next begins.

## Preferences
- **Flow Mode**: Guided
- **Source Branch**: cms-12-to-13-upgrade
- **Working Branch**: cms-12-to-13-upgrade (current)
- **Commit Strategy**: After Each Task

## Source Control
- **Source Branch**: cms-12-to-13-upgrade
- **Working Branch**: cms-12-to-13-upgrade
- **Commit Strategy**: After Each Task

## Key Constraints
- Repository must build successfully before starting each stage
- CMS upgrade must complete before search migration can begin
- Graph migration is mandatory for CMS 13 compatibility
- All CMS admin and application validation must pass
- Documentation artifacts are required for compliance and audit

## Orchestration Status
- Stage 1 (CMS 13 Upgrade): *Pending*
- Stage 2 (Search & Navigation → Graph): *Blocked until Stage 1 completes*
- Stage 3 (Final Verification): *Blocked until Stage 2 completes*
