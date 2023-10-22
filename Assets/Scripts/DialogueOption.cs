[System.Serializable]
public class DialogueOption
{
    public string text;  // Text for the option button
    public int nextSceneIndex;  // Index of the next scene to transition to when this option is chosen
}