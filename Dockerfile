# Atlas Reactor game-server image. Published to ghcr.io/zheneq/hc.
#
# Two stages:
#   1. build   — compile Assembly-CSharp.dll (Server Release) with Mono's msbuild
#                (Roslyn) against the checked-in dependencies/ + vendored GitInfo.
#   2. runtime — FROM a PREBAKED base image that already contains the full static
#                Unity "Atlas Reactor Server" install + wine + xvfb; we drop the
#                freshly-built DLL into the install's Managed/ folder.
#
# The prebaked base (built once, out of band — see below) MUST provide:
#   - debian + wine + xvfb (with the `xvfb-run` helper) + a ready WINEPREFIX
#   - the Unity server install under $HC_HOME/Win64, i.e. $HC_HOME/Win64/AtlasReactor.exe
#     and $HC_HOME/Win64/AtlasReactor_Data/Managed/
#
# Runtime is driven by the distribution's generated compose, which appends
#   -c /data/Config/AtlasReactorServerConfig.json -e game_server_N
# as arguments (they flow through the entrypoint to AtlasReactor.exe). /data is a
# writable volume mounted by the distribution; config arrives via that -c path.

# Declared before the first FROM so it is a *global* build arg and can be used
# in the runtime FROM below (an ARG declared after a FROM is stage-scoped and
# would expand to blank there).
ARG BASE_IMAGE=ghcr.io/zheneq/hc-base:latest

# ---- build ----
FROM mono:6.12 AS build
# Which msbuild Configuration to compile. Defaults to the server build; the
# client-release CI job overrides this with "Client Release" to emit the
# EVOS client DLL from the same source tree.
ARG CONFIG="Server Release"
WORKDIR /src
# GitInfo stamps the assembly version by shelling out to `git` at build time,
# but mono:6.12 ships without it. mono:6.12 is Debian buster (EOL), whose apt
# mirrors moved to archive.debian.org, so repoint sources (and skip the expired
# Valid-Until check) before installing git. This is a throwaway build stage, so
# the EOL base is not a runtime concern. Done before COPY so it stays cached.
RUN sed -i -e 's|http://deb.debian.org/debian|http://archive.debian.org/debian|g' \
           -e '/buster-updates/d' \
           /etc/apt/sources.list \
    && apt-get -o Acquire::Check-Valid-Until=false update \
    && apt-get install -y --no-install-recommends git \
    && rm -rf /var/lib/apt/lists/*
# Full tree incl. .git — GitInfo derives the assembly version from git tags.
COPY . .
# OutputPath is overridden on the CLI. dependencies/ (HintPath) + the vendored
# packages/GitInfo satisfy all references, so no nuget restore is needed.
RUN msbuild Assembly-CSharp/Assembly-CSharp.csproj \
        /t:Rebuild \
        /p:Configuration="$CONFIG" \
        /p:Platform=AnyCPU \
        /p:OutputPath=/out/

# ---- export ----
# Minimal stage holding ONLY the compiled DLL, so the CI job can export it to
# the local filesystem (buildx `--output type=local`) without dragging out the
# whole mono build image. Not part of the runtime image.
FROM scratch AS export
COPY --from=build /out/Assembly-CSharp.dll /Assembly-CSharp.dll

# ---- runtime ----
FROM ${BASE_IMAGE} AS runtime

# Root of the base image install; the Unity server lives under $HC_HOME/Win64
# (contains AtlasReactor.exe and AtlasReactor_Data/). Overridable to match
# however the base image is laid out.
ARG HC_HOME=/opt/hc
ENV HC_HOME=${HC_HOME} \
    WINEDEBUG=-all

# Only the game assembly is overlaid; every other DLL comes from the base install.
COPY --from=build /out/Assembly-CSharp.dll ${HC_HOME}/Win64/AtlasReactor_Data/Managed/Assembly-CSharp.dll
# Baked default config. ServerBootstrap.HandleConfig loads CommonServerConfig
# from a FIXED install-relative path (Application.dataPath/../../Config =>
# $HC_HOME/Config) and REQUIRES a base file named `json` there, evaluated BEFORE
# -c is parsed. So bake Config/ (which includes that `json` base) to
# $HC_HOME/Config, not Managed/Config (which nothing reads). Per-instance values
# still arrive via -c /data/Config (mounted by the distribution).
COPY Config/ ${HC_HOME}/Config/

COPY docker-entrypoint.sh /usr/local/bin/docker-entrypoint.sh
RUN chmod +x /usr/local/bin/docker-entrypoint.sh

ENTRYPOINT ["/usr/local/bin/docker-entrypoint.sh"]