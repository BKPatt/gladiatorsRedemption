using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public GameObject optionButtonPrefab;
    public Transform buttonPanel;
    private readonly Dictionary<string, int> lastDialogueIndex = new();
    public Dictionary<string, List<DialogueScene>> npcDialogues = new();
    private string currentNPC = "";
    private int currentSceneIndex = 0;
    public GameObject dialogPanel;
    public Button[] optionButtons;

    void Start()
    {
        // First chapter
        npcDialogues["FirstGlimpse"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Player",
                dialogue = "Hello?! Can anyone hear me?",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "I demand an explanation!", nextSceneIndex = 1 },
                    new DialogueOption { text = "I need to find a way out, quickly.", nextSceneIndex = 1 }
                },
                autoProceed = false, dialogueIndex = 1
            },
            new DialogueScene
            {
                characterName = "",
                dialogue = "Automatically proceeds to next scene",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 1
            }
        };

        // Initialize dialogues for Lucius
        npcDialogues["Lucius"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Another lamb for the slaughter. They never stop, do they?",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Fight? What are you talking about?", nextSceneIndex = 1 },
                    new DialogueOption { text = "This is a mistake. I shouldn't be here.", nextSceneIndex = 1 }
                },
                autoProceed = false, dialogueIndex = 2
            },
            new DialogueScene
            {
                characterName = "Chiron Woodland",
                dialogue = "You must be new here. Big fight tomorrow. They like to toss in new souls to stir the pot.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 2
            },
            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Ah, the world's spinning for you too, huh? Welcome to the pit.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "I need to know, who are you? And why have I been thrown in here?", nextSceneIndex = -1 },
                    new DialogueOption { text = "Every corner of this place feels... dark.", nextSceneIndex = -1 },
                    new DialogueOption { text = "I demand answers. Now.", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 2
            }
        };

        // Initialize dialogues for Draxus
        npcDialogues["Draxus"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "Well, if it isn't the runaway. Thought you could elude your past?",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "You have the wrong person! I've committed no crimes!", nextSceneIndex = 1 },
                    new DialogueOption { text = "Unchain me at once!", nextSceneIndex = 1 },
                    new DialogueOption { text = "Laugh now, but one day, I'll be the one laughing.", nextSceneIndex = 1 }
                },
                autoProceed = false, dialogueIndex = 3
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "Heard that one before, don’t get too comfortable. You’ll be sent to the training grounds shortly. A bit of sharpening before the big show tonight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Training? So, you do want me to stand a chance.", nextSceneIndex = -1 },
                    new DialogueOption { text = "I won’t be part of your twisted games.", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 3
            }
        };

        // Initialize dialogues for Caelia
        npcDialogues["Caelia"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Fall in line! This isn't a vacation. The sands of the arena are thirsty. And you are here to quench that thirst.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Challenge accepted. I won't disappoint.", nextSceneIndex = -1 },
                    new DialogueOption { text = "Who do you think you are, ordering me around?", nextSceneIndex = -1 },
                    new DialogueOption { text = "I’ve been wrongfully accused! I have no business here!", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 4
            }
        };

        // Initialize dialogues for Quintus
        npcDialogues["Quintus"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "A commendable display for a novice.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "And you are?", nextSceneIndex = -1 },
                    new DialogueOption { text = "Just a bit of prior experience coming into play.", nextSceneIndex = -1 },
                    new DialogueOption { text = "I could use some friends in this place. Are you trustworthy?", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 5
            }
        };

        npcDialogues["Opponent"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Opponent",
                dialogue = "Hope you've made peace with your gods.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Only one of us needs to.", nextSceneIndex = -1 },
                    new DialogueOption { text = "Don't underestimate me.", nextSceneIndex = -1 },
                    new DialogueOption { text = "I promise you, I won't be an easy prey.", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 6
            }
        };

        // Initialize dialogues for Varro
        npcDialogues["Varro"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Varro",
                dialogue = "Fascinating... This one's different.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 7
            }
        };

        // Initialize dialogues for Crowd
        npcDialogues["Crowd"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Crowd",
                dialogue = "Blood and glory! Blood and glory!",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Then let the sands be soaked!", nextSceneIndex = -1 },
                    new DialogueOption { text = "I am no slave to your whims.", nextSceneIndex = -1 }
                },
                autoProceed = false, dialogueIndex = 8
            }
        };

        // Initialize dialogues for Admirers and Masses (could be considered 'scenes')
        npcDialogues["AdmirersAndMasses"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "",
                dialogue = "After a remarkable victory, gifts rain down: gold, trinkets, and weapons, reflecting the crowd's favor.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 9
            },
            new DialogueScene
            {
                characterName = "",
                dialogue = "Ferocious beasts prowl and roar, hunger evident in their eyes. They are released as the crowd becomes disappointed. Gladiators huddle, uncertainty etched on their faces.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 9
            }
        };
    }

    public void StartNPCDialogue(string npcName)
    {
        currentNPC = npcName;
        currentSceneIndex = 0;
        StartScene(currentSceneIndex);
    }

    public void StartScene(int sceneIndex)
    {
        currentSceneIndex = sceneIndex;
        DialogueScene scene = npcDialogues[currentNPC][sceneIndex];

        dialogText.text = scene.characterName + ": " + scene.dialogue;

        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < scene.options.Length; i++)
        {
            GameObject optionButton = Instantiate(optionButtonPrefab, buttonPanel);
            optionButton.GetComponentInChildren<Text>().text = scene.options[i].text;
            int nextSceneIndex = scene.options[i].nextSceneIndex;
            optionButton.GetComponent<Button>().onClick.AddListener(() => StartScene(nextSceneIndex));
        }
    }

    public void TriggerNextDialogue(string characterName)
    {
        if (!lastDialogueIndex.ContainsKey(characterName))
        {
            lastDialogueIndex[characterName] = -1;
        }

        lastDialogueIndex[characterName]++;

        if (npcDialogues.ContainsKey(characterName))
        {
            List<DialogueScene> dialogues = npcDialogues[characterName];

            if (lastDialogueIndex[characterName] < dialogues.Count)
            {
                ShowDialog(dialogues[lastDialogueIndex[characterName]]);
            }
        }
    }

    private void ShowDialog(DialogueScene dialogueScene)
    {
        // Set the dialog panel active
        dialogPanel.SetActive(true);

        // Set the main dialogue text
        dialogText.text = dialogueScene.characterName + ": " + dialogueScene.dialogue;

        // Clear existing buttons
        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new option buttons based on the current dialogue scene
        for (int i = 0; i < dialogueScene.options.Length; i++)
        {
            GameObject button = Instantiate(optionButtonPrefab, buttonPanel);
            button.GetComponentInChildren<Text>().text = dialogueScene.options[i].text;

            int nextSceneIndex = dialogueScene.options[i].nextSceneIndex;
            button.GetComponent<Button>().onClick.AddListener(() => StartScene(nextSceneIndex));
        }
    }

}
