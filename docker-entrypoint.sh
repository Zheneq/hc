#!/usr/bin/env bash
# Launch the Unity game server under a virtual X display via wine. All arguments
# (e.g. -c <config> -e <env>, supplied by the distribution's generated compose)
# are forwarded to AtlasReactor.exe.
set -euo pipefail

LOG_FILE=/tmp/atlasreactor-server.log
: > "$LOG_FILE"
tail -n +1 -F "$LOG_FILE" &

exec xvfb-run -a wine "${HC_HOME}/Win64/AtlasReactor.exe" \
    -nographics -batchmode -logFile 'Z:\tmp\atlasreactor-server.log' "$@"