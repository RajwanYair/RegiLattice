# RegiLattice — Tweak Categories Reference

> Complete reference for all 158 tweak categories in RegiLattice v6.34.0.
> Total: 7,718 tweaks across 158 categories.

Run `RegiLatticeCLI.exe --categories` to get live counts from your installed version.

---

## By Domain

### Privacy & Telemetry

| Category | Slug | Focus |
|----------|------|-------|
| Privacy | `priv` | Telemetry, diagnostic data, advertising ID, activity tracking |
| Telemetry Advanced | `telem` | Advanced telemetry pipes, CEIP, feedback frequency |
| AI / Copilot | `ai` | Windows Copilot, AI features, cloud-AI data sharing |
| M365 Copilot | `m365` | Microsoft 365 Copilot data policies |
| Windows Recall | `recall` | Windows Recall AI feature, screenshot retention |
| OneDrive | `od` | OneDrive sync, auto-start, file-on-demand notifications |
| Cloud Storage | `cloud` | Cloud backup prompts, sync centre, provider ads |
| Clipboard & Drag-Drop | `clip` | Cloud clipboard, clipboard history, drag-drop behaviour |
| Phone Link | `phone` | Phone Link app, mobile device notifications |
| Cortana & Search | `cortana` | Cortana, search suggestions, Bing integration |

---

### Security & Hardening

| Category | Slug | Focus |
|----------|------|-------|
| Security | `sec` | Core security: SEHOP, DEP, ASLR, driver signing |
| Hardening | `harden` | System hardening: LSA-PPL, SMB signing, LSASS protection |
| User Account | `uac` | UAC prompt levels, virtual account isolation |
| Encryption | `enc` | BitLocker, EFS file encryption settings |
| Firewall | `fw` | Windows Firewall profiles, blocking rules |
| Remote Desktop | `rdp` | RDP access, port, NLA, session timeout |
| RealVNC | `vnc` | VNC server configuration, authentication |
| Proxy & VPN | `proxy` | Proxy auto-detection, VPN split-tunnel settings |
| DNS & Networking Advanced | `dns` | DNS-over-HTTPS, LLMNR, NetBIOS, mDNS |
| Crash & Diagnostics | `crash` | WER, minidump settings, Dr. Watson |
| Lock Screen & Login | `lock` | Lock screen content, PIN/password policies |
| Screensaver & Lock | `ss` | Screensaver timeout, lock-on-sleep |

---

### Performance & System

| Category | Slug | Focus |
|----------|------|-------|
| Performance | `perf` | Visual effects, animation, prefetch, superfetch |
| Memory | `mem` | Virtual memory, paging file, memory compression |
| Boot | `boot` | Boot timeout, BCD settings, fast startup |
| Power | `power` | Power plans, hibernate, fast startup |
| Power Management | `pwrmgmt` | CPU throttling, sleep states, USB power |
| SSD Optimization | `ssd` | TRIM, write cache, defrag scheduling |
| Storage | `stor` | ReadyBoost, disk write caching, 8.3 names |
| File System | `fs` | NTFS timestamps, 8.3 filenames, hardlink limits |
| System | `sys` | System-wide settings, heap, timer resolution |

---

### Windows 11 & Shell

| Category | Slug | Focus |
|----------|------|-------|
| Windows 11 | `w11` | Win11-specific UI/UX tweaks |
| Taskbar | `tb` | Taskbar icons, clock, news, system tray |
| Explorer | `explorer` | File Explorer behaviour, ribbon, quick access |
| Context Menu | `ctx` | Right-click menu items, Send To |
| Shell | `shell` | Shell extensions, icon cache, folder types |
| Snap & Multitasking | `snap` | Snap layouts, virtual desktops, Alt-Tab |
| Widgets & News | `widgets` | Widgets panel, news feed, weather |
| Notifications | `notif` | Notification centre, quiet hours, focus assist |
| Display | `display` | HDR, scaling, refresh rate, ClearType |
| Night Light & Display | `night` | Night light schedule, colour temperature |
| Fonts | `font` | Font smoothing, ClearType, font rendering |

---

### Networking

| Category | Slug | Focus |
|----------|------|-------|
| Network | `net` | General network settings, homegroup, discovery |
| Network Optimization | `netopt` | TCP/IP stack, receive window, nagle algorithm |
| Maintenance | `maint` | Windows maintenance schedule, automatic tasks |

---

### Applications & Bloat

| Category | Slug | Focus |
|----------|------|-------|
| Debloat | `debloat` | Pre-installed app removal, consumer features |
| Microsoft Store | `msstore` | Store auto-updates, app installs, ratings prompts |
| App Compatibility | `compat` | AppCompatFlags, shim engine, compatibility layers |
| Startup | `startup` | Startup program delay, boot animations |

---

### Browser

| Category | Slug | Focus |
|----------|------|-------|
| Browser Common | `browser` | Common cross-browser settings |
| Edge | `edge` | Microsoft Edge telemetry, Bing, startup boost |
| Chrome | `chrome` | Google Chrome update, reporting, extensions policy |
| Firefox | `firefox` | Firefox update, telemetry, DRM, reporting |

---

### Developer & Tooling

| Category | Slug | Focus |
|----------|------|-------|
| Dev Drive / Developer | `dev` | Dev Drive, developer mode, Hyper-V, WSL settings |
| Command Line | `cmd` | CMD/PowerShell quickedit, buffer size, legacy mode |
| PowerShell | `ps` | PowerShell execution policy, history, telemetry |
| VS Code | `vscode` | VS Code telemetry, update settings |
| Windows Terminal | `term` | Windows Terminal settings, GPU rendering |
| Virtualization | `virt` | Hyper-V, containers, WDAG, sandbox settings |
| WSL | `wsl` | Windows Subsystem for Linux settings (interop, memory, systemd) |

---

### Accessibility

| Category | Slug | Focus |
|----------|------|-------|
| Accessibility | `acc` | Magnifier, Live Captions, Eye Control, Voice Access, Narrator, High Contrast |
| Voice Access & Speech | `speech` | Speech recognition, voice typing, language packs |
| Touch & Pen | `touch` | Touch keyboard, pen input, handwriting recognition |

---

### Input & Peripherals

| Category | Slug | Focus |
|----------|------|-------|
| Input | `input` | Keyboard repeat rate, mouse precision, haptic feedback |
| USB & Peripherals | `usb` | USB selective suspend, device enumeration |
| Bluetooth | `bt` | Bluetooth visibility, handsfree, audio quality |
| Printing | `printing` | Print spooler, network print discovery |

---

### Multimedia & Communication

| Category | Slug | Focus |
|----------|------|-------|
| Audio | `audio` | Exclusive mode, enhancements, Spatial Sound |
| Multimedia | `media` | Video playback, codec settings, Windows Media Player |
| Communication | `comm` | VoIP quality, call management, QoS |

---

### Services & Maintenance

| Category | Slug | Focus |
|----------|------|-------|
| Services | `svc` | Windows service startup types (disable/manual/auto) |
| Scheduled Tasks | `schtask` | Task Scheduler entries, telemetry tasks |
| Indexing & Search | `idx` | Windows Search indexing scope and performance |
| Event Logging | `evtlog` | Event log sizes, channel enable/disable |
| Disk Cleanup | `cleanup` | Disk cleanup automation, WinSxS, temp file policies |
| Backup & Recovery | `backup` | Backup schedules, VSS, shadow copies |
| Recovery | `recovery` | Recovery environment, WinRE availability |
| System Restore | `restore` | System Restore point frequency and storage |

---

### Office & Productivity

| Category | Slug | Focus |
|----------|------|-------|
| Office | `office` | Microsoft Office telemetry, updates, add-ins |
| M365 Copilot | `m365` | Microsoft 365 AI and data sharing policies |
| Adobe | `adobe` | Adobe Reader/Acrobat update and crash reporting |
| Java | `java` | Java update checker, deployment settings |
| LibreOffice | `lo` | LibreOffice telemetry and update settings |

---

### Package Management

| Category | Slug | Focus |
|----------|------|-------|
| Package Management | `pkg` | Package manager integrations (Scoop, pip, WinGet, Choco) |
| Scoop Tools | `scoop` | Scoop package tool installations |

---

## Profiles × Categories

Each built-in profile targets a specific set of categories:

### 🏢 business (39 categories)

Productivity, cloud tools, security hardening, OneDrive, Edge, networking, backup, and remote access. Enables corp-safe tweaks only.

### 🎮 gaming (31 categories)

GPU scheduling, power (high-performance plan), network optimization, input latency, audio, display (disable VSync issues), services (disable gaming-incompatible background tasks).

### 🔒 privacy (31 categories)

Privacy, telemetry, AI/Copilot, Windows Recall, Cortana, browser hardening (Edge/Chrome/Firefox), OneDrive, cloud storage, clipboard, phone link.

### ⚡ minimal (22 categories)

Performance, boot, SSD, file system, memory, startup, taskbar (clean), notifications (quiet), services (essential only).

### 🖥️ server (28 categories)

Hardening, security, firewall, RDP controls, services (disable GUI-only services), scheduled tasks (disable telemetry tasks), event logging, DNS, network, power (high-performance).

---

## Category Detail: Privacy (sample)

The **Privacy** category (`priv-*`) is the largest single category with 89 tweaks. Key areas:

| Area | Example tweaks |
|------|---------------|
| Telemetry | Disable AllowTelemetry, CEIP, feedback |
| Activity tracking | Disable activity history, timeline |
| Advertising | Disable advertising ID, app suggestions |
| Diagnostics | Disable WER uploads, diagnostic data |
| Location | Block location access, geofencing |
| Cortana | Disable Cortana, Bing search, suggestions |
| Clipboard | Disable cloud clipboard sync |
| Camera/Mic | Access policy for apps |

---

## Finding Tweaks

```powershell
# List all categories with counts
RegiLatticeCLI.exe --categories

# Search within a category
RegiLatticeCLI.exe search telemetry --scope machine

# List all tweaks in a category
RegiLatticeCLI.exe --list | Select-String "Privacy"

# Check category tweak counts in the engine stats
RegiLatticeCLI.exe --stats
```

---

## See Also

- [CLI-Reference.md](CLI-Reference.md) — full command reference
- [Profiles Guide](#profiles--categories) — which categories each profile covers
- [API Reference](Api.md) — `TweakEngine.Categories()`, `TweaksByCategory()`
