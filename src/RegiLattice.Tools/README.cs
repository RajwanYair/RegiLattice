// RegiLattice.Tools — optional tooling dialogs
// This assembly provides supplemental system-tool panels (battery, network, disk, health)
// that are loaded optionally at runtime by MainForm.
//
// Design intent (A.4 roadmap):
//   - This project contains ~35 non-core dialog classes currently in RegiLattice.GUI
//   - MainForm loads RegiLattice.Tools.dll at startup if the file is present alongside the EXE
//   - Absence of the DLL gracefully degrades — tool menu items are hidden
//
// Migration guide (future sprints):
//   1. Move battery/network/disk/health dialogs from RegiLattice.GUI/Forms/ to this project
//   2. Define an IToolPanel interface here and implement per-dialog
//   3. MainForm discovers IToolPanel implementations via Assembly.GetTypes()
//   4. Each panel registers itself in the Tools menu on load

namespace RegiLattice.Tools;
