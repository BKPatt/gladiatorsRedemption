# Imposters-Among-Us
CS 6457 Video Game Design - Fall 2023 - Team: Imposters Among Us - Game: Gladiator's Redemption


Start Scene: MainMenu


Instructions:
Start in the cell and talk to your cellmates, they will point you to leave the cell. As you leave the cell and walk towards the training room upstairs, Draxus the guard will confront you and eventually tell you where to go. The cells are meant to be an area where the story progresses and dialogue happens. You will then go upstairs to the right, and interact with the big double doors to enter the training room. When starting in the training room, the player will auto enter dialogue with Caelia the trainer, if you interact with her after this dialogue she will point you to leave the training room when ready. The training room allows the player to choose between the 3 clans changing the weapon they will have in the fight, and you can test the weapon on the training dummy that appears after choosing a clan. When done, you go interact with the double doors at the top of the stairs and move onto the Colosseum scene. You will start dialogue with your opponent and go through a couple of options of hearing people talk, interacting with the crowd, etc. The player will then be given a game over screen when defeating the enemy (or able to respawn if they lost to the opponent which will restart the Colosseum scene).

Button Mappings:
W: Forward
A: Left
Double tap A: roll left
S: Backwards (known issue about moving forward slightly after moving back)
Double tap S: roll backwards
D: Right
Double tap D: roll right
Space: Jump
Left Click: Attack
P: Pause/Resume


Known Problem Areas: 
1. Draxus is supposed to go back to his traversal after interacting with the player, but right now instead stays where the interactions starts and turns towards the player.
2. Attack animation can be odd sometimes and start in the middle of the animation for some reason


Who Did What:

Brantley:
1. Added all dialogue logic to the game
2. Added to PlayerMovement to handle the player interacting with the environment and NPCs
3. Added characters to each scene
4. Added guard pathfinding in the Cells
5. Set up each of the 3 player scenes (Cells, TrainingRoom, Colosseum)
6. Added a rough camera controller to the player to work with PlayerMovement
7. Added logic to move between scenes with the doors.
Scripts added/edited: DialogManager, PlayerMovement, DialogueOption, DialogueScene, DialogueNode, DialogueEvent, DoorController, DoorSelector, CameraController, ChangeButtonTextColor, CharacterAI
Post Alpha:
1. Revamped the dialogue.
2. Added different weapons based on dialogue in the training room.
3. Added the ability to talk to the opponent in the training room.
4. Added some dialogue hints to the training room where the choices matter.
5. Made the attack UI around training dummy clearer
6. Added dodging left right and back
7. Look into the strafing issue and fixed that.
8. Gameplay video
9. Revamped the AI a bit.
10. Fixed camera angles and moving while character moves.
11. Fixed the attack clipping.
12. Fixed the jump clipping.
Scripts added/edited: DialogManager, PlayerMovement, CharacterAI

Shiyu Liu:
1. Generated all the animations to 9 characters including idle, walk, run, backwards walk, left turn, right turn, jump and attack.
2. Added animator with blend tree and different layers to control the characters
3. Added rigid body, character controller to all characters
4. Created PlayerMovement script with movement logic to move the Player
5. Created a camera controller to the player to test player movement
6. Cleaned up all the assets and codes with comments 
Scripts added/edited/debugged with identifying issues: PlayerMovement, CameraController, EventManager, AIMovement, playerHealth, EnemyHealth.

Seth:
1. Designed and integrated MainMenu and Respawn Scenes, and PauseMenu canvas.
2. Integrated all button functions for each menu.
3. Wrote scripts associated to all menu functions.
4. Implemented a DataPersistenceManager to support load/save functionality in game.
5. Discovered Artlist.io as a source for license free music and SFX.
6. Added music to all menus and scenes or engaging UX.
Scripts added/edited: MainMenu, PauseMenu, ColosseumRespawn, ColosseumGameOver, playerHealth, enemyHealth, DataPersistenceManager, FileDataHandler, IDataPersistence, GameData

Ashok:
1. Added sound setup to the game and added the Backgoround scrore to the scenes.
2. Added Unity event manager setup.
3. Added walking sound to the characters using the event manager setup
4. Added bear asset to the project, fours instances are present in the Play arena scene
5. Added hotkeys setup for the scenes, hot keys added to toggle between all scenes
5. Added audio assets and prefabs
6. Fixed the issues with AI circling the player, optimised the path finding and attacking parts
7. Used PlayerPrefs Unity Class to store the weapons info and share the info across the scenes
Scripts added/edited: HotKeyScript,AudioEventManager, EventSound3D, BGController, WalkEvent, AIMovement, PlayerMovement, DialogueManagement  

Cole:
1. Added the player combat and the fight scenes in the main Colosseum arena while modifying PlayerMovement
2. Created the AI for the Minotaur and created the AIMovement scripts
3. Created and modified the PlayerMovent calling of the animations and focused on the attack/jump
4. Created the UI health bars and tracking of damage done using colliders and triggers on enemy and player axe
5. Modified CameraController script to back off on its focus on the player
6. Also created EnemyAxe and SwordHit scripts to check if collisions occurred between weapons and player/enemy and doc health from each
Scripts added/edited: PlayerMovement, CameraController, AIMovement, playerHealth, EnemyHealth, EnemyAxe, SwordHit
