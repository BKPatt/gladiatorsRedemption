using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string initialScene;  // New field to hold the initial scene name
    private DialogManager dialogManager;

    void Start()
    {
        // Initialize DialogManager and DialogueList
        dialogManager = GameObject.Find("DialogCanvas").GetComponent<DialogManager>();
        dialogManager.dialogueList = new DialogueList();

        // First Glimpse
        DialogueNode firstGlimpseNode = new()
        {
            sceneName = "First Glimpse",
            npcName = "",
            dialogueText = "",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "Hello?! Can anyone hear me?", npcResponse = "" },
            new PlayerOption { optionText = "I demand an explanation!", npcResponse = "" },
            new PlayerOption { optionText = "I need to find a way out, quickly.", npcResponse = "" }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(firstGlimpseNode);

        // Shadows and Whispers
        DialogueNode shadowsAndWhispersNode = new()
        {
            sceneName = "Shadows and Whispers",
            npcName = "Lucius",
            dialogueText = "Another lamb for the slaughter. They never stop, do they?",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "Fight? What are you talking about?", npcResponse = "You must be new here. Big fight tomorrow. They like to toss in new souls to stir the pot." },
            new PlayerOption { optionText = "This is a mistake. I shouldn't be here.", npcResponse = "You must be new here. Big fight tomorrow. They like to toss in new souls to stir the pot." }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(shadowsAndWhispersNode);

        // A Roommate's Tale
        DialogueNode roommatesTaleNode = new()
        {
            sceneName = "A Roommate's Tale",
            npcName = "Lucius",
            dialogueText = "Ah, the world's spinning for you too, huh? Welcome to the pit.",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "I need to know, who are you? And why have I been thrown in here?", npcResponse = "Name's Lucius. As for your arrival, well, that's the million-gold question. This colosseum, it's more than just bloody games. There are puppeteers behind the scenes." },
            new PlayerOption { optionText = "Every corner of this place feels... dark.", npcResponse = "Centuries of pain and betrayal leave their mark. Trust your instincts. And more importantly, watch your back." },
            new PlayerOption { optionText = "I demand answers. Now.", npcResponse = "Calm yourself. All in due time. For now, your primary concern? Earning the respect of the clans." }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(roommatesTaleNode);

        // A Jailer's Mockery
        DialogueNode jailersMockeryNode = new()
        {
            sceneName = "A Jailer's Mockery",
            npcName = "Draxus",
            dialogueText = "Well, if it isn't the runaway. Thought you could elude your past?",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "You have the wrong person! I've committed no crimes!", npcResponse = "Heard that one before, don’t get too comfortable. You’ll be sent to the training grounds shortly. A bit of sharpening before the big show tonight." },
            new PlayerOption { optionText = "Unchain me at once!", npcResponse = "Heard that one before, don’t get too comfortable. You’ll be sent to the training grounds shortly. A bit of sharpening before the big show tonight." },
            new PlayerOption { optionText = "Laugh now, but one day, I'll be the one laughing.", npcResponse = "Heard that one before, don’t get too comfortable. You’ll be sent to the training grounds shortly. A bit of sharpening before the big show tonight." }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(jailersMockeryNode);

        // Rites of Passage
        DialogueNode ritesOfPassageNode = new()
        {
            sceneName = "Rites of Passage",
            npcName = "Caelia",
            dialogueText = "Fall in line! This isn't a vacation. The sands of the arena are thirsty. And you are here to quench that thirst.",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "Challenge accepted. I won't disappoint.", npcResponse = "Brave words. But bravery alone doesn't cut it. Let's see if your blade is as sharp as your tongue." },
            new PlayerOption { optionText = "Who do you think you are, ordering me around?", npcResponse = "I am Caelia, keeper of discipline. My word is bond here. Challenge it, and you'll taste the sand before you know it." },
            new PlayerOption { optionText = "I’ve been wrongfully accused! I have no business here!", npcResponse = "Neither did most. But fate's twisted, isn't it? Make it count." }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(ritesOfPassageNode);

        DialogueNode baptismByCombatNode = new()
        {
            sceneName = "Baptism by Combat",
            npcName = "Opponent",
            dialogueText = "Hope you've made peace with your gods.",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "Only one of us needs to.", npcResponse = "" },
            new PlayerOption { optionText = "Don't underestimate me.", npcResponse = "" },
            new PlayerOption { optionText = "I promise you, I won't be an easy prey.", npcResponse = "" }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(baptismByCombatNode);

        // The Crowd's Power
        DialogueNode theCrowdsPowerNode = new()
        {
            sceneName = "The Crowd's Power",
            npcName = "Crowd",
            dialogueText = "Blood and glory! Blood and glory!",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "Then let the sands be soaked!", npcResponse = "[Crowd erupts in ecstatic cheers]" },
            new PlayerOption { optionText = "I am no slave to your whims.", npcResponse = "[Crowd's boos grow menacingly]" }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(theCrowdsPowerNode);

        // A Mentor in the Shadows
        DialogueNode mentorInTheShadowsNode = new()
        {
            sceneName = "A Mentor in the Shadows",
            npcName = "Quintus",
            dialogueText = "A commendable display for a novice.",
            playerOptions = new List<PlayerOption>
        {
            new PlayerOption { optionText = "And you are?", npcResponse = "Names Quintus. Survived longer in this hellhole than most. If freedom's what you seek, find me when the moon is highest." },
            new PlayerOption { optionText = "Just a bit of prior experience coming into play.", npcResponse = "Skill can get you so far, experience is what keeps you alive. Stay sharp, and the colosseum might just teach you a thing or two." },
            new PlayerOption { optionText = "I could use some friends in this place. Are you trustworthy?", npcResponse = "Trust? A luxury few can afford. But prove yourself to the clans, and perhaps we can discuss alliances and trust." }
        }
        };
        dialogManager.dialogueList.dialogueNodes.Add(mentorInTheShadowsNode);
    }
}
