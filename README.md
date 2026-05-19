# Floating Text

A Unity Package Manager (UPM) package for floating text effects — damage numbers, XP notifications, resource indicators, and more. Built on TextMeshPro with ScriptableObject-driven configuration.

## Requirements

- Unity 2020.3 or later
- TextMeshPro 3.0.0 or later

## Installation

Add the package via the Unity Package Manager using the Git URL:

```
https://github.com/zacharysnewman/floating-text.git
```

Or add it directly to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.zacharysnewman.floating-text": "https://github.com/zacharysnewman/floating-text.git"
  }
}
```

## Quick Start

1. **Create a `FloatingTextData` asset** — right-click in the Project window and choose `ScriptableObjects > FloatingTextData`. Configure the fields (see reference below).

2. **Set up a prefab** — create a UI GameObject with a `TMP_Text` component and a `FloatingTextEffect` component. Assign it to the `textPrefab` field of your `FloatingTextData` asset.

3. **Add `FloatingTextManager` to your scene** — attach the `FloatingTextManager` component to any GameObject. Assign a `Canvas` to its `Target Canvas` field.

4. **Spawn floating text** — call `CreateFloatingText` from any script:

```csharp
floatingTextManager.CreateFloatingText(
    target: enemyTransform,
    text: "150",
    data: damageTextData
);
```

## `FloatingTextData` Field Reference

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `textPrefab` | `GameObject` | — | Prefab with `TMP_Text` and `FloatingTextEffect` components. |
| `prefix` | `string` | `""` | Text prepended to the value (e.g. `"-"`). |
| `suffix` | `string` | `""` | Text appended to the value (e.g. `" XP"`). |
| `offset` | `Vector3` | `(0,0,0)` | World-space offset from the target transform at spawn time. |
| `floatDistance` | `float` | `5` | World-space units the text travels upward. |
| `duration` | `float` | `2` | Seconds the animation runs before the instance is destroyed. |
| `animationType` | `AnimationType` | `Linear` | Easing curve: `Linear`, `EaseIn`, `EaseOut`, or `EaseInOut`. |

## Multi-Camera Setup

By default `FloatingTextEffect` uses `Camera.main`. To use a different camera, assign it to the `Override Camera` field on the prefab or set it at runtime before the effect starts.

## Sample Scene

Import the **Example Usage** sample via the Package Manager to see damage, XP, and resource floating text in action.
