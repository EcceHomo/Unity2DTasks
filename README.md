# Unity2DTasks

[![N|Solid](https://unity3d.com/profiles/unity3d/themes/unity/images/pages/branding_trademarks/unity-mwu-black.png)](https://unity.com/)

Unity2DTasks is 2D Unity project consisting of 2 scenes:
  - BlockPicking
  - PathFinder

# BlockPicking

  - Block picking character controller
  - Random box instantiating
  - Differentiate between orange and blue boxes
  - Placing corresponding box to dumpster
  - Event based component communication

# PathFinder

  - Grid based world bulding [x,y]
  - Pathfinding algorithm based on A*
  - Visual representation of path
  - Taking obsticles into consideration
  - Variation in path taking

### Project stucture
    ├───Assets
    │   ├───Animations
    │   │   ├───Character.controller
    │   │   ├───PickUp.anim
    │   │   ├───Throw.anim
    │   │   ├───Walk.anim
    │   │   └───WalkPickUp.anim
    │   ├───Prefabs
    │   │   ├───BlockPicking
    │   │   │ ├───BlueBox.prefab
    │   │   │ └───OrangeBox.prefab
    │   │   └─────BlockPicking
    │   │     ├───Cheese.prefab
    │   │     ├───GridTile.prefab
    │   │     └───Mouse.prefab
    │   ├───Scenes
    │   │   ├───BlockPicking.unity
    │   │   └───PathFinder.unity
    │   ├───Scripts
    │   │   ├───BlockPicking
    │   │   │ ├───BoxManager.cs
    │   │   │ ├───CharacterAIController.cs
    │   │   │ └───MapManager.cs
    │   │   ├───PathFinder
    │   │   │ ├───Graph.cs
    │   │   │ ├───GridManager.cs
    │   │   │ ├───MouseController.cs
    │   │   │ ├───Node.cs
    │   │   │ └───SearchGraph.cs
    │   │   └───Utils
    │   │     ├───CameraControl.cs
    │   │     └───EventManager.cs
    │   ├───Sounds
    │   │   ├───BoxPicking
    │   │   │ ├───BoxDestroyed.ogg
    │   │   │ ├───BoxPicked.ogg
    │   │   │ └───BoxPickingTheme.ogg
    │   │   └───Pathfinding
    │   │     ├───PathfindingTheme.ogg
    │   │     └───PathfiningTvStatic.ogg
    │   └───Textures
    │       ├───GridTile.png
    │       └───SpriteAtlas.png

# Trivia

> There are 10 .cs files and only one Update method
