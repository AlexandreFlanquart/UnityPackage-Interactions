## [1.0.4] - 2026-07-07
### Fixed
- Trigger-only interactables (no Condition) now fire Enter/Exit effects
- No more phantom ExitEffect without a prior Enter
- `once` interactions can no longer be re-armed by a GameObject disable/enable cycle
- First `onEnableAction` is no longer lost when `delay = 0` (subscriptions now complete before activation)
- `Disable()` called mid-interaction now fires exit effects and fully resets state
- A `Disable()` call from inside an effect (e.g. `EffectUnityEvent`) is no longer overwritten
- `RangeHandler`/`ConditionRange` reset their state and notify listeners when disabled independently
- Missing `RangeChecker`/`player`/`rangeHandler` references now log a warning instead of throwing
- `shouldBeTrue` is serialized again (NOT-condition support restored). Note: prefabs/scenes carrying old serialized `shouldBeTrue` data will now read it back
- Docs example fixed (`EvaluateCondition()` is the override point, not `CheckCondition()`)

### Added
- `AInteractable.ResetInteractionState()` to re-arm a used-up `once` interaction (pooling/respawn)
- `RangeHandler.isMovingTarget` — opt-in distance recalculation for moving targets (e.g. patrolling NPCs)
- `ACondition.ResetReadyState()` protected helper for condition subclasses to reset on disable

### Changed
- `AInteractable` internally refactored around a single lifecycle state machine (Disabled/Ready/Entered/Interacting/WindingDown). No public API change; lifecycle methods called from an invalid state are now safely ignored
- `isEnable` is now a read-only computed property for subclasses

## [1.0.3] - 2026-04-07
### Fixed
- Fix interactions when disable/enable

### Changed
- Remove unused UI

## [1.0.2] - 2026-01-26
### Added
- Sample scene 2D
- 2D interactions
- Documentaions & comments

## [1.0.1] - 2025-07-11
### Fixed
- Fix samples
- Fix Animation

### Changed
- Upgrade sample scene

## [1.0.0] - 2025-07-01
### Added
- Condition logic
- Effect logic
- Interactable logic
- Range and Trigger Logic
- Sample
- Prefab

[1.0.4]:https://github.com/AlexandreFlanquart/UnityPackage-Interactions/releases/tag/1.0.4
[1.0.3]:https://github.com/AlexandreFlanquart/UnityPackage-Interactions/releases/tag/1.0.3
[1.0.2]:https://github.com/AlexandreFlanquart/UnityPackage-Interactions/releases/tag/1.0.2
[1.0.1]:https://github.com/AlexandreFlanquart/UnityPackage-Interactions/releases/tag/1.0.1
[1.0.0]:https://github.com/AlexandreFlanquart/UnityPackage-Interactions/releases/tag/1.0.0