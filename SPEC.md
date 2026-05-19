# SPEC.md — Floating Text Runtime Specification

This is the source of truth for all runtime behavior, API contracts, and field semantics.
All code must conform to this document. Update this file before changing code.

---

## `AnimationType` (enum, `FloatingTextData.cs`)

Controls the easing curve applied to the float animation's progress value `t`.

| Value | Behavior |
|-------|----------|
| `Linear` | `t` passes through unchanged |
| `EaseIn` | Starts slow, accelerates — `t * t` |
| `EaseOut` | Starts fast, decelerates — `t * (2 - t)` |
| `EaseInOut` | Slow-fast-slow — `t < 0.5 ? 2*t*t : -1 + (4 - 2*t)*t` |

---

## `FloatingTextData` (ScriptableObject, `FloatingTextData.cs`)

Inspector-authored configuration asset. One asset per visual style (damage, XP, resource, etc.).

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `textPrefab` | `GameObject` | — | Prefab to instantiate. Must have `TMP_Text` and `FloatingTextEffect` components. |
| `prefix` | `string` | `""` | Prepended to the display string. |
| `suffix` | `string` | `""` | Appended to the display string. |
| `offset` | `Vector3` | `Vector3.zero` | World-space offset from the target transform at spawn time. |
| `floatDistance` | `float` | `5` | World-space units the text travels upward over the animation. |
| `duration` | `float` | `2` | Seconds the animation runs before the instance is destroyed. |
| `animationType` | `AnimationType` | `Linear` | Easing applied to the animation progress. |

---

## `FloatingTextEffect` (MonoBehaviour, `FloatingTextEffect.cs`)

Attached to each prefab. Owns all animation state. Spawned and configured by `FloatingTextManager`.

| Field | Type | Description |
|-------|------|-------------|
| `target` | `Transform` | The world-space anchor the text floats above. Set by the manager at spawn time. |
| `offset` | `Vector3` | World-space offset from `target` at animation start. Set by the manager at spawn time. |
| `floatDistance` | `float` | Upward travel distance in world units. Set by the manager at spawn time. |
| `duration` | `float` | Total animation duration in seconds. Set by the manager at spawn time. |
| `animationType` | `AnimationType` | Easing curve for the animation. Set by the manager at spawn time. |
| `overrideCamera` | `Camera` | If assigned, used instead of `Camera.main`. Optional; supports multi-camera scenes. |

### Lifecycle

- `Start()` runs the animation **once**, then destroys the GameObject. No looping.
- Position is derived fresh from `target.position + offset` at the start of each animation, never from a stale cached value.
- Alpha fades from the text's initial color to fully transparent over `duration` seconds.
- Easing is applied to the lerp `t` value via `animationType` before computing position and color.

### Camera Resolution

```
camera = overrideCamera != null ? overrideCamera : Camera.main
```

---

## `FloatingTextManager` (MonoBehaviour, `FloatingTextManager.cs`)

Scene singleton responsible for spawning floating text instances.

| Field | Type | Description |
|-------|------|-------------|
| `targetCanvas` | `Canvas` | Parent canvas for all spawned instances. Must be assigned. |

### `CreateFloatingText`

```csharp
public void CreateFloatingText(Transform target, string text, FloatingTextData data)
```

- Validates that `data`, `data.textPrefab`, and `targetCanvas` are non-null; logs an error and returns early if any are missing.
- Instantiates `data.textPrefab` as a child of `targetCanvas`.
- Retrieves `FloatingTextEffect` from the instance; logs an error and destroys the instance if not found.
- Sets `effect.target`, `effect.offset`, `effect.floatDistance`, `effect.duration`, `effect.animationType` from the data asset.
- Retrieves `TMP_Text` from the instance and sets `text` to `data.prefix + text + data.suffix`.
