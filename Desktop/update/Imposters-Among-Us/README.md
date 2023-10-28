# Imposters-Among-Us
CS 6457 Video Game Design - Fall 2023 - Team: Imposters Among Us - Game: Gladiator's Redemption


Start Scene: MainMenu


Instructions:
Start in the cell and talk to your cellmates, they will point you to leave the cell. As you leave the cell and walk towards the training room upstairs, Draxus the guard will confront you and eventually tell you where to go. The cells are meant to be an area where the story progresses and dialogue happens. You will then go upstairs to the right, and interact with the big double doors to enter the training room. When starting in the training room, the player will auto enter dialogue with Caelia the trainer, if you interact with her after this dialogue she will point you to leave the training room when ready. In the future, the training room will allow the player to improve their skills outside of the colosseum and also an area for the player to interact with NPCs outside of their cell. When done, you go interact with the double doors at the top of the stairs and move onto the Colosseum scene. You will start dialogue with your opponent and go through a couple of options of hearing people talk, interacting with the crowd, etc. The player will then be traveled back to the Cell scene when defeating the enemy (or able to respawn if they lost to the opponent which will restart the Colosseum scene), where they will enter one more dialogue with Quintus before the content for now ends.


Known Problem Areas: 
1. AI is iffy and early development, so it can circle the player or have some odd behavior.
2. Draxus is supposed to go back to his traversal after interacting with the player, but right now instead stays where the interactions starts and turns towards the player.


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
Cleaned up all the assets and codes with comments Scripts added/edited/debugged with identifying issues: PlayerMovement, CameraController, AIMovement, playerHealth, EnemyHealth.