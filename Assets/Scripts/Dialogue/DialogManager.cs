using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    // UI components and data structures for dialog logic
    public Text dialogText;
    public GameObject optionButtonPrefab;
    public Transform buttonPanel;
    private readonly Dictionary<string, int> lastDialogueIndex = new();
    public Dictionary<string, List<DialogueScene>> npcDialogues = new();
    public string currentNPC = "";
    public int currentSceneIndex = 0;
    public GameObject dialogPanel;
    public Button[] optionButtons;
    private readonly Dictionary<string, int> lastSceneShown = new();
    public bool isInDialogue = false;
    public string playerClan = "";
    public GameObject player;
    public GameObject dummy = null;

    void Start()
    {
        if (dummy != null)
        {
            dummy.SetActive(false);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // First chapter
        npcDialogues["FirstGlimpse"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Player",
                dialogue = "Hello?! Can anyone hear me?",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: I demand an explanation!", nextSceneName = "Lucius", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 2</b>: I need to find a way out, quickly.", nextSceneName = "Lucius", nextSceneIndex = 0 }
                },
                autoProceed = true
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
                    new DialogueOption { text = "<b>Option 1</b>: This is a mistake. I shouldn't be here.", nextSceneName = "Chiron", nextSceneIndex = 0 }
                },
                autoProceed = true
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Ah, the world's spinning for you too, huh? Welcome to the pit.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: I need to know, who are you? And why have I been thrown in here?", nextSceneName = "Lucius", nextSceneIndex = 2 },
                    new DialogueOption { text = "<b>Option 2</b>: Every corner of this place feels... dark.", nextSceneName = "Lucius", nextSceneIndex = 3 },
                    new DialogueOption { text = "<b>Option 3</b>: I demand answers. Now.", nextSceneName = "Lucius", nextSceneIndex = 4 }
                },
                autoProceed = true
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Name's Lucius. As for your arrival, well, that's the million-gold question. This colosseum, it's more than just bloody games. There are puppeteers behind the scenes.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Centuries of pain and betrayal leave their mark. Trust your instincts. And more importantly, watch your back.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Calm yourself. All in due time. For now, your primary concern? Earning the respect of the clans.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Draxus has unlocked the cell, you should go to the training room upstairs to get ready for the fight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };

        // Initialize dialogues for Chiron
        npcDialogues["Chiron"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Chiron",
                dialogue = "You must be new here. Big fight tomorrow. They like to toss in new souls to stir the pot.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: I don't remember how I got here.", nextSceneName = "Lucius", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Fight? What are you talking about?", nextSceneName = "Lucius", nextSceneIndex = 1 }
                },
                autoProceed = true
            },

            new DialogueScene
            {
                characterName = "Chiron",
                dialogue = "You best get moving to training. It is your best bet to win the fight tonight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };

        // Initialize dialogues for Leaving Dialogue
        npcDialogues["Leave"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "",
                dialogue = "",
                options = new DialogueOption[0],
                autoProceed = true
            },
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
                    new DialogueOption { text = "<b>Option 1</b>: You have the wrong person! I've committed no crimes!", nextSceneName = "Draxus", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Laugh now, but one day, I'll be the one laughing.", nextSceneName = "Draxus", nextSceneIndex = 1 }
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "Heard that one before, don’t get too comfortable. You’ll need to walk towards the training grounds. It is upstairs to the right. A bit of sharpening before the big show tonight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Training? So, you do want me to stand a chance.", nextSceneName = "Draxus", nextSceneIndex = 2 },
                    new DialogueOption { text = "<b>Option 2</b>: I won’t be part of your twisted games.", nextSceneName = "Draxus", nextSceneIndex = 3 }
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "It’s not about you. It’s about the show. A swift death is a boring death.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "You don’t have a choice. Now, get moving!",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "What are you doing? They are waiting for you at training upstairs!",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
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
                    new DialogueOption { text = "<b>Option 1</b>: Challenge accepted. I won't disappoint.", nextSceneName = "Caelia", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Who do you think you are, ordering me around?", nextSceneName = "Caelia", nextSceneIndex = 2 },
                    new DialogueOption { text = "<b>Option 3</b>: I’ve been wrongfully accused! I have no business here!", nextSceneName = "Caelia", nextSceneIndex = 3 }
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Brave words. But bravery alone doesn't cut it. Let's see if your blade is as sharp as your tongue. Talk to the clan leaders and choose your clan and fighting style.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "I am Caelia, keeper of discipline. My word is bond here. Challenge it, and you'll taste the sand before you know it. Talk to the clan leaders and choose your clan and fighting style.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Neither did most. But fate's twisted, isn't it? Make it count. Talk to the clan leaders and choose your clan and fighting style.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Test your fighting skills on the dummy after choosing a clan and talk to your opponent at the top of the stairs if you'd like. When you are done with training, you can proceed out the doors to the colosseum for the fight. Good luck tonight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
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
                    new DialogueOption { text = "<b>Option 1</b>: And you are?", nextSceneName = "Quintus", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Just a bit of prior experience coming into play.", nextSceneName = "Quintus", nextSceneIndex = 2 },
                    new DialogueOption { text = "<b>Option 3</b>: I could use some friends in this place. Are you trustworthy?", nextSceneName = "Quintus", nextSceneIndex = 3 }
                },
                autoProceed = false
            },

            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Names Quintus. Survived longer in this hellhole than most.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Skill can get you so far, experience is what keeps you alive. Stay sharp, and the colosseum might just teach you a thing or two.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Trust? A luxury few can afford. But prove yourself to the clans, and perhaps we can discuss alliances and trust.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
        };

        // Initialize dialogues for Colosseum Opponent
        npcDialogues["Decimus"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Decimus",
                dialogue = "Are you ready for the big fight? Only one of us is making it out of there.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Only one of us needs to. (Decimus gets angry)", nextSceneName = "Leave", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 2</b>: I just want an honorable fight. (Decimus is less angry)", nextSceneName = "Leave", nextSceneIndex = 0 }
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Decimus",
                dialogue = "Hope you've made peace with your gods.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Only one of us needs to.", nextSceneName = "Varro", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 2</b>: Don't underestimate me.", nextSceneName = "Varro", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 3</b>: I promise you, I won't be an easy prey.", nextSceneName = "Varro", nextSceneIndex = 0 }
                },
                autoProceed = false
            }
        };

        npcDialogues["Minotaur Leader"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Minotaur Leader",
                dialogue = "I am Titus Stormbringer the leader of the Minotaur clan. We fight with battle axes to survive. We could use a newbie if you are interested.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Count me in.", nextSceneName = "Minotaur Leader", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Let me think about it.", nextSceneName = "Leave", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 3</b>: No thanks.", nextSceneName = "Leave", nextSceneIndex = 0 }
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Minotaur Leader",
                dialogue = "Great decision! Go test your new weapon on the dummy",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Minotaur Leader",
                dialogue = "Can't say I agree with your decision in clans.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };

        npcDialogues["Dimachaeru Leader"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Dimachaeru Leader",
                dialogue = "I am Titus Stormbringer the leader of the Dimachaeru clan. We fight with halberds for each other. You are always welcome here.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Count me in.", nextSceneName = "Dimachaeru Leader", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Let me think about it.", nextSceneName = "Leave", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 3</b>: No thanks.", nextSceneName = "Leave", nextSceneIndex = 0 }
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Dimachaeru Leader",
                dialogue = "Great decision! Go test your new weapon on the dummy",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Dimachaeru Leader",
                dialogue = "I hope your clan choice works out for you.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };

        npcDialogues["Sagittarii Leader"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Sagittarii Leader",
                dialogue = "I am Sagittarius Arrowsong the leader of the Sagittarii clan. We fight with swords against the other clans. We can always use fresh meat.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Count me in.", nextSceneName = "Sagittarii Leader", nextSceneIndex = 1 },
                    new DialogueOption { text = "<b>Option 2</b>: Let me think about it.", nextSceneName = "Leave", nextSceneIndex = 0 },
                    new DialogueOption { text = "<b>Option 3</b>: No thanks.", nextSceneName = "Leave", nextSceneIndex = 0 }
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Sagittarii Leader",
                dialogue = "Great decision! Go test your new weapon on the dummy",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "Sagittarii Leader",
                dialogue = "You made the wrong decision, newbie. We will see blood in the arena.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };

        // Initialize dialogues for Varro
        npcDialogues["Varro"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Varro",
                dialogue = "Fascinating... This one's different.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Crowd", nextSceneIndex = 0}
                },
                autoProceed = false
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
                    new DialogueOption { text = "<b>Option 1</b>: Then let the sands be soaked! (Gain extra health and the crowd's addoration)", nextSceneName = "", nextSceneIndex = -1 },
                    new DialogueOption { text = "<b>Option 2</b>: I am no slave to your whims. (Health stays the same and the crowd seems bored)", nextSceneName = "", nextSceneIndex = -1 }
                },
                autoProceed = false
            }
        };

        // Initialize dialogues for Admirers and Masses (could be considered 'scenes')
        npcDialogues["AdmirersAndMasses"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "",
                dialogue = "After a remarkable victory, gifts rain down: gold, trinkets, and weapons, reflecting the crowd's favor.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            },
            new DialogueScene
            {
                characterName = "",
                dialogue = "Ferocious beasts prowl and roar, hunger evident in their eyes. They are released as the crowd becomes disappointed. Gladiators huddle, uncertainty etched on their faces.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "<b>Option 1</b>: Leave", nextSceneName = "Leave", nextSceneIndex = 0}
                },
                autoProceed = false
            }
        };
    }

    // Start dialogue with a given NPC
    public void StartDialogue(string sceneName)
    {
        currentNPC = sceneName;
        isInDialogue = true;

        // Try to get the last scene shown for this NPC, default to 0 if not found
        if (!lastSceneShown.TryGetValue(sceneName, out currentSceneIndex))
        {
            currentSceneIndex = 0;
        }

        StartScene(sceneName, currentSceneIndex);
    }

    // Display the dialogue scene based on the NPC and scene index
    public void StartScene(string sceneName, int sceneIndex)
    {
        currentNPC = sceneName;
        dialogPanel.SetActive(true);
        isInDialogue = true;

        // If the sceneIndex is -1, end the dialogue
        if (sceneIndex == -1)
        {
            EndDialogue();
            return;
        }

        // Check if the player is already in a clan
        if (playerClan != "" && (sceneName == "Minotaur Leader" || sceneName == "Dimachaeru Leader" || sceneName == "Sagittarii Leader") && sceneIndex == 0)
        {
            sceneIndex = 2;
        }

        float buttonHeight = 50f;
        float initialYPosition = 0;
        float padding = 10f;

        // Ensure the NPC and scene index are valid
        if (npcDialogues.ContainsKey(currentNPC) && sceneIndex < npcDialogues[currentNPC].Count)
        {
            currentSceneIndex = sceneIndex;
            DialogueScene scene = npcDialogues[currentNPC][sceneIndex];
            dialogText.text = scene.characterName + ": " + scene.dialogue;

            // Remove existing buttons
            foreach (Transform child in buttonPanel)
            {
                Destroy(child.gameObject);
            }

            // No options means the dialogue is ended
            if (scene.options.Length == 0)
            {
                EndDialogue();
                return;
            }

            // Create buttons for each dialogue option
            for (int i = 0; i < scene.options.Length; i++)
            {
                GameObject optionPanel = Instantiate(optionButtonPrefab, buttonPanel);
                optionPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, initialYPosition - i * (buttonHeight + padding));
                optionPanel.transform.SetAsLastSibling();
                Text optionText = optionPanel.GetComponentInChildren<Text>();
                optionText.text = scene.options[i].text;

                int nextSceneIndex = scene.options[i].nextSceneIndex;
                Button optionButton = optionPanel.GetComponent<Button>();
                optionButton.interactable = true;
                optionButton.onClick.RemoveAllListeners();
                optionButton.onClick.AddListener(CreateListener(scene.options[i].nextSceneName ?? currentNPC, nextSceneIndex));
            }

            // Update the last scene shown for this NPC
            lastSceneShown[currentNPC] = currentSceneIndex;
        }
        else
        {
            // Invalid NPC or scene index
            EndDialogue();
        }
    }

    // Create a UnityAction for button clicks to go to the next dialogue scene
    private UnityEngine.Events.UnityAction CreateListener(string nextSceneName, int nextSceneIndex)
    {
        return () => {
            if (nextSceneName == "Minotaur Leader" || nextSceneName == "Dimachaeru Leader" || nextSceneName == "Sagittarii Leader")
            {
                playerClan = nextSceneName.Split(' ')[0];
                GivePlayerWeapon(playerClan, player);
            }

            StartScene(nextSceneName, nextSceneIndex);
        };
    }

    // End the dialogue and reset variables
    public void EndDialogue()
    {
        isInDialogue = false;
        dialogPanel.SetActive(false);
        currentNPC = "";
        currentSceneIndex = 0;
    }

    // Move to the next dialogue for a given NPC
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

    // Handle scene loading to initiate dialogues
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (npcDialogues.ContainsKey(scene.name))
        {
            StartDialogue(scene.name);
        }
    }

    // Cleanup when the object is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void GivePlayerWeapon(string clanName, GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            // Deactivate all weapons initially
            playerMovement.battleAxe.SetActive(false);
            playerMovement.halberd.SetActive(false);
            playerMovement.sword.SetActive(false);

            // Activate the weapon based on the clan
            switch (clanName)
            {
                case "Minotaur":
                    playerMovement.battleAxe.SetActive(true);
                    dummy.SetActive(true);
                    break;
                case "Dimachaeru":
                    playerMovement.halberd.SetActive(true);
                    dummy.SetActive(true);
                    break;
                case "Sagittarii":
                    playerMovement.sword.SetActive(true);
                    dummy.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }


    // Display the actual dialogue on the UI
    private void ShowDialog(DialogueScene dialogueScene)
    {
        dialogPanel.SetActive(true);
        dialogText.text = dialogueScene.characterName + ": " + dialogueScene.dialogue;

        // Remove existing buttons
        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        float buttonHeight = 50f;
        float initialYPosition = 0;
        float padding = 10f;

        // Create buttons for each dialogue option
        for (int i = 0; i < dialogueScene.options.Length; i++)
        {
            GameObject button = Instantiate(optionButtonPrefab, buttonPanel);
            button.GetComponentInChildren<Text>().text = dialogueScene.options[i].text;
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, initialYPosition - i * (buttonHeight + padding));

            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1); // Left top
            rectTransform.anchorMax = new Vector2(0, 1); // Left top
            rectTransform.pivot = new Vector2(0, 1); // Left top
            rectTransform.anchoredPosition = new Vector2(0, initialYPosition - i * (buttonHeight + padding));

            string nextSceneName = dialogueScene.options[i].nextSceneName ?? currentNPC;
            int nextSceneIndex = dialogueScene.options[i].nextSceneIndex;

            button.GetComponent<Button>().onClick.AddListener(() => StartScene(nextSceneName, nextSceneIndex));
        }
    }
}