# Tower Defense Unity Project

## Introduction
TowerDefenseUnity is a personal project aimed at exploring the capabilities of the Unity game engine while continually refining various conceptual ideas.

## Current Functionality
The project currently features a 15x15 grid that incorporates a procedurally generated pathway stretching from the starting point (0,0) to the endpoint (14,14). Presently, this pathway is rigidly hardcoded and lacks flexibility without minor adjustments. However, the project's future direction involves enhancing this system to offer greater adaptability.

## Enemy Spawning
The project includes a mechanism for initializing enemy spawners per wave. The spawning occurs in iterations based on the number and types of enemies present in each wave. The enemies within a given spawner continue to spawn until a predetermined count is achieved. Currently, this count is set arbitrarily within the code, with a consistent increment of +5 for every subsequent wave. (For varying enemy types, distinct hardcoded checks dictate changes in this pattern.) The objective is to transition this system to become JSON/data driven, enabling integration with a spreadsheet outlining wave compositions. This way, the appropriate enemies can be spawned according to the provided data.

## Tower Mechanics
A basic tower prefab has been developed, equipped with the ability to shoot projectiles at enemies within a designated range. At present, these projectiles possess "dumb" behavior, lacking target locking capabilities. However, the code repository contains provisions to toggle these projectiles into homing mode. The concept envisions a potential upgrade system where each shot might have a chance to become a homing projectile.

## Tower Placement System
Work has commenced on a tower placement system, enabling players to add and position towers within the game. Currently, a cell indicator alters the appearance of grid tiles based on mouse interaction. When the mouse hovers over a tile along the path, it turns red, whereas hovering over a grass tile changes it to green. This visual feedback mechanism will eventually serve to indicate when players are in tower placement mode, allowing them to position towers effectively.
