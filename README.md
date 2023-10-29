# Imposters-Among-Us
CS 6457 Video Game Design - Fall 2023 - Team: Imposters Among Us - Game: Gladiator's Redemption


Start Scene: MainMenu


Instructions:
Start in the cell and talk to your cellmates, they will point you to leave the cell. As you leave the cell and walk towards the training room upstairs, Draxus the guard will confront you and eventually tell you where to go. The cells are meant to be an area where the story progresses and dialogue happens. You will then go upstairs to the right, and interact with the big double doors to enter the training room. When starting in the training room, the player will auto enter dialogue with Caelia the trainer, if you interact with her after this dialogue she will point you to leave the training room when ready. In the future, the training room will allow the player to improve their skills outside of the colosseum and also an area for the player to interact with NPCs outside of their cell. When done, you go interact with the double doors at the top of the stairs and move onto the Colosseum scene. You will start dialogue with your opponent and go through a couple of options of hearing people talk, interacting with the crowd, etc. The player will then be traveled back to the Cell scene when defeating the enemy (or able to respawn if they lost to the opponent which will restart the Colosseum scene), where they will enter one more dialogue with Quintus before the content for now ends.

Button Mappings:
W: Forward
A: Left
S: Backwards (known issue about moving forward slightly after moving back)
D: Right
Space: Jump
Left Click: Attack
P: Pause/Resume

Following Number keys can be used as shortcuts to toggle between scenes.

| Scene  | Shortcut  |
|:--------:| -------------:|
| Cell| 1 |
| Colosseum| 2 |
| TrainingRoom| 3 |


Known Problem Areas: 
1. AI is iffy and early development, so it can circle the player or have some odd behavior.
2. Draxus is supposed to go back to his traversal after interacting with the player, but right now instead stays where the interactions starts and turns towards the player.
3. Load/Save function is developed, but does not yet work as what data to be saved/loaded has not been determined.
4. Looking up and down is not locked for some reason in the Training Room scene.
5. There is no end scene yet, so after beating the NPC the NPC is just destroyed and nothing happens yet.
6. Some scene changes have differences in lighting
7. Player has sliding effects when moving, needs animation adjustments


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
Scripts added/edited: MainMenu, PauseMenu, ColosseumRespawn, playerHealth, DataPersistenceManager, FileDataHandler, IDataPersistence, GameData

Ashok:
1. Added sound setup to the game and added the Backgoround scrore to the scenes.
2. Added Unity event manager setup.
3. Added walking sound to the characters using the event manager setup
4. Added bear asset to the project, fours instances are present in the Play arena scene
5. Added hotkeys setup for the scenes, hot keys added to toggle between all scenes
5. Added audio assets and prefabs
Scripts added/edited: HotKeyScript,AudioEventManager, EventSound3D, BGController, WalkEvent  

Cole:
1. Added the player combat and the fight scenes in the main Colosseum arena while modifying PlayerMovement
2. Created the AI for the Minotaur and created the AIMovement scripts
3. Created and modified the PlayerMovent calling of the animations and focused on the attack/jump
4. Created the UI health bars and tracking of damage done using colliders and triggers on enemy and player axe
5. Modified CameraController script to back off on its focus on the player
6. Also created EnemyAxe and SwordHit scripts to check if collisions occurred between weapons and player/enemy and doc health from each
Scripts added/edited: PlayerMovement, CameraController, AIMovement, playerHealth, EnemyHealth, EnemyAxe, SwordHit
