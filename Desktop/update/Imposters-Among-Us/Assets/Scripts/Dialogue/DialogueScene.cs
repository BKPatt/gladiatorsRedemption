using System.Collections.Generic;

[System.Serializable]
public class DialogueScene
{
    public string characterName;  // Name of the character who is speaking
    public string dialogue;  // The dialogue text
    public DialogueOption[] options;  // Array of options the player can choose from
    public bool autoProceed;  // Should the dialog automatically proceed to the next scene?
    public int dialogueIndex;

    public DialogueScene() { }

    public DialogueScene(string characterName, string dialogue, DialogueOption[] options, bool autoProceed, int dialogueIndex)
    {
        this.characterName = characterName;
        this.dialogue = dialogue;
        this.options = options;
        this.autoProceed = autoProceed;
        this.dialogueIndex = dialogueIndex;
    }
}

