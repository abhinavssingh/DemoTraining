#!/usr/bin/env bash
set -euo pipefail

# Start SQL Server in the foreground, but kick off the import in the background.
# If the import fails, keep SQL running and show the error in logs.
(
  /usr/local/bin/SetupDatabases.sh || echo "SetupDatabases.sh failed; check logs above."
) &

exec /opt/mssql/bin/sqlservr