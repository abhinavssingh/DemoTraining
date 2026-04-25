@modernize use the skill `modernize-optimizely-cms13`.

Perform a full upgrade of this repository from Optimizely CMS 12 to CMS 13.

Follow all phases in order:
- Analysis
- Validation
- Solution / platform upgrade
- CMS dependency upgrade
- Hosting and initialization updates
- Content APIs, controllers, features
- Configuration updates
- Build validation
- Documentation

Strict rules:
- Do not refactor business or domain logic
- Do not upgrade CMS packages before the platform upgrade succeeds
- Prefer TODO comments over assumptions
- Explain all CMS 13 breaking changes inline
- Stop execution and report blockers if a mandatory phase fails

Produce the following artifacts:
- CMS13-Upgrade.md
- CMS13-BreakingChanges.md
- CMS13-PostUpgradeChecklist.md