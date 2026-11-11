# Jump Race 3D — Hyper-Casual Prototype

A 3D hyper-casual racing prototype in Unity: bounce from trampoline to trampoline along a generated path and reach the finish before the AI racers.

> Built in August 2020 (commit history: 10 days, 2 design iterations). Archived as a finished prototype.

## Gameplay

- Hold and drag left or right to steer the character in mid-air. Landing on a pad bounces you on to the next jump.
- A guide laser shows where you will land.
- AI opponents race on the same path; the last pad is the finish zone.
- Miss a pad and you fall.

Inspired by the mechanic of the mobile game *Jump Race*; the code (apart from the third-party SplineMesh library) is my own.

## Technical highlights

- **Procedural course** — `PathGenerator` places jumping pads along a spline with random spacing, scale, yaw and side offset, sets materials and turns the last pad into the finish zone. A custom editor (`Editor/PathGeneratorEditor.cs`) lets designers regenerate the course in the editor.
- **AI racers** — `EnemyAI` picks the next pad as its target, steers towards it and bounces on contact, so the AI follows the same physics as the player.
- **Landing guide** — `GuideLaser` raycasts down to show the landing point.
- **Game loop:** `GameManager` switches the game between playable and unplayable states, and the player, the AI and the UI all check this state.
- **Jump feel:** a fall multiplier gives a snappy arc with a quick rise and a faster fall.

## Project layout

```
Assets/Scripts/
  PlayerController.cs    jump physics and steering
  EnemyAI.cs             AI racer
  PathGenerator.cs       procedural pad placement on a spline
  JumpingPad.cs          bounce behaviour
  GuideLaser.cs          landing indicator
  GameManager.cs, UIController.cs, InputManager.cs
Assets/SplineMesh/       third-party spline library
```

## Run it

Open in Unity **2019.2.21f1** and play the scene in `Assets/Scenes`. Portrait orientation.

## Tech

Unity 2019.2 · C# · SplineMesh · TextMesh Pro
