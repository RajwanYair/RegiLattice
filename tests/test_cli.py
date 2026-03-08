"""Tests for regilattice.cli — argument parsing and action dispatch."""

from __future__ import annotations

import json
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

from regilattice.cli import _build_parser, _confirm, _run_action, main
from regilattice.tweaks import TweakResult

# ── _confirm helper ──────────────────────────────────────────────────────────


class TestConfirm:
    def test_yes(self) -> None:
        with patch("builtins.input", return_value="y"):
            assert _confirm("Do it?") is True

    def test_no(self) -> None:
        with patch("builtins.input", return_value="n"):
            assert _confirm("Do it?") is False

    def test_eof(self) -> None:
        with patch("builtins.input", side_effect=EOFError):
            assert _confirm("Do it?") is False

    def test_keyboard_interrupt(self) -> None:
        with patch("builtins.input", side_effect=KeyboardInterrupt):
            assert _confirm("Do it?") is False


# ── Parser ───────────────────────────────────────────────────────────────────


class TestBuildParser:
    def test_parser_has_expected_args(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--list"])
        assert ns.list is True

    def test_parser_mode_and_tweak(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["apply", "some-id"])
        assert ns.mode == "apply"
        assert ns.tweak == "some-id"


# ── --list flag ──────────────────────────────────────────────────────────────


class TestListFlag:
    def test_list_prints_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "explorer-show-file-extensions" in out or "telem-disable" in out

    def test_list_shows_header(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "ID" in out
        assert "Category" in out


# ── --version ────────────────────────────────────────────────────────────────


class TestVersion:
    def test_version_flag(self, capsys: pytest.CaptureFixture[str]) -> None:
        with pytest.raises(SystemExit) as exc_info:
            main(["--version"])
        assert exc_info.value.code == 0


# ── User abort ───────────────────────────────────────────────────────────────


class TestAbortByUser:
    def test_abort_returns_1(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.assert_not_corporate"), patch("builtins.input", return_value="n"):
            rc = main(["apply", "explorer-show-file-extensions"])
        assert rc == 1
        assert "Aborted" in capsys.readouterr().out


# ── --snapshot / --restore / --snapshot-diff ─────────────────────────────────


class TestSnapshotFlag:
    def test_snapshot_creates_file(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        path = tmp_path / "snap.json"
        with patch("regilattice.tweaks.save_snapshot") as mock_save:

            def _save(p: Path) -> None:
                p.parent.mkdir(parents=True, exist_ok=True)
                p.write_text(json.dumps({"test": "applied"}))

            mock_save.side_effect = _save
            rc = main(["--snapshot", str(path)])
        assert rc == 0
        assert path.exists()

    def test_restore_calls_restore_snapshot(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        snap = tmp_path / "snap.json"
        snap.write_text(json.dumps({"fake-id": "applied"}))
        with patch("regilattice.tweaks.restore_snapshot", return_value={"fake-id": "applied"}) as mock_restore:
            rc = main(["--restore", str(snap)])
        assert rc == 0
        mock_restore.assert_called_once()

    def test_snapshot_diff_no_differences(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied"}))
        b.write_text(json.dumps({"x": "applied"}))
        with patch("regilattice.tweaks.diff_snapshots", return_value={}):
            rc = main(["--snapshot-diff", str(a), str(b)])
        assert rc == 0
        assert "No differences" in capsys.readouterr().out

    def test_snapshot_diff_with_differences(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied"}))
        b.write_text(json.dumps({"x": "not applied"}))
        with patch("regilattice.tweaks.diff_snapshots", return_value={"x": ("applied", "not applied")}):
            rc = main(["--snapshot-diff", str(a), str(b)])
        assert rc == 0
        out = capsys.readouterr().out
        assert "1 tweak(s) differ" in out

    def test_snapshot_diff_html_output(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        html_out = tmp_path / "diff.html"
        a.write_text(json.dumps({"x": "applied"}))
        b.write_text(json.dumps({"x": "not applied"}))
        diffs = {"x": ("applied", "not applied")}
        with patch("regilattice.tweaks.diff_snapshots", return_value=diffs):
            rc = main(["--snapshot-diff", str(a), str(b), "--html", str(html_out)])
        assert rc == 0
        assert html_out.exists()
        content = html_out.read_text(encoding="utf-8")
        assert "RegiLattice" in content
        assert "applied" in content
        assert "<table>" in content


# ── Snapshot diff helpers ────────────────────────────────────────────────────


class TestDiffHelpers:
    def test_state_colour_applied(self) -> None:
        from regilattice.cli import _state_colour

        result = _state_colour("applied")
        assert "applied" in result
        assert "\033[32m" in result  # green

    def test_state_colour_not_applied(self) -> None:
        from regilattice.cli import _state_colour

        result = _state_colour("not applied")
        assert "not applied" in result
        assert "\033[31m" in result  # red

    def test_state_colour_absent(self) -> None:
        from regilattice.cli import _state_colour

        result = _state_colour("(absent)")
        assert "(absent)" in result
        assert "\033[2m" in result  # dim

    def test_state_colour_unknown(self) -> None:
        from regilattice.cli import _state_colour

        result = _state_colour("unknown")
        assert "unknown" in result
        assert "\033[33m" in result  # yellow

    def test_print_diff_coloured(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.cli import _print_diff_coloured

        diffs = {
            "tweak-a": ("applied", "not applied"),
            "tweak-b": ("(absent)", "applied"),
            "tweak-c": ("applied", "(absent)"),
        }
        _print_diff_coloured(diffs, "before.json", "after.json")
        out = capsys.readouterr().out
        assert "3 tweak(s) differ" in out
        assert "tweak-a" in out
        assert "+1 added" in out
        assert "-1 removed" in out
        assert "~1 changed" in out

    def test_write_diff_html(self, tmp_path: Path) -> None:
        from regilattice.cli import _write_diff_html

        dest = tmp_path / "report.html"
        diffs = {"tweak-x": ("applied", "not applied")}
        _write_diff_html(diffs, dest, "snap_a.json", "snap_b.json")
        assert dest.exists()
        content = dest.read_text(encoding="utf-8")
        assert "<!DOCTYPE html>" in content
        assert "tweak-x" in content
        assert "snap_a.json" in content
        assert "snap_b.json" in content
        assert "1 difference(s)" in content

    def test_write_diff_html_escapes_special_chars(self, tmp_path: Path) -> None:
        from regilattice.cli import _write_diff_html

        dest = tmp_path / "report.html"
        diffs = {"tweak-<script>": ("<b>applied</b>", "not applied")}
        _write_diff_html(diffs, dest, "a.json", "b.json")
        content = dest.read_text(encoding="utf-8")
        assert "<script>" not in content
        assert "&lt;script&gt;" in content


# ── --dry-run ────────────────────────────────────────────────────────────────


class TestDryRun:
    def test_dry_run_sets_flag(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--dry-run", "--list"])
        assert rc == 0
        assert "Dry-run" in capsys.readouterr().out


# ── --profile ────────────────────────────────────────────────────────────────


class TestProfile:
    def test_profile_applies(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_for_profile", return_value=[MagicMock()]),
            patch("regilattice.cli.apply_profile", return_value={"t1": TweakResult.APPLIED}),
            patch("builtins.input", return_value="y"),
        ):
            rc = main(["--profile", "gaming"])
        assert rc == 0

    def test_profile_abort(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_for_profile", return_value=[MagicMock()]),
            patch("builtins.input", return_value="n"),
        ):
            rc = main(["--profile", "gaming"])
        assert rc == 1

    def test_profile_corp_blocked(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.corpguard import CorporateNetworkError

        with patch("regilattice.cli.assert_not_corporate", side_effect=CorporateNetworkError("blocked")):
            rc = main(["--profile", "gaming"])
        assert rc == 6


# ── --gui ────────────────────────────────────────────────────────────────────


class TestGuiFlag:
    def test_gui_launches(self) -> None:
        with patch("regilattice.gui.launch") as mock_launch:
            rc = main(["--gui"])
        assert rc == 0
        mock_launch.assert_called_once()


# ── status mode ──────────────────────────────────────────────────────────────


class TestStatusMode:
    def test_status_known_tweak(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["status", first.id])
        assert rc == 0

    def test_status_unknown_tweak(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["status", "nonexistent-tweak-xyz-999"])
        assert rc == 2
        assert "Unknown" in capsys.readouterr().out


# ── _run_action ──────────────────────────────────────────────────────────────


class TestRunAction:
    def test_corp_blocked(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.corpguard import CorporateNetworkError

        with patch("regilattice.cli.assert_not_corporate", side_effect=CorporateNetworkError("blocked")):
            rc = _run_action("apply", "some-id", assume_yes=True)
        assert rc == 6

    def test_unknown_tweak(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.assert_not_corporate"):
            rc = _run_action("apply", "nonexistent-zzz-999", assume_yes=True)
        assert rc == 2

    def test_batch_all_apply(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.apply_all", return_value={"t": TweakResult.APPLIED}),
        ):
            rc = _run_action("apply", "all", assume_yes=True)
        assert rc == 0

    def test_batch_all_abort(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("builtins.input", return_value="n"),
        ):
            rc = _run_action("apply", "all", assume_yes=False)
        assert rc == 1

    def test_single_apply_success(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch.object(type(first), "apply_fn", new_callable=lambda: property(lambda self: MagicMock())),
        ):
            rc = _run_action("apply", first.id, assume_yes=True)
        assert rc == 0

    def test_single_admin_error(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.registry import AdminRequirementError
        from regilattice.tweaks import all_tweaks

        all_tweaks()  # ensure tweaks loaded
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.get_tweak") as mock_get,
            patch("builtins.input", return_value="y"),
        ):
            td = MagicMock()
            td.apply_fn.side_effect = AdminRequirementError("need admin")
            td.label = "Test"
            mock_get.return_value = td
            rc = _run_action("apply", "fake-id", assume_yes=True)
        assert rc == 5


# ── --config ─────────────────────────────────────────────────────────────────


class TestConfigFlag:
    def test_config_file_loaded(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        cfg_path = tmp_path / "test.toml"
        cfg_path.write_text("[general]\nforce_corp = true\n")
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--config", str(cfg_path), "--list"])
        assert rc == 0


# ── --search ─────────────────────────────────────────────────────────────────


class TestSearchFlag:
    def test_search_finds_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--search", "explorer"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "tweak(s) found" in out

    def test_search_no_results(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.search_tweaks", return_value=[]):
            rc = main(["--search", "xyznonexistent999"])
        assert rc == 0
        assert "No tweaks matching" in capsys.readouterr().out


# ── --category ───────────────────────────────────────────────────────────────


class TestCategoryFlag:
    def test_category_apply(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
            patch("builtins.input", return_value="y"),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 0

    def test_category_unknown(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "all", "--category", "NON_EXISTENT_CAT_999"])
        assert rc == 2
        assert "Unknown category" in capsys.readouterr().out

    def test_category_abort(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
            patch("builtins.input", return_value="n"),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 1

    def test_category_corp_blocked(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.corpguard import CorporateNetworkError

        with (
            patch("regilattice.cli.assert_not_corporate", side_effect=CorporateNetworkError("blocked")),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 6


# ── --export-json ────────────────────────────────────────────────────────────


class TestExportJson:
    def test_export_writes_file(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        out_path = tmp_path / "export.json"
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--export-json", str(out_path)])
        assert rc == 0
        assert out_path.exists()
        data = json.loads(out_path.read_text(encoding="utf-8"))
        assert isinstance(data, list)
        assert len(data) > 0
        assert "id" in data[0]
        assert "category" in data[0]


# ── --check-deps ─────────────────────────────────────────────────────────────


class TestCheckDeps:
    def test_check_deps_all_present(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--check-deps"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "pytest" in out


# ── --list-profiles ──────────────────────────────────────────────────────────


class TestListProfiles:
    def test_list_profiles(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list-profiles"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "business" in out
        assert "gaming" in out
        assert "privacy" in out
        assert "Profile" in out


# ── --categories ─────────────────────────────────────────────────────────────


class TestCategoriesFlag:
    def test_categories_lists_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--categories"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Explorer" in out
        assert "categories" in out


# ── --tags ───────────────────────────────────────────────────────────────────


class TestTagsFlag:
    def test_tags_lists_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--tags"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Tag" in out
        assert "unique tags" in out
        # Tags always include common ones like "explorer" or "privacy"
        assert "explorer" in out or "privacy" in out


# ── --import-json ────────────────────────────────────────────────────────────


class TestImportJson:
    def test_import_applies_tweaks(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        jfile = tmp_path / "ids.json"
        jfile.write_text(json.dumps([first.id]), encoding="utf-8")
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch.object(type(first), "apply_fn", new_callable=lambda: property(lambda self: MagicMock())),
        ):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 0

    def test_import_dict_format(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        jfile = tmp_path / "ids.json"
        jfile.write_text(json.dumps({"tweaks": [first.id]}), encoding="utf-8")
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch.object(type(first), "apply_fn", new_callable=lambda: property(lambda self: MagicMock())),
        ):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 0

    def test_import_unknown_ids(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        jfile = tmp_path / "ids.json"
        jfile.write_text(json.dumps(["nonexistent-zzz-999"]), encoding="utf-8")
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 2
        assert "No valid tweaks" in capsys.readouterr().out

    def test_import_bad_json(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        jfile = tmp_path / "bad.json"
        jfile.write_text("{bad", encoding="utf-8")
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 3

    def test_import_abort(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        jfile = tmp_path / "ids.json"
        jfile.write_text(json.dumps([first.id]), encoding="utf-8")
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("builtins.input", return_value="n"),
        ):
            rc = main(["apply", "dummy", "--import-json", str(jfile)])
        assert rc == 1


# ── --export-reg ─────────────────────────────────────────────────────────────


class TestExportReg:
    def test_non_windows_returns_4(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.is_windows", return_value=False):
            rc = main(["--export-reg", "out.reg"])
        assert rc == 4
        assert "requires Windows" in capsys.readouterr().out

    def test_exports_file_on_windows(self, tmp_path: Path, capsys: pytest.CaptureFixture[str]) -> None:
        """Mock winreg to verify .reg file generation."""
        dest = tmp_path / "export.reg"
        mock_handle = MagicMock()
        mock_handle.__enter__ = MagicMock(return_value=mock_handle)
        mock_handle.__exit__ = MagicMock(return_value=False)

        def fake_enum(handle: object, idx: int) -> tuple[str, int, int]:
            if idx == 0:
                return ("TestVal", 1, 4)  # REG_DWORD = 4
            raise OSError("no more")

        with (
            patch("regilattice.cli.is_windows", return_value=True),
            patch("regilattice.cli._split_root_for_reg", return_value=(0x80000001, "Software\\Test")),
            patch("regilattice.cli._format_reg_value", return_value="dword:00000001"),
        ):
            import regilattice.cli as _cli_mod

            _wr_mock = MagicMock()
            _wr_mock.OpenKey.return_value = mock_handle
            _wr_mock.EnumValue = fake_enum
            _wr_mock.KEY_READ = 0x20019

            with patch.dict("sys.modules", {"winreg": _wr_mock}):
                # Directly call _export_reg to avoid parser issues with winreg import
                rc = _cli_mod._export_reg(dest)

        assert rc == 0
        assert dest.exists()
        content = dest.read_text(encoding="utf-16-le")
        assert "Windows Registry Editor Version 5.00" in content

    def test_parser_has_export_reg(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--export-reg", "test.reg"])
        assert ns.export_reg == "test.reg"


# ── --check flag ─────────────────────────────────────────────────────────────


class TestCheckFlag:
    def test_check_shows_summary(self, capsys: pytest.CaptureFixture[str]) -> None:
        fake_map = {
            "tweak-a": TweakResult.APPLIED,
            "tweak-b": TweakResult.NOT_APPLIED,
            "tweak-c": TweakResult.UNKNOWN,
        }
        with patch("regilattice.tweaks.status_map", return_value=fake_map):
            rc = main(["--check"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Applied" in out
        assert "Default" in out
        assert "Unknown" in out
        assert "1" in out  # at least 1 applied

    def test_check_lists_applied_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        td = MagicMock()
        td.label = "Test Tweak"
        fake_map = {"tweak-applied": TweakResult.APPLIED}
        with (
            patch("regilattice.tweaks.status_map", return_value=fake_map),
            patch("regilattice.cli.get_tweak", return_value=td),
        ):
            rc = main(["--check"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "tweak-applied" in out
        assert "Test Tweak" in out

    def test_parser_has_check(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--check"])
        assert ns.check is True


# ── --diff flag ──────────────────────────────────────────────────────────────


class TestDiffFlag:
    def test_diff_no_changes(self, capsys: pytest.CaptureFixture[str]) -> None:
        td = MagicMock()
        td.id = "tweak-a"
        fake_map = {"tweak-a": TweakResult.APPLIED}
        with (
            patch("regilattice.cli.tweaks_for_profile", return_value=[td]),
            patch("regilattice.tweaks.status_map", return_value=fake_map),
        ):
            rc = main(["--diff", "minimal"])
        assert rc == 0
        assert "no changes needed" in capsys.readouterr().out

    def test_diff_shows_delta(self, capsys: pytest.CaptureFixture[str]) -> None:
        td = MagicMock()
        td.id = "tweak-a"
        td.label = "Tweak A"
        fake_map = {"tweak-a": TweakResult.NOT_APPLIED, "tweak-extra": TweakResult.APPLIED}
        with (
            patch("regilattice.cli.tweaks_for_profile", return_value=[td]),
            patch("regilattice.tweaks.status_map", return_value=fake_map),
            patch("regilattice.cli.get_tweak", return_value=td),
        ):
            rc = main(["--diff", "minimal"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "APPLY" in out
        assert "tweak-a" in out
        assert "tweak-extra" in out

    def test_parser_has_diff(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--diff", "gaming"])
        assert ns.diff == "gaming"


# ── Interactive menu fallback ────────────────────────────────────────────────


class TestInteractiveMenuFallback:
    def test_non_windows_returns_4(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.is_windows", return_value=False):
            rc = main([])
        assert rc == 4


# ── C6 enhancements: --validate, --stats, --output json, --list --category ───


class TestValidateFlag:
    """Tests for --validate non-destructive consistency check."""

    def test_validate_clean_returns_0(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--validate"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "All" in out or "valid" in out.lower() or "OK" in out

    def test_validate_output_contains_tweak_count(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--validate"])
        assert rc == 0
        out = capsys.readouterr().out
        # Should mention a numeric count
        assert any(char.isdigit() for char in out)

    def test_parser_has_validate(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--validate"])
        assert ns.validate is True


class TestStatsFlag:
    """Tests for --stats breakdown."""

    def test_stats_returns_0(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--stats"])
        assert rc == 0
        out = capsys.readouterr().out
        assert len(out) > 0

    def test_stats_shows_categories(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--stats"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "categor" in out.lower()

    def test_stats_shows_total(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--stats"])
        assert rc == 0
        out = capsys.readouterr().out
        # total tweak count somewhere
        assert any(char.isdigit() for char in out)

    def test_parser_has_stats(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--stats"])
        assert ns.stats is True


class TestOutputJson:
    """Tests for --output json flag on --list and --search."""

    def test_list_output_json(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list", "--output", "json"])
        assert rc == 0
        out = capsys.readouterr().out.strip()
        data = json.loads(out)
        assert isinstance(data, list)
        assert len(data) > 0
        assert "id" in data[0]

    def test_list_json_has_expected_fields(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            main(["--list", "--output", "json"])
        out = capsys.readouterr().out.strip()
        data = json.loads(out)
        first = data[0]
        assert "id" in first
        assert "label" in first
        assert "category" in first

    def test_search_output_json(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--search", "explorer", "--output", "json"])
        assert rc == 0
        out = capsys.readouterr().out.strip()
        data = json.loads(out)
        assert isinstance(data, list)
        assert all("id" in item for item in data)

    def test_categories_output_json(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--categories", "--output", "json"])
        assert rc == 0
        out = capsys.readouterr().out.strip()
        data = json.loads(out)
        # --categories outputs {"Category Name": count}
        assert isinstance(data, dict)
        assert "Explorer" in data

    def test_parser_has_output_flag(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--list", "--output", "json"])
        assert ns.output == "json"

    def test_parser_output_default_is_table(self) -> None:
        parser = _build_parser()
        ns = parser.parse_args(["--list"])
        assert ns.output == "table"


class TestListCategoryFilter:
    """Tests for --list --category filtering."""

    def test_list_category_filters_results(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list", "--category", "Explorer"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Explorer" in out

    def test_list_category_unknown_returns_2(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--category", "ZZZ_NONEXISTENT_CATEGORY_999"])
        assert rc == 2
        assert "No tweaks found" in capsys.readouterr().out

    def test_list_category_json_output(self, capsys: pytest.CaptureFixture[str]) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list", "--category", "Explorer", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        assert all(item["category"] == "Explorer" for item in data)

    def test_list_categories_flag(self, capsys: pytest.CaptureFixture[str]) -> None:
        """--list-categories should list category names."""
        rc = main(["--list-categories"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Explorer" in out

    def test_list_categories_json(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list-categories", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        # --list-categories shares the same code path as --categories: dict output
        assert isinstance(data, dict)
        assert "Explorer" in data


# ── --scope / --min-build / --corp-safe / --needs-admin filter tests ─────────


class TestListScopeFilter:
    """Tests for --scope filter on --list and --search."""

    def test_scope_user_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--scope", "user"])
        assert rc == 0

    def test_scope_machine_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--scope", "machine"])
        assert rc == 0

    def test_scope_both_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--scope", "both"])
        assert rc == 0

    def test_scope_invalid_rejected(self) -> None:
        """argparse should reject invalid --scope values."""
        with pytest.raises(SystemExit) as exc_info:
            _build_parser().parse_args(["--list", "--scope", "invalid"])
        assert exc_info.value.code != 0

    def test_scope_user_json_only_user_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks, tweak_scope

        rc = main(["--list", "--scope", "user", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        all_user_ids = {td.id for td in all_tweaks() if tweak_scope(td) == "user"}
        for item in data:
            assert item["id"] in all_user_ids

    def test_scope_machine_json_only_machine_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks, tweak_scope

        rc = main(["--list", "--scope", "machine", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        all_machine_ids = {td.id for td in all_tweaks() if tweak_scope(td) == "machine"}
        for item in data:
            assert item["id"] in all_machine_ids

    def test_search_with_scope_filter(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--search", "disable", "--scope", "machine"])
        assert rc == 0


class TestListMinBuildFilter:
    """Tests for --min-build filter on --list."""

    def test_min_build_zero_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--min-build", "0"])
        assert rc == 0

    def test_min_build_large_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        """High build number should produce an empty (or small) list, not an error."""
        rc = main(["--list", "--min-build", "99999"])
        assert rc == 0

    def test_min_build_filters_tweaks(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.tweaks import all_tweaks

        build = 22000
        rc = main(["--list", "--min-build", str(build), "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        expected_ids = {td.id for td in all_tweaks() if td.min_build <= build}
        for item in data:
            assert item["id"] in expected_ids

    def test_min_build_with_scope(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--scope", "user", "--min-build", "22000"])
        assert rc == 0


class TestListCorpAdminFilter:
    """Tests for --corp-safe and --needs-admin filter flags."""

    def test_corp_safe_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--corp-safe"])
        assert rc == 0

    def test_needs_admin_exits_zero(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--needs-admin"])
        assert rc == 0

    def test_corp_safe_json_all_corp_safe(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--corp-safe", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        assert all(item["corp_safe"] for item in data)

    def test_needs_admin_json_all_need_admin(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--list", "--needs-admin", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        assert all(item["needs_admin"] for item in data)

    def test_corp_safe_and_needs_admin_combined(self, capsys: pytest.CaptureFixture[str]) -> None:
        """Combined flags should intersect: corp_safe=True AND needs_admin=True."""
        rc = main(["--list", "--corp-safe", "--needs-admin", "--output", "json"])
        assert rc == 0
        data = json.loads(capsys.readouterr().out.strip())
        for item in data:
            assert item["corp_safe"]
            assert item["needs_admin"]

    def test_search_with_corp_safe_filter(self, capsys: pytest.CaptureFixture[str]) -> None:
        rc = main(["--search", "network", "--corp-safe"])
        assert rc == 0
