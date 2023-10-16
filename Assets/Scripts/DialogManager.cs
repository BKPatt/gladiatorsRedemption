using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text npcNameText;
    public Text dialogText;
    public GameObject optionsPanel;
    public Button optionButtonPrefab;
    public DialogueList dialogueList;  // New field to hold all dialogue nodes
    public event EventHandler<DialogueEvent> OnDialogueTrigger;

    void Start()
    {
        dialogPanel.SetActive(false);
    }

    public void StartDialogueBySceneName(string sceneName)
    {
        DialogueNode nodeToTrigger = dialogueList.dialogueNodes.Find(node => node.sceneName == sceneName);
        if (nodeToTrigger != null)
        {
            ShowDialog(nodeToTrigger);
        }
    }

    public void ShowDialog(DialogueNode dialogueNode)
    {
        dialogPanel.SetActive(true); // Enable the dialog panel

        // Populate the NPC name and dialogue text
        npcNameText.text = dialogueNode.npcName;
        dialogText.text = dialogueNode.dialogueText;

        // Clear out any existing buttons
        foreach (Transform child in optionsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create option buttons
        foreach (PlayerOption option in dialogueNode.playerOptions)
        {
            Button newButton = Instantiate(optionButtonPrefab, optionsPanel.transform);
            newButton.GetComponentInChildren<Text>().text = option.optionText;
            newButton.onClick.AddListener(() => HandlePlayerChoice(option));
        }
    }

    private void HandlePlayerChoice(PlayerOption chosenOption)
    {
        // For now, just close the dialog panel; extend this to handle player choices.
        dialogPanel.SetActive(false);
    }

}
