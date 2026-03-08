#!/usr/bin/env bash
set -euo pipefail

: "${SA_PASSWORD:?Must provide SA_PASSWORD env}"
: "${DATABASE_NAME:?Must provide DATABASE_NAME env}"

SQLPACKAGE="/opt/sqlpackage/sqlpackage"
MARKER="/var/opt/mssql/.dbimported"

echo "[setup] Starting SetupDatabases.sh"
echo "[setup] DATABASE_NAME=${DATABASE_NAME}"

if [ -f "$MARKER" ]; then
  echo "[setup] Marker '$MARKER' exists; skipping import."
  exit 0
fi

echo "[setup] Waiting for SQL Server TCP 1433 to be ready..."
for i in {1..90}; do
  if nc -z localhost 1433; then
    echo "[setup] SQL Server TCP is up."
    break
  fi
  echo "[setup]  ...still waiting ($i/90)"
  sleep 2
done

echo "[setup] sqlpackage version:"
"$SQLPACKAGE" -Version || true   # should print 170.x

echo "[setup] Importing BACPAC into database '${DATABASE_NAME}'..."
if "$SQLPACKAGE" /a:Import \
  /tsn:localhost,1433 \
  /tdn:"${DATABASE_NAME}" \
  /tu:sa \
  /tp:"${SA_PASSWORD}" \
  /sf:/tmp/db/demoTraining.bacpac \
  /TargetEncryptConnection:True \
  /TargetTrustServerCertificate:True \
  /Quiet:True ; then
  echo "[setup] Import complete."
  touch "$MARKER"
else
  echo "[setup][ERROR] sqlpackage import failed. See error above."
  exit 1
fi

echo "[setup] SetupDatabases.sh finished."