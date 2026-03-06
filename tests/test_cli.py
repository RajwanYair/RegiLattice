"""Tests for regilattice.cli — argument parsing and action dispatch."""

from __future__ import annotations

import json
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
    def test_list_prints_tweaks(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "explorer-show-file-extensions" in out or "telem-disable" in out

    def test_list_shows_header(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "ID" in out
        assert "Category" in out


# ── --version ────────────────────────────────────────────────────────────────


class TestVersion:
    def test_version_flag(self, capsys) -> None:
        with pytest.raises(SystemExit) as exc_info:
            main(["--version"])
        assert exc_info.value.code == 0


# ── User abort ───────────────────────────────────────────────────────────────


class TestAbortByUser:
    def test_abort_returns_1(self, capsys) -> None:
        with patch("regilattice.cli.assert_not_corporate"), patch("builtins.input", return_value="n"):
            rc = main(["apply", "explorer-show-file-extensions"])
        assert rc == 1
        assert "Aborted" in capsys.readouterr().out


# ── --snapshot / --restore / --snapshot-diff ─────────────────────────────────


class TestSnapshotFlag:
    def test_snapshot_creates_file(self, tmp_path, capsys) -> None:
        path = tmp_path / "snap.json"
        with patch("regilattice.tweaks.save_snapshot") as mock_save:

            def _save(p):
                p.parent.mkdir(parents=True, exist_ok=True)
                p.write_text(json.dumps({"test": "applied"}))

            mock_save.side_effect = _save
            rc = main(["--snapshot", str(path)])
        assert rc == 0
        assert path.exists()

    def test_restore_calls_restore_snapshot(self, tmp_path, capsys) -> None:
        snap = tmp_path / "snap.json"
        snap.write_text(json.dumps({"fake-id": "applied"}))
        with patch("regilattice.tweaks.restore_snapshot", return_value={"fake-id": "applied"}) as mock_restore:
            rc = main(["--restore", str(snap)])
        assert rc == 0
        mock_restore.assert_called_once()

    def test_snapshot_diff_no_differences(self, tmp_path, capsys) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied"}))
        b.write_text(json.dumps({"x": "applied"}))
        with patch("regilattice.tweaks.diff_snapshots", return_value={}):
            rc = main(["--snapshot-diff", str(a), str(b)])
        assert rc == 0
        assert "No differences" in capsys.readouterr().out

    def test_snapshot_diff_with_differences(self, tmp_path, capsys) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied"}))
        b.write_text(json.dumps({"x": "not applied"}))
        with patch("regilattice.tweaks.diff_snapshots", return_value={"x": ("applied", "not applied")}):
            rc = main(["--snapshot-diff", str(a), str(b)])
        assert rc == 0
        out = capsys.readouterr().out
        assert "1 tweak(s) differ" in out


# ── --dry-run ────────────────────────────────────────────────────────────────


class TestDryRun:
    def test_dry_run_sets_flag(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--dry-run", "--list"])
        assert rc == 0
        assert "Dry-run" in capsys.readouterr().out


# ── --profile ────────────────────────────────────────────────────────────────


class TestProfile:
    def test_profile_applies(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_for_profile", return_value=[MagicMock()]),
            patch("regilattice.cli.apply_profile", return_value={"t1": TweakResult.APPLIED}),
            patch("builtins.input", return_value="y"),
        ):
            rc = main(["--profile", "gaming"])
        assert rc == 0

    def test_profile_abort(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_for_profile", return_value=[MagicMock()]),
            patch("builtins.input", return_value="n"),
        ):
            rc = main(["--profile", "gaming"])
        assert rc == 1

    def test_profile_corp_blocked(self, capsys) -> None:
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
    def test_status_known_tweak(self, capsys) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["status", first.id])
        assert rc == 0

    def test_status_unknown_tweak(self, capsys) -> None:
        rc = main(["status", "nonexistent-tweak-xyz-999"])
        assert rc == 2
        assert "Unknown" in capsys.readouterr().out


# ── _run_action ──────────────────────────────────────────────────────────────


class TestRunAction:
    def test_corp_blocked(self, capsys) -> None:
        from regilattice.corpguard import CorporateNetworkError

        with patch("regilattice.cli.assert_not_corporate", side_effect=CorporateNetworkError("blocked")):
            rc = _run_action("apply", "some-id", assume_yes=True)
        assert rc == 6

    def test_unknown_tweak(self, capsys) -> None:
        with patch("regilattice.cli.assert_not_corporate"):
            rc = _run_action("apply", "nonexistent-zzz-999", assume_yes=True)
        assert rc == 2

    def test_batch_all_apply(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.apply_all", return_value={"t": TweakResult.APPLIED}),
        ):
            rc = _run_action("apply", "all", assume_yes=True)
        assert rc == 0

    def test_batch_all_abort(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("builtins.input", return_value="n"),
        ):
            rc = _run_action("apply", "all", assume_yes=False)
        assert rc == 1

    def test_single_apply_success(self, capsys) -> None:
        from regilattice.tweaks import all_tweaks

        first = all_tweaks()[0]
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch.object(type(first), "apply_fn", new_callable=lambda: property(lambda self: MagicMock())),
        ):
            rc = _run_action("apply", first.id, assume_yes=True)
        assert rc == 0

    def test_single_admin_error(self, capsys) -> None:
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
    def test_config_file_loaded(self, tmp_path, capsys) -> None:
        cfg_path = tmp_path / "test.toml"
        cfg_path.write_text('[general]\nforce_corp = true\n')
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--config", str(cfg_path), "--list"])
        assert rc == 0


# ── --search ─────────────────────────────────────────────────────────────────


class TestSearchFlag:
    def test_search_finds_tweaks(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--search", "explorer"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "tweak(s) found" in out

    def test_search_no_results(self, capsys) -> None:
        with patch("regilattice.cli.search_tweaks", return_value=[]):
            rc = main(["--search", "xyznonexistent999"])
        assert rc == 0
        assert "No tweaks matching" in capsys.readouterr().out


# ── --category ───────────────────────────────────────────────────────────────


class TestCategoryFlag:
    def test_category_apply(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
            patch("builtins.input", return_value="y"),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 0

    def test_category_unknown(self, capsys) -> None:
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "all", "--category", "NON_EXISTENT_CAT_999"])
        assert rc == 2
        assert "Unknown category" in capsys.readouterr().out

    def test_category_abort(self, capsys) -> None:
        with (
            patch("regilattice.cli.assert_not_corporate"),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
            patch("builtins.input", return_value="n"),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 1

    def test_category_corp_blocked(self, capsys) -> None:
        from regilattice.corpguard import CorporateNetworkError

        with (
            patch("regilattice.cli.assert_not_corporate", side_effect=CorporateNetworkError("blocked")),
            patch("regilattice.cli.tweaks_by_category", return_value={"Explorer": [MagicMock()]}),
        ):
            rc = main(["apply", "all", "--category", "Explorer"])
        assert rc == 6


# ── --export-json ────────────────────────────────────────────────────────────


class TestExportJson:
    def test_export_writes_file(self, tmp_path, capsys) -> None:
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
    def test_check_deps_all_present(self, capsys) -> None:
        rc = main(["--check-deps"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "pytest" in out


# ── --list-profiles ──────────────────────────────────────────────────────────


class TestListProfiles:
    def test_list_profiles(self, capsys) -> None:
        rc = main(["--list-profiles"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "business" in out
        assert "gaming" in out
        assert "privacy" in out
        assert "Profile" in out


# ── --categories ─────────────────────────────────────────────────────────────


class TestCategoriesFlag:
    def test_categories_lists_all(self, capsys) -> None:
        rc = main(["--categories"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Explorer" in out
        assert "categories" in out


# ── --tags ───────────────────────────────────────────────────────────────────


class TestTagsFlag:
    def test_tags_lists_all(self, capsys) -> None:
        rc = main(["--tags"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "Tag" in out
        assert "unique tags" in out
        # Tags always include common ones like "explorer" or "privacy"
        assert "explorer" in out or "privacy" in out


# ── --import-json ────────────────────────────────────────────────────────────


class TestImportJson:
    def test_import_applies_tweaks(self, tmp_path, capsys) -> None:
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

    def test_import_dict_format(self, tmp_path, capsys) -> None:
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

    def test_import_unknown_ids(self, tmp_path, capsys) -> None:
        jfile = tmp_path / "ids.json"
        jfile.write_text(json.dumps(["nonexistent-zzz-999"]), encoding="utf-8")
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 2
        assert "No valid tweaks" in capsys.readouterr().out

    def test_import_bad_json(self, tmp_path, capsys) -> None:
        jfile = tmp_path / "bad.json"
        jfile.write_text("{bad", encoding="utf-8")
        with patch("regilattice.cli.assert_not_corporate"):
            rc = main(["apply", "dummy", "--import-json", str(jfile), "-y"])
        assert rc == 3

    def test_import_abort(self, tmp_path, capsys) -> None:
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


# ── Interactive menu fallback ────────────────────────────────────────────────


class TestInteractiveMenuFallback:
    def test_non_windows_returns_4(self, capsys) -> None:
        with patch("regilattice.cli.is_windows", return_value=False):
            rc = main([])
        assert rc == 4
