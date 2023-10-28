using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class DialogueNode
{
    public string sceneName;  // New field to identify the scene
    public string npcName;
    public string dialogueText;
    public List<PlayerOption> playerOptions = new();
    public UnityEvent onNodeReached;
}

[System.Serializable]
public class PlayerOption
{
    public string optionText;
    public DialogueNode nextNode;
    public string npcResponse;  // New field for the NPC's response to the player option
}

// Serialized list to hold all dialogue nodes for each scene.
[System.Serializable]
public class DialogueList
{
    public List<DialogueNode> dialogueNodes = new();
}
