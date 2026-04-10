# syntax=docker/dockerfile:1
# RegiLattice CLI — Windows Nano Server container.
#
# The CI workflow (packages.yml) downloads the self-contained CLI binary from
# the GitHub Release and places it as `regilattice.exe` in the build context
# before running `docker build`.
#
# Usage:
#   docker run --rm ghcr.io/rajwanyair/regilattice:latest --list
#   docker run --rm ghcr.io/rajwanyair/regilattice:latest --stats

FROM mcr.microsoft.com/windows/nanoserver:ltsc2022

LABEL org.opencontainers.image.source="https://github.com/RajwanYair/RegiLattice"
LABEL org.opencontainers.image.description="RegiLattice CLI — Windows registry tweak toolkit (7,568 tweaks across 127 categories)"
LABEL org.opencontainers.image.licenses="MIT"
LABEL org.opencontainers.image.vendor="Yair Rajwan"

# Self-contained .NET 10 CLI binary — no .NET runtime installation required.
COPY regilattice.exe C:/RegiLattice/regilattice.exe

ENTRYPOINT ["C:\\RegiLattice\\regilattice.exe"]
