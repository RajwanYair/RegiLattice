# RegiLattice — Architecture

> Visual overview of the solution structure, data flow, and component relationships.
> All diagrams are hand-crafted SVGs using the Catppuccin Mocha colour palette.

---

## Solution Overview

Three projects share `RegiLattice.Core` as the single source of truth for tweak logic:

<p align="center">
  <img src="assets/solution-overview.svg" alt="Solution Overview — GUI, CLI, Core subcomponents" width="100%"/>
</p>

---

## Core Data Flow — Apply a Tweak

<p align="center">
  <img src="assets/data-flow.svg" alt="Data Flow — Apply Tweak sequence diagram" width="100%"/>
</p>

---

## TweakDef Model

<p align="center">
  <img src="assets/tweakdef-model.svg" alt="TweakDef Model — UML class diagram" width="100%"/>
</p>

---

## TweakEngine Public API

<p align="center">
  <img src="assets/engine-api.svg" alt="TweakEngine Public API mindmap" width="100%"/>
</p>

---

## CI/CD Pipeline

<p align="center">
  <img src="assets/cicd-pipeline.svg" alt="CI/CD Pipeline — ci.yml and release.yml workflows" width="100%"/>
</p>

---

## Package Manager Dialog Hierarchy (GUI)

<p align="center">
  <img src="assets/pkg-mgr-hierarchy.svg" alt="Package Manager Dialog Hierarchy" width="100%"/>
</p>

---

## 5 Built-in Profiles

<p align="center">
  <img src="assets/profiles-quadrant.svg" alt="Built-in Profiles — quadrant chart" width="100%"/>
</p>

---

## Registry Operation Lifecycle

<p align="center">
  <img src="assets/registry-lifecycle.svg" alt="Registry Operation Lifecycle — state diagram" width="100%"/>
</p>
