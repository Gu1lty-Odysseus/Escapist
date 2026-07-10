
---

# Team Organization

| Developer  | Role                          | Owns                                                       |
| ---------- | ----------------------------- | ---------------------------------------------------------- |
| **Latoya** | Technical Lead & Core Systems | Core architecture, managers, gameplay framework            |
| **Amir**   | World & Environment           | Rooms, lighting, prefabs, environment interactions         |
| **Lauren** | Puzzle & Escape Room Engineer | Puzzle mechanics, puzzle prefabs, moral dilemma logic      |
| **Ashly**  | Narrative & UI                | Story, dialogue, menus, journal, endings                   |
| **Kea**    | Player Systems & Persistence  | Player controller, inventory, save/load, audio integration |

---

# LATOYA

## Role

Technical Lead / Gameplay Architect

## Folder Ownership

```
Scripts/
    Core/
    Managers/
    Utilities/
```

No one else edits these folders without approval.

---

## Responsibilities

### GameManager

Responsible for

* Game State
* Scene State
* Game Flow
* Pause
* Restart
* Victory
* Failure

Scripts

```
GameManager.cs
```

---

### Input Manager

Handles

```
Keyboard

Mouse

Gamepad

Rebinding
```

Scripts

```
InputManager.cs
```

---

### Scene Manager

Responsible for

```
Loading Scenes

Room transitions

Fade transitions

Async Loading
```

Scripts

```
SceneLoader.cs

SceneTransition.cs
```

---

### Manager Framework

Create and maintain

```
GameManager

SceneManager

InputManager

EventManager

ServiceLocator
```

---

### Event System

No script may directly communicate with another system.

Everything goes through events.

Examples

```
DoorUnlocked

PuzzleSolved

PlayerDied

InventoryUpdated

DialogueStarted

DialogueFinished

ChoiceMade
```

---

### AI Guardrails

Allowed

```
Singletons

Interfaces

Events

SOLID

Dependency Injection

ScriptableObjects
```

Never

```
UI

Dialogue

Environment

Puzzle Logic

Player Movement

Inventory

Save System
```

---

# AMIR

## Role

Environment Engineer

Owns

```
Scenes/

Art/

Materials/

Prefabs/

Lighting/
```

---

## Responsibilities

### Build Rooms

```
Lobby

Room01

Room02

Room03

Ending
```

---

### Create Environment Prefabs

```
Mirror

Door

Chair

Cabinet

Desk

Terminal

Camera

Speaker

Projector

Generator

Emergency Light

Ceiling Light

Broken Monitor

Key Cabinet
```

---

### Lighting

Create lighting profiles

```
Normal

Emergency

Dark

Mirror World

Alarm

Ending
```

---

### Reflection Effects

Responsible for

```
Reflection Probes

Mirror Materials

Glass

Post Processing

Fog

Particles

Screen Effects
```

---

### Audio Zones

Creates trigger zones

```
Heartbeat

Echo

Whispers

Static

Alarm

Silence
```

No audio programming.

Only placement.

---

### AI Guardrails

Never modify

```
Scripts/Core

Managers

Gameplay

UI

Inventory

Save

Dialogue
```

---

# LAUREN

## Role

Puzzle Systems Engineer

Owns

```
Scripts/

PuzzleSystem/

Gameplay/
```

---

## Responsibilities

Create the reusable puzzle framework.

Every puzzle must inherit

```
PuzzleBase
```

---

### Puzzle Types

Develop

```
Button Puzzle

Pressure Plate

Combination Lock

Lever Puzzle

Sequence Puzzle

Mirror Alignment

Code Terminal

Keypad

Object Placement

Symbol Puzzle

Memory Puzzle

Reflection Puzzle
```

---

### Moral Dilemma System

Create

```
ChoiceController

ChoiceOutcome

ConsequenceTracker
```

Responsible for

```
Mercy

Justice

Trust

Selfishness

Sacrifice

Cooperation
```

---

### Puzzle Manager

Tracks

```
Solved

Failed

Hints

Attempts

Timer

Objectives
```

---

### AI Guardrails

Never create

```
GameManager

SaveManager

UI

Dialogue

Player Controller

Lighting

Scenes
```

---

# ASHLY

## Role

Narrative & UI Engineer

Owns

```
Scripts/UI/

Scripts/Story/

Canvas/

Dialogue/

Fonts/
```

---

## Responsibilities

### Main Menu

```
Play

Settings

Credits

Quit
```

---

### Pause Menu

```
Resume

Settings

Restart

Exit
```

---

### Dialogue System

Develop

```
DialogueManager

DialogueNode

DialogueChoice

Subtitle System

Voice Text

Typewriter Effect
```

---

### Story System

Responsible for

```
Journal

Documents

Audio Logs

Evidence

Narration

Story Progression
```

---

### Ending System

Create

```
Good Ending

Neutral Ending

Bad Ending

Secret Ending
```

---

### UI

Develop

```
Crosshair

Interaction Prompt

Inventory UI

Puzzle UI

Choice UI

HUD

Timer

Hint Display
```

---

### AI Guardrails

Never create

```
Managers

Gameplay

Puzzles

Scenes

Lighting

Player Controller
```

---

# KEA

## Role

Player Systems & Persistence Engineer

Owns

```
Scripts/

Player/

Save/

Audio/
```

---

## Responsibilities

### Player Controller

Develop

```
Walking

Running

Crouching

Looking

Interaction Ray

Footsteps

Head Bob

Sprint

Stamina
```

---

### Inventory

Create

```
InventoryManager

InventoryItem

InventorySlot

InventoryUIConnector
```

Supports

```
Keys

Puzzle Items

Story Items

Evidence

Collectables
```

---

### Save System

Create

```
SaveManager

Checkpoint

AutoSave

ManualSave

Serialization
```

Must save

```
Player Position

Inventory

Puzzle States

Dialogue Progress

Choices

Ending Flags
```

---

### Audio Manager

Integrate

```
Music

SFX

Ambient

Voice

Snapshots
```

Program only.

Placement belongs to Amir.

---

### AI Guardrails

Never create

```
Puzzle Logic

Dialogue

UI

Lighting

Environment

Managers owned by Latoya
```

---

# Shared Development Rules

## Branches

```
main

develop

feature/latoya-core

feature/amir-environment

feature/lauren-puzzles

feature/ashly-ui-story

feature/kea-player

hotfix/*
```

---

# Definition of Done

Every completed feature must include:

* Compiles with **0 errors**.
* No compiler warnings introduced.
* XML comments for public classes and methods where appropriate.
* Uses interfaces/events where applicable.
* Inspector variables marked with `[SerializeField]` instead of unnecessary public fields.
* Prefabs created when objects are reusable.
* Tested in Play Mode.
* Git commit with a descriptive message.
* README or usage notes for reusable systems.

---
