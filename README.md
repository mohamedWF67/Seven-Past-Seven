# Seven-Past-Seven

A 2D platformer game built with Unity featuring action-packed gameplay, abilities, and puzzle elements.

## About

Seven-Past-Seven is a platformer game where players navigate through challenging levels using a variety of movement mechanics and abilities. The game features a scoring system based on time and performance, multiple acts/levels, checkpoint systems, and various enemy types.

## Features

### Player Mechanics
- **Advanced Movement System**: Running, jumping (with double jump support), and dashing
- **Ability System**: Multiple ability types including projectile shooting and special powers
- **Health System**: Player health management with healing fields and damage indicators
- **Checkpoint System**: Save progress at checkpoints throughout levels

### Gameplay Elements
- **Enemy Types**: Various enemies including shooters, movers, and decaying damage enemies
- **Environmental Hazards**:
  - Falling platforms
  - Falling traps
  - Fire hazards
  - Breakable bricks
- **Platforming Mechanics**:
  - Moving platforms
  - One-way platforms
  - Portal system
- **Puzzle System**: Interactive puzzles with UI feedback
- **Collectibles**: Keys, items, and pickups

### Game Systems
- **Score System**: Time-based scoring with multipliers
- **Dialogue System**: Story and interaction dialogues
- **Sound & Visual Effects**:
  - Camera shake effects
  - Colored flash effects
  - Particle systems
  - Floating damage text
  - Full-screen effects
- **Menu Systems**: Main menu, pause menu, settings, and ability/stats menus
- **Developer Mode**: Built-in dev tools for testing

## Technical Details

- **Engine**: Unity
- **Language**: C#
- **Input System**: Unity's new Input System
- **Rendering**: Universal Render Pipeline (URP) with 2D lighting

## Project Structure

```
Assets/
├── Scripts/          # Game logic and systems
├── Sprites/          # Visual assets
└── SO/              # ScriptableObjects (Acts, Audio)
```

## Getting Started

### Requirements
- Unity (2021.3 or later recommended)
- Git

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Seven-Past-Seven.git
   ```
2. Open the project in Unity
3. Load a scene from the Acts to start playing

### Controls
- **Movement**: Arrow Keys/WASD
- **Jump**: Space
- **Dash**: Shift
- **Shoot/Abilities**: Mouse/Controller buttons
- **Pause**: Escape

## Development

The game is actively being developed with regular updates. Check the commit history for the latest changes and version updates.

### Key Scripts
- `PlayerMovementScript.cs`: Handles all player movement mechanics
- `GameManagerScript.cs`: Manages game state, scoring, and scene transitions
- `AbilityScript.cs`: Ability system implementation
- `HealthSystem.cs`: Player and enemy health management
- `DialogueSystem.cs`: Dialogue and story interactions

## License

This project is licensed under the terms included in the LICENSE file.

## Version History

- v0.0.18 - Final Version
- v0.0.17 - Dialogue system improvements
- v0.0.16 - Major updates
- v0.0.15 - Continued development
- Earlier versions - Core systems development

---

Built with Unity 🎮
