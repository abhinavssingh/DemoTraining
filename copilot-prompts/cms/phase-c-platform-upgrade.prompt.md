@modernize upgrade the solution platform for Optimizely CMS 13.

Tasks:
- Plan the solution upgrade path
- Upgrade TargetFramework as required by CMS 13
- Update global.json if present
- Update SDK constraints if needed
- Validate the build after platform upgrade

Rules:
- Do not upgrade Optimizely CMS packages yet
- Stop and report if the build does not succeed
- Generate Platform-Upgrade.md summary