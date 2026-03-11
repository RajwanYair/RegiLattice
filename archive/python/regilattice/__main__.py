"""RegiLattice package entry point — delegates to cli.main()."""

from .cli import main

if __name__ == "__main__":  # pragma: no cover
    raise SystemExit(main())
