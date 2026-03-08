#!/usr/bin/env bash
set -euo pipefail

CONN_STR="${ConnectionStrings__EPiServerDB:-}"
if [ -z "$CONN_STR" ]; then
  echo "ConnectionStrings__EPiServerDB not set"
  exit 1
fi

echo "Waiting for SQL endpoint to be reachable..."
# crude wait based on TCP port; since web uses depends_on+healthcheck, this is usually unnecessary
for i in {1..60}; do
  nc -zv sql 1433 && break || true
  echo "  ...still waiting ($i/60)"
  sleep 2
done

echo "Starting site..."
exec dotnet DemoTraining.Web.dll