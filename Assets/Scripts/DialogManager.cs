using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public GameObject optionButtonPrefab;
    public Transform buttonPanel;
    private readonly Dictionary<string, int> lastDialogueIndex = new();
    public Dictionary<string, List<DialogueScene>> npcDialogues = new();
    private string currentNPC = "";
    public int currentSceneIndex = 0;
    public GameObject dialogPanel;
    public Button[] optionButtons;
    private readonly Dictionary<string, int> lastSceneShown = new();
    public bool isInDialogue = false;

    void Start()
    {
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
                    new DialogueOption { text = "I demand an explanation!", nextSceneIndex = 2 },
                    new DialogueOption { text = "I need to find a way out, quickly.", nextSceneIndex = 2 }
                },
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
                    new DialogueOption { text = "Fight? What are you talking about?", nextSceneIndex = 3 },
                    new DialogueOption { text = "This is a mistake. I shouldn't be here.", nextSceneIndex = 3 }
                },
                autoProceed = false, dialogueIndex = 2
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Ah, the world's spinning for you too, huh? Welcome to the pit.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "I need to know, who are you? And why have I been thrown in here?", nextSceneIndex = 5 },
                    new DialogueOption { text = "Every corner of this place feels... dark.", nextSceneIndex = 6 },
                    new DialogueOption { text = "I demand answers. Now.", nextSceneIndex = 7 }
                },
                autoProceed = false, dialogueIndex = 4
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Name's Lucius. As for your arrival, well, that's the million-gold question. This colosseum, it's more than just bloody games. There are puppeteers behind the scenes.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 5
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Centuries of pain and betrayal leave their mark. Trust your instincts. And more importantly, watch your back.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 6
            },

            new DialogueScene
            {
                characterName = "Lucius",
                dialogue = "Calm yourself. All in due time. For now, your primary concern? Earning the respect of the clans.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 7
            },
        };

        npcDialogues["Chiron Woodland"] = new List<DialogueScene>
        {
            new DialogueScene
            {
                characterName = "Chiron Woodland",
                dialogue = "You must be new here. Big fight tomorrow. They like to toss in new souls to stir the pot.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "I don't remember how I got here.", nextSceneIndex = 4 }
                },
                autoProceed = false, dialogueIndex = 3
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
                    new DialogueOption { text = "You have the wrong person! I've committed no crimes!", nextSceneIndex = 9 },
                    new DialogueOption { text = "Unchain me at once!", nextSceneIndex = 9 },
                    new DialogueOption { text = "Laugh now, but one day, I'll be the one laughing.", nextSceneIndex = 9 }
                },
                autoProceed = false, dialogueIndex = 8
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "Heard that one before, don’t get too comfortable. You’ll be sent to the training grounds shortly. A bit of sharpening before the big show tonight.",
                options = new DialogueOption[]
                {
                    new DialogueOption { text = "Training? So, you do want me to stand a chance.", nextSceneIndex = 10 },
                    new DialogueOption { text = "I won’t be part of your twisted games.", nextSceneIndex = 11 }
                },
                autoProceed = false, dialogueIndex = 9
            },

            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "It’s not about you. It’s about the show. A swift death is a boring death.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 10
            },
            new DialogueScene
            {
                characterName = "Draxus",
                dialogue = "You don’t have a choice. Now, get moving!",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 11
            },
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
                    new DialogueOption { text = "Challenge accepted. I won't disappoint.", nextSceneIndex = 13 },
                    new DialogueOption { text = "Who do you think you are, ordering me around?", nextSceneIndex = 14 },
                    new DialogueOption { text = "I’ve been wrongfully accused! I have no business here!", nextSceneIndex = 15 }
                },
                autoProceed = false, dialogueIndex = 12
            },

            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Brave words. But bravery alone doesn't cut it. Let's see if your blade is as sharp as your tongue.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 13
            },
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "I am Caelia, keeper of discipline. My word is bond here. Challenge it, and you'll taste the sand before you know it.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 14
            },
            new DialogueScene
            {
                characterName = "Caelia",
                dialogue = "Neither did most. But fate's twisted, isn't it? Make it count.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 15
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
                    new DialogueOption { text = "And you are?", nextSceneIndex = 22 },
                    new DialogueOption { text = "Just a bit of prior experience coming into play.", nextSceneIndex = 23 },
                    new DialogueOption { text = "I could use some friends in this place. Are you trustworthy?", nextSceneIndex = 24 }
                },
                autoProceed = false, dialogueIndex = 21
            },

            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Names Quintus. Survived longer in this hellhole than most. If freedom's what you seek, find me when the moon is highest.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 22
            },
            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Skill can get you so far, experience is what keeps you alive. Stay sharp, and the colosseum might just teach you a thing or two.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 23
            },
            new DialogueScene
            {
                characterName = "Quintus",
                dialogue = "Trust? A luxury few can afford. But prove yourself to the clans, and perhaps we can discuss alliances and trust.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 24
            },
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
                autoProceed = false, dialogueIndex = 16
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
                autoProceed = false, dialogueIndex = 17
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
                autoProceed = false, dialogueIndex = 18
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
                autoProceed = false, dialogueIndex = 19
            },
            new DialogueScene
            {
                characterName = "",
                dialogue = "Ferocious beasts prowl and roar, hunger evident in their eyes. They are released as the crowd becomes disappointed. Gladiators huddle, uncertainty etched on their faces.",
                options = new DialogueOption[0],
                autoProceed = false, dialogueIndex = 20
            }
        };
    }

    public void StartDialogue(string sceneName)
    {
        Debug.Log("StartDialogue called with sceneName: " + sceneName);
        isInDialogue = true;

        currentNPC = sceneName;

        if (!lastSceneShown.TryGetValue(sceneName, out currentSceneIndex))
        {
            currentSceneIndex = 0;
        }

        StartScene(currentSceneIndex);
    }

    public void StartScene(int sceneIndex)
    {
        Debug.Log("StartScene called with sceneIndex: " + sceneIndex);

        if (sceneIndex == -1)
        {
            EndDialogue();
            return;
        }

        currentSceneIndex = sceneIndex;
        DialogueScene scene = npcDialogues[currentNPC][sceneIndex];

        dialogText.text = scene.characterName + ": " + scene.dialogue;

        // Destroy previous buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }

        if (scene.options.Length == 0)
        {
            EndDialogue();
            return;
        }

        float buttonHeight = 50f;
        float initialYPosition = 0;
        float padding = 10f;

        for (int i = 0; i < scene.options.Length; i++)
        {
            GameObject optionPanel = Instantiate(optionButtonPrefab, buttonPanel);
            optionPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, initialYPosition - i * (buttonHeight + padding));

            optionPanel.transform.SetAsLastSibling();

            Text optionText = optionPanel.GetComponentInChildren<Text>();
            optionText.text = scene.options[i].text;

            int nextSceneIndex = scene.options[i].nextSceneIndex;

            Button optionButton = optionPanel.GetComponent<Button>();
            optionButton.onClick.RemoveAllListeners();
            optionButton.onClick.AddListener(CreateListener(nextSceneIndex));
        }

        lastSceneShown[currentNPC] = currentSceneIndex;
    }

    private UnityEngine.Events.UnityAction CreateListener(int nextSceneIndex)
    {
        return () => {
            Debug.Log("Option button clicked. Going to next scene index: " + nextSceneIndex);
            StartScene(nextSceneIndex);
        };
    }

    public void EndDialogue()
    {
        Debug.Log("EndDialogue called");
        isInDialogue = false;
        dialogPanel.SetActive(false);
        currentNPC = "";
        currentSceneIndex = 0;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene has an associated dialogue
        if (npcDialogues.ContainsKey(scene.name))
        {
            // Start the dialogue automatically
            StartDialogue(scene.name);
        }
    }

    void OnDestroy()
    {
        // Unregister from scene loaded events when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        float buttonHeight = 50f; // Set height of each button
        float initialYPosition = 0; // Initial Y position for the first button
        float padding = 10f; // Padding between buttons

        // Create new option buttons based on the current dialogue scene
        for (int i = 0; i < dialogueScene.options.Length; i++)
        {
            GameObject button = Instantiate(optionButtonPrefab, buttonPanel);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, initialYPosition - i * (buttonHeight + padding));

            button.GetComponentInChildren<Text>().text = dialogueScene.options[i].text;

            int nextSceneIndex = dialogueScene.options[i].nextSceneIndex;
            button.GetComponent<Button>().onClick.AddListener(() => StartScene(nextSceneIndex));
        }
    }

}
