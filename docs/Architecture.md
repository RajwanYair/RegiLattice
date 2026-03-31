# RegiLattice — Architecture

> Visual overview of the solution structure, data flow, and component relationships.
> All diagrams use [Mermaid](https://mermaid.js.org/) — rendered natively in GitHub Markdown.

---

## Solution Overview

Three projects share `RegiLattice.Core` as the single source of truth for tweak logic:

```mermaid
graph TD
    GUI["🖥️ RegiLattice.GUI\n(WinForms · 11 themes)"]
    CLI["⌨️ RegiLattice.CLI\n(25+ commands)"]
    Core["🔧 RegiLattice.Core\n(engine · models · services)"]

    GUI -->|references| Core
    CLI -->|references| Core

    subgraph Core ["RegiLattice.Core"]
        TweakEngine["TweakEngine"]
        Models["Models\n(TweakDef · RegOp · ProfileDef)"]
        Registry["RegistrySession"]
        Services["Services\n(15 services)"]
        Tweaks["Tweaks/\n(83 modules · 9190 tweaks)"]
        Plugins["Plugins\n(Pack marketplace)"]

        TweakEngine --> Models
        TweakEngine --> Registry
        TweakEngine --> Services
        TweakEngine --> Tweaks
        TweakEngine --> Plugins
    end
```

---

## Core Data Flow — Apply a Tweak

```mermaid
sequenceDiagram
    participant User
    participant CLI/GUI
    participant TweakEngine
    participant TweakDef
    participant RegistrySession
    participant OS as Windows Registry

    User->>CLI/GUI: apply "priv-disable-telemetry"
    CLI/GUI->>TweakEngine: Apply(id)
    TweakEngine->>TweakDef: GetTweak(id)
    TweakDef-->>TweakEngine: TweakDef { ApplyOps: [...] }
    TweakEngine->>RegistrySession: Execute(ApplyOps)
    RegistrySession->>OS: SetDword(HKLM\...\AllowTelemetry, 0)
    OS-->>RegistrySession: OK
    RegistrySession-->>TweakEngine: TweakResult.Applied
    TweakEngine-->>CLI/GUI: TweakResult.Applied
    CLI/GUI-->>User: ✅ Applied
```

---

## TweakDef Model

```mermaid
classDiagram
    class TweakDef {
        +string Id
        +string Label
        +string Category
        +string Description
        +IReadOnlyList~string~ Tags
        +bool NeedsAdmin
        +bool CorpSafe
        +int MinBuild
        +IReadOnlyList~RegOp~ ApplyOps
        +IReadOnlyList~RegOp~ RemoveOps
        +IReadOnlyList~RegOp~ DetectOps
        +Action~bool~? ApplyAction
        +Func~bool~? DetectAction
        +int ImpactScore
        +int SafetyRating
        +TweakKind Kind
        +TweakScope Scope
    }

    class RegOp {
        +RegOpKind Kind
        +string Path
        +string Name
        +int? DwordValue
        +string? StringValue
        +SetDword(path, name, value)$ RegOp
        +SetString(path, name, value)$ RegOp
        +DeleteValue(path, name)$ RegOp
        +CheckDword(path, name, expected)$ RegOp
        +CheckMissing(path, name)$ RegOp
    }

    class TweakKind {
        <<enumeration>>
        Registry
        PowerShell
        SystemCommand
        ServiceControl
        ScheduledTask
        FileConfig
        GroupPolicy
        PackageManager
    }

    class TweakScope {
        <<enumeration>>
        User
        Machine
        Both
    }

    TweakDef "1" --> "0..*" RegOp : ApplyOps / RemoveOps / DetectOps
    TweakDef --> TweakKind : Kind
    TweakDef --> TweakScope : Scope
```

---

## TweakEngine Public API

```mermaid
mindmap
  root((TweakEngine))
    Core
      Register
      RegisterBuiltins
      AllTweaks
      GetTweak
      Categories
      TweaksByCategory
    Search & Filter
      Search
      Filter
    Status
      DetectStatus
      StatusMap
    Apply / Remove
      Apply
      Remove
      ApplyBatch
      RemoveBatch
    Profiles
      GetProfile
      TweaksForProfile
      ApplyProfile
    Snapshots
      SaveSnapshot
      LoadSnapshot
      RestoreSnapshot
    Validation
      ValidateTweaks
    Dependencies
      ResolveDependencies
      Dependents
```

---

## CI/CD Pipeline

```mermaid
flowchart LR
    commit(["git push\nor PR"]) --> ci

    subgraph ci ["ci.yml (on: push/PR)"]
        restore["Restore NuGet"] --> build["Build Release"]
        build --> test_core["Test Core"]
        test_core --> test_cli["Test CLI"]
        test_cli --> test_gui["Test GUI"]
        test_gui --> coverage["Upload Coverage\n(Codecov)"]
        coverage --> validate["Validate TweakDef\n(--validate)"]
        validate --> mutation["Mutation Tests\n(Stryker)"]
    end

    tag(["git push --tags\nvX.Y.Z"]) --> release

    subgraph release ["release.yml (on: tag v*)"]
        r_restore["Restore"] --> r_build["Build Release"]
        r_build --> r_test_core["Test Core"]
        r_test_core --> r_test_cli["Test CLI"]
        r_test_cli --> r_test_gui["Test GUI"]
        r_test_gui --> r_publish["Publish\nGUI + CLI EXEs"]
        r_publish --> r_installer["Build MSI\n(WiX)"]
        r_installer --> r_checksums["Generate\nSHA256SUMS"]
        r_checksums --> r_chocolatey["Build Chocolatey\npackage"]
        r_chocolatey --> r_release["Create GitHub\nRelease"]
        r_release --> r_verify["Verify Release\n(assets check)"]
    end

    r_release -.->|"on failure"| notify["notify-failure.yml\n(creates GH Issue)"]
```

---

## Package Manager Dialog Hierarchy (GUI)

```mermaid
classDiagram
    class BasePackageManagerDialog {
        <<abstract>>
        #SplitContainer splitContainer
        #ListView packageListView
        #RichTextBox logBox
        +ManagerName()* string
        +RefreshAsync()* Task
        +InstallAsync(name)* Task
        +RemoveAsync(name)* Task
        +UpgradeAsync(name)* Task
        +AppendLog(message)
        +SetBusy(busy)
    }

    class ChocolateyManagerDialog
    class ScoopManagerDialog
    class PipManagerDialog {
        #BuildScopePanel()
    }
    class WinGetManagerDialog {
        #AddExtraButtons()
    }
    class PSModuleManagerDialog {
        #BuildScopePanel()
        +UpgradeText = "Update"
    }

    BasePackageManagerDialog <|-- ChocolateyManagerDialog
    BasePackageManagerDialog <|-- ScoopManagerDialog
    BasePackageManagerDialog <|-- PipManagerDialog
    BasePackageManagerDialog <|-- WinGetManagerDialog
    BasePackageManagerDialog <|-- PSModuleManagerDialog
```

---

## 5 Built-in Profiles

```mermaid
quadrantChart
    title Tweak Profiles — Safety vs. Scope
    x-axis Few Categories --> Many Categories
    y-axis Conservative --> Aggressive
    quadrant-1 Power Users
    quadrant-2 Broad & Safe
    quadrant-3 Minimal
    quadrant-4 Targeted & Aggressive
    business: [0.65, 0.45]
    gaming: [0.55, 0.75]
    privacy: [0.50, 0.65]
    minimal: [0.30, 0.35]
    server: [0.47, 0.55]
```

---

## Registry Operation Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Defined : new TweakDef { ApplyOps = [...] }
    Defined --> Registered : TweakEngine.Register()

    Registered --> Detecting : DetectStatus()
    Detecting --> Applied : CheckDword matches expected
    Detecting --> NotApplied : CheckDword does not match
    Detecting --> Unknown : key/value missing or error

    NotApplied --> Applying : Apply()
    Applying --> Applied : RegistrySession.Execute(ApplyOps)

    Applied --> Removing : Remove()
    Removing --> NotApplied : RegistrySession.Execute(RemoveOps)

    Applied --> [*] : Snapshot saved
    NotApplied --> [*] : Snapshot saved
```
