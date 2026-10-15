# WOLOLO AR Card Battle

A two-player, turn-based augmented reality game built with **Unity, C# and Vuforia Engine** as an Augmented Reality course project at the University of Primorska, FAMNIT.

Physical character cards bring **Malakor the Wizard** and **Titus the Crusader** into the camera view. Players present ability cards to choose their actions, then resolve combat with animated characters, particle effects, sound effects and on-screen health updates.

**Platform:** Android Â· **Unity:** 6000.0.39f1 Â· **Team:** Mehmet KaÄŸan Ã‡evik and Ramazan ÃœÃ§ler

## Gameplay Demo

[Watch the gameplay video on YouTube] (https://www.youtube.com/watch?v=FrKgtgF83JI)

## How It Works

1. Point the camera at the physical character cards to display the characters in AR.
2. Press **START**. A virtual dice roll determines the initial turn order.
3. Present the appropriate ability cards to select each character's action: **Attack, Shield or Heal**.
4. Once both actions are selected, press **FIGHT** to resolve the round.
5. Continue until one character's health reaches zero.

The game uses **image-target tracking**, rather than plane detection. Character cards anchor the characters in the camera view, while ability cards provide physical inputs for the combat system.

## Features

- Vuforia image tracking for character and ability cards.
- Two characters with distinct combat animations and effects.
- Turn-based combat, initial dice rolls and alternating first-action order across rounds.
- Mobile UI with action buttons, health bars, turn information and a combat status log.
- Animation, particle and audio feedback for combat actions.

## Team Contributions

### Mehmet KaÄŸan Ã‡evik â€” Technical Art, AR Integration & Game Design

My work focused on assembling the playable experience in Unity and connecting the project's visual, audio and AR components.

- Set up Vuforia Engine, the AR Camera, the image-target database and scene configuration.
- Designed the physical character and ability cards, using AI-assisted artwork during the design process.
- Contributed to game design and the presentation of the card-based interactions.
- Built and configured the mobile UI Canvas, buttons, health displays and status elements.
- Imported and integrated third-party character models and animations; configured Animator Controllers and animation events.
- Integrated particle effects and sound effects with character actions.
- Supported debugging and made targeted C# adjustments to particle-effect playback in `Wizard.cs` and `Crusader.cs` to improve animation/effect synchronization.
- Troubleshot scene configuration and Android build issues.

### Ramazan ÃœÃ§ler â€” Core Gameplay Programming

Ramazan handled the core C# scripting, including the combat logic and gameplay systems. My scripting contribution was focused on integration fixes and visual-effect behavior rather than authorship of the core gameplay code.

## Technical Challenges

**AR camera conflicts:** AR Foundation components and the Vuforia camera were present in the same scene. Removing redundant XR Origin and AR Session components and configuring the Vuforia AR Camera resolved the conflicting setup.

**Ability-card ownership:** During testing, an ability card could trigger the wrong character's action. The solution used per-card owner and skill assignments in the Inspector to distinguish each player's abilities.

**Animation/effect synchronization:** Some effects ended too early or did not trigger correctly. Animation-event configuration and targeted adjustments to particle playback helped align the effects with character actions.

## Project Structure

| Path | Purpose |
| --- | --- |
| `Assets/Scenes/main_scene.unity` | Main gameplay scene |
| `Assets/Scripts/GameManager.cs` | Turn order, round resolution and game-over handling |
| `Assets/Scripts/Character.cs` | Shared character behavior |
| `Assets/Scripts/Wizard.cs` / `Crusader.cs` | Character-specific behavior |
| `Assets/Scripts/AbilityCardTarget.cs` | Ability-card interaction |
| `Assets/Scripts/HealthBar.cs` | Health-bar behavior |
| `Assets/StreamingAssets/Vuforia/` | Vuforia image-target database |
| `Assets/Editor/Vuforia/ImageTargetTextures/AR_project_cards/` | Character and ability target images |

## Opening the Project

1. Clone or download this repository.
2. Open the project in **Unity 6000.0.39f1**, with Android Build Support installed if building for a device.
3. Allow Unity to resolve dependencies and check the Vuforia Engine configuration. Configure a valid Vuforia license for your own use if required.
4. Open `Assets/Scenes/main_scene.unity`.
5. Prepare the character and ability cards from the target-image directory listed above.
6. Build for a compatible Android device and grant camera access when prompted.

Camera-based tracking depends on lighting, card visibility and image quality. This repository documents a student prototype, not a production-ready release.

## Assets & Acknowledgements

The project uses third-party assets, including character models and animations from the Unity Asset Store and Mixamo, alongside external audio, textures and fonts. Card designs include AI-assisted artwork. These assets are not presented as original models, animations or audio created by the team; the team's work includes their selection, configuration and integration into the game.
