# PLAN.md — Implementation Plan

All phases implement the behavior defined in `SPEC.md`.
Complete phases in order; each phase is a standalone commit.

---

## Phase 1 — Critical Bug Fixes

**Files:** `Runtime/FloatingTextEffect.cs`

### 1a. Remove the infinite loop from `Start()`

Replace the `while (true)` coroutine loop with a single invocation of `FloatingTextAnimation()`.
The animation already destroys the GameObject on completion; no loop is needed.

**Before:**
```csharp
IEnumerator Start()
{
    ...
    while (true)
    {
        rectTransform.position = startingScreenPosition;
        tmpText.color = startColor;
        yield return FloatingTextAnimation();
    }
}
```

**After:**
```csharp
IEnumerator Start()
{
    rectTransform = GetComponent<RectTransform>();
    tmpText = GetComponent<TMP_Text>();
    mainCamera = Camera.main;
    yield return FloatingTextAnimation();
}
```

### 1b. Remove stale `startingScreenPosition` reset

With the loop gone, the stale cached position computed from `transform.position` (not `target.position + offset`) is also eliminated. `FloatingTextAnimation()` already derives position correctly from `target.position + offset` on its own first line.

---

## Phase 2 — API Completion & Easing

**Files:** `Runtime/FloatingTextData.cs`, `Runtime/FloatingTextEffect.cs`, `Runtime/FloatingTextManager.cs`

### 2a. Add animation defaults to `FloatingTextData`

Add `offset`, `floatDistance`, `duration`, and `animationType` fields to `FloatingTextData` so each ScriptableObject asset encapsulates a complete, self-contained floating text configuration.

### 2b. Add `animationType` and `overrideCamera` to `FloatingTextEffect`

Add `animationType` field. Implement easing in `FloatingTextAnimation()` by applying the selected curve to `t` before passing it to `Vector3.Lerp` and `Color.Lerp`.

Add optional `overrideCamera` field. Resolve the camera as `overrideCamera != null ? overrideCamera : Camera.main`.

### 2c. Wire everything through `FloatingTextManager.CreateFloatingText()`

Copy all fields from the `FloatingTextData` asset onto the spawned `FloatingTextEffect` instance:
`target`, `offset`, `floatDistance`, `duration`, `animationType`.

Remove all commented-out code.

---

## Phase 3 — Robustness

**Files:** `Runtime/FloatingTextManager.cs`

### 3a. Add null guards

Before instantiating, validate:
- `data != null`
- `data.textPrefab != null`
- `targetCanvas != null`

After instantiating, validate:
- `FloatingTextEffect` component is present

Log a descriptive `Debug.LogError` and return early (destroying the orphaned instance if needed) on any failure.

---

## Phase 4 — Documentation & Dependency Hygiene

**Files:** `README.md`, `package.json`

### 4a. Add `README.md`

Cover: what the package does, installation, quick-start usage, `FloatingTextData` field reference, multi-camera setup.

### 4b. Relax TextMeshPro version pin

Change `"com.unity.textmeshpro": "3.0.0"` to `"com.unity.textmeshpro": "3.0.0"` with a note — or relax to a minimum version range acceptable to the UPM resolver — to avoid conflicts with projects already on a newer TMP release.
