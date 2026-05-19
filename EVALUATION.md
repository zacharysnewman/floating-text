# Library Evaluation: `com.zacharysnewman.floating-text`

**Evaluated:** 2026-04-11
**Version:** 1.0.0
**Repository:** https://github.com/zacharysnewman/floating-text

---

## Overview

A Unity Package Manager (UPM) package for floating text effects — damage numbers, XP notifications, resource indicators, etc. The package is built on TextMeshPro and uses a ScriptableObject-driven configuration pattern.

This is an early-stage release (v1.0.0, ~4 weeks old, 4 commits total) with a working proof-of-concept structure but several bugs that prevent it from functioning correctly in practice.

---

## Project Structure

```
Runtime/
  FloatingTextData.cs       # ScriptableObject configuration container
  FloatingTextEffect.cs     # Core animation coroutine
  FloatingTextManager.cs    # Factory for spawning floating text instances
Samples~/ExampleUsage/      # Sample scene and prefabs
.github/workflows/
  release.yml               # Semantic-release CI/CD pipeline
package.json
CHANGELOG.md
```

Follows standard UPM conventions. Assembly definition is correctly configured with a `Unity.TextMeshPro` reference.

---

## Critical Bugs

### 1. Infinite Animation Loop (`FloatingTextEffect.cs:27-32`)

```csharp
while (true)
{
    rectTransform.position = startingScreenPosition;
    tmpText.color = startColor;
    yield return FloatingTextAnimation();
}
```

`FloatingTextAnimation()` calls `Destroy(gameObject)` when `destroyOnComplete` is true, which ends the loop by destroying the object — but only on the first iteration. If `destroyOnComplete` is false, the animation will restart forever, resetting the position and color each time with no way to stop it. The intent appears to be a single play-through, not a loop.

**Impact:** Any use case with `destroyOnComplete = false` results in a permanent, uncontrollable loop.

### 2. Incomplete Public API (`FloatingTextManager.cs:9, 19-21`)

```csharp
public void CreateFloatingText(Transform target, string text, FloatingTextData floatingTextData)
    //, Vector3 offset, float floatDistance, float duration)
{
    floatingTextEffect.target = target;
    // floatingTextEffect.offset = offset;
    // floatingTextEffect.floatDistance = floatDistance;
    // floatingTextEffect.duration = duration;
}
```

The `offset`, `floatDistance`, and `duration` parameters are defined on `FloatingTextEffect` as public fields but are commented out of the manager's API. Callers cannot customize animation behavior at runtime — they must edit every prefab manually.

**Impact:** The library's core value proposition (configurable, reusable floating text) is broken at the API level.

### 3. Stale Starting Position (`FloatingTextEffect.cs:24, 29`)

```csharp
// Computed once at Start():
Vector3 startingScreenPosition = mainCamera.WorldToScreenPoint(startingPosition);

// Reused verbatim in the loop:
rectTransform.position = startingScreenPosition;
```

`startingPosition` is read from `transform.position` at `Start()`, not from `target.position + offset`. If the target has moved between spawn time and when the reset in the loop runs, the text snaps to the wrong location. More importantly, `FloatingTextAnimation()` correctly derives position from `target.position + offset` — the reset line is inconsistent with its own sibling coroutine.

**Impact:** Incorrect starting position after the first loop iteration; inconsistency with the rest of the animation logic.

---

## Additional Issues

| Severity | Location | Issue |
|----------|----------|-------|
| Medium | `FloatingTextData.cs:1-9` | `AnimationType` enum (`Linear`, `EaseIn`, `EaseOut`, `EaseInOut`) is declared but never referenced anywhere. Dead code that implies unfinished easing support. |
| Medium | `FloatingTextManager.cs:12, 15` | No null checks on `floatingTextData.textPrefab`, `targetCanvas`, or the `FloatingTextEffect` component. Any misconfigured prefab causes an unguarded `NullReferenceException`. |
| Medium | `FloatingTextEffect.cs:21` | Hard-coded `Camera.main` lookup runs every time an instance starts. This is a tagged-object search and is slow in scenes with many instances or multiple cameras. |
| Low | `package.json:8` | TextMeshPro pinned to exact version `3.0.0`. Stricter than necessary; may cause conflicts in projects already using a newer TMP version. |
| Low | All files | No XML doc comments, no README. There is no documentation for integrators beyond the sample scene. |

---

## What Is Working Well

- **UPM package structure** is clean and follows conventions correctly.
- **ScriptableObject configuration** (`FloatingTextData`) is the right pattern for this use case — easy to author in the Unity Inspector.
- **Core animation logic** inside `FloatingTextAnimation()` is sound: lerp-based position and alpha fade using `WorldToScreenPoint` is a correct approach.
- **Semantic-release CI/CD** pipeline is professionally configured for automated versioning and changelog generation.
- **Sample scene** provides a working reference for integrators.

---

## Dependency Summary

| Package | Declared Version | Notes |
|---------|-----------------|-------|
| `com.unity.textmeshpro` | `3.0.0` (exact) | Could be relaxed to `>=3.0.0` |
| Unity editor | `2020.3+` | Reasonable minimum |

---

## Git History

| Commit | Date | Message |
|--------|------|---------|
| `bd6bfeb` | 2026-03-17 | chore(release): 1.0.0 [skip ci] |
| `8227b8a` | 2026-03-16 | fix: standardize author field in package.json |
| `eb0b686` | 2026-03-16 | ci: add semantic-release workflow and config |
| `0939d58` | 2026-03-13 | Initial commit: floating-text UPM package v1.0.0 |

4 commits total. No development activity since the initial release.

---

## Summary

| Category | Status |
|----------|--------|
| Project structure | Good |
| Core animation logic | Buggy (infinite loop, stale position) |
| Public API | Incomplete (parameters commented out) |
| Configuration pattern | Good |
| Documentation | Missing |
| CI/CD | Good |
| Dependency hygiene | Minor issue (exact version pin) |

The library has a solid structural foundation but is **not production-ready**. The three critical bugs must be resolved before the package behaves as intended:

1. Remove or redesign the `while (true)` loop in `Start()`.
2. Expose `offset`, `floatDistance`, and `duration` through `CreateFloatingText()`.
3. Fix the stale `startingScreenPosition` reset to derive from `target.position + offset`.

Addressing the dead `AnimationType` enum (implement easing or remove it), adding null guards, and adding a README would bring the library to an acceptable release quality.
