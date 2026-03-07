"""Tests for regilattice.gui_dialogs — dialogs and export helpers."""

from __future__ import annotations

import json
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import MagicMock, patch

from regilattice.gui_dialogs import export_json_selection, export_powershell, import_json_selection, show_about
from regilattice.tweaks import TweakDef


def _make_td(**overrides: object) -> TweakDef:
    defaults: dict[str, object] = {
        "id": "test-tweak",
        "label": "Test Tweak",
        "category": "Test",
        "apply_fn": lambda *, require_admin: None,
        "remove_fn": lambda *, require_admin: None,
        "detect_fn": None,
        "needs_admin": True,
        "corp_safe": False,
        "registry_keys": [r"HKLM\SOFTWARE\Test\Key"],
        "description": "A test tweak",
        "tags": ["test"],
    }
    defaults.update(overrides)
    return TweakDef(**defaults)  # type: ignore[arg-type]


# ── export_powershell tests ──────────────────────────────────────────────────


class TestExportPowershell:
    def test_empty_selection_shows_info(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            export_powershell([], MagicMock())
            mbox.showinfo.assert_called_once()

    def test_cancel_dialog_does_nothing(self) -> None:
        td = _make_td()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = ""
            status = MagicMock()
            export_powershell([td], status)
            status.assert_not_called()

    def test_writes_ps1_file(self, tmp_path: Path) -> None:
        td = _make_td(
            id="sec-example",
            label="Example Tweak",
            registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Test"],
            description="Example description",
        )
        out = tmp_path / "out.ps1"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            status = MagicMock()
            export_powershell([td], status)
        content = out.read_text(encoding="utf-8-sig")
        assert "Example Tweak" in content
        assert "HKLM:" in content
        assert "#Requires -RunAsAdministrator" in content
        assert "Test-Path" in content
        status.assert_called_once()

    def test_multiple_tweaks_in_order(self, tmp_path: Path) -> None:
        tweaks = [
            _make_td(id="a-first", label="First", registry_keys=[r"HKLM\SOFTWARE\A"]),
            _make_td(id="b-second", label="Second", registry_keys=[r"HKCU\Software\B"]),
        ]
        out = tmp_path / "multi.ps1"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_powershell(tweaks, MagicMock())
        content = out.read_text(encoding="utf-8-sig")
        assert content.index("First") < content.index("Second")


# ── import_json_selection tests ──────────────────────────────────────────────


class TestImportJsonSelection:
    @staticmethod
    def _make_row(tid: str, *, disabled: bool = False) -> SimpleNamespace:
        return SimpleNamespace(
            td=SimpleNamespace(id=tid),
            var=MagicMock(),
            disabled_by_corp=disabled,
        )

    def test_cancel_dialog_does_nothing(self) -> None:
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = ""
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())

    def test_imports_list_of_ids(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps(["tweak-a", "tweak-b"]), encoding="utf-8")
        rows = [self._make_row("tweak-a"), self._make_row("tweak-b"), self._make_row("tweak-c")]
        deselect = MagicMock()
        update = MagicMock()
        status = MagicMock()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, deselect, update, status)
        deselect.assert_called_once()
        update.assert_called_once()
        rows[0].var.set.assert_called_once_with(True)
        rows[1].var.set.assert_called_once_with(True)
        rows[2].var.set.assert_not_called()

    def test_imports_dict_format(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps({"tweaks": ["tweak-x"]}), encoding="utf-8")
        rows = [self._make_row("tweak-x")]
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, MagicMock(), MagicMock(), MagicMock())
        rows[0].var.set.assert_called_once_with(True)

    def test_skips_corp_disabled_rows(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps(["tweak-a"]), encoding="utf-8")
        rows = [self._make_row("tweak-a", disabled=True)]
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, MagicMock(), MagicMock(), MagicMock())
        rows[0].var.set.assert_not_called()

    def test_invalid_json_shows_error(self, tmp_path: Path) -> None:
        jf = tmp_path / "bad.json"
        jf.write_text("{bad", encoding="utf-8")
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
        ):
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())
            mbox.showerror.assert_called_once()

    def test_unsupported_root_type_shows_error(self, tmp_path: Path) -> None:
        jf = tmp_path / "num.json"
        jf.write_text("42", encoding="utf-8")
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
        ):
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())
            mbox.showerror.assert_called_once()


# ── export_json_selection tests ──────────────────────────────────────────────


class TestExportJsonSelection:
    def test_empty_selection_shows_info(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            export_json_selection([], MagicMock())
            mbox.showinfo.assert_called_once()

    def test_cancel_dialog_does_nothing(self) -> None:
        td = _make_td()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = ""
            status = MagicMock()
            export_json_selection([td], status)
            status.assert_not_called()

    def test_writes_json_file(self, tmp_path: Path) -> None:
        td = _make_td(id="sec-example", label="Example Tweak")
        out = tmp_path / "out.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            status = MagicMock()
            export_json_selection([td], status)
        data = json.loads(out.read_text(encoding="utf-8"))
        assert data["tweaks"] == ["sec-example"]
        assert "version" in data
        status.assert_called_once()

    def test_multiple_tweaks_ids_in_order(self, tmp_path: Path) -> None:
        tweaks = [
            _make_td(id="a-first", label="First"),
            _make_td(id="b-second", label="Second"),
        ]
        out = tmp_path / "multi.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_json_selection(tweaks, MagicMock())
        data = json.loads(out.read_text(encoding="utf-8"))
        assert data["tweaks"] == ["a-first", "b-second"]

    def test_roundtrip_with_import(self, tmp_path: Path) -> None:
        """An exported JSON file can be reimported successfully."""
        td = _make_td(id="roundtrip-tweak")
        out = tmp_path / "roundtrip.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_json_selection([td], MagicMock())

        row = SimpleNamespace(td=SimpleNamespace(id="roundtrip-tweak"), var=MagicMock(), disabled_by_corp=False)
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(out)
            import_json_selection(MagicMock(), [row], MagicMock(), MagicMock(), MagicMock())
        row.var.set.assert_called_once_with(True)


# ── show_about tests ─────────────────────────────────────────────────────────


class TestShowAbout:
    def test_calls_messagebox(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            show_about(corp_blocked=False)
            mbox.showinfo.assert_called_once()
            title, body = mbox.showinfo.call_args.args
            assert "RegiLattice" in title
            assert "Corporate: No" in body

    def test_corp_blocked_true(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            show_about(corp_blocked=True)
            _, body = mbox.showinfo.call_args.args
            assert "Corporate: Yes" in body
