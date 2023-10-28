using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // Assume you have a button named "myButton" in your scene.
    public Button myButton;

    private void Start()
    {
        // Register the button event.
        myButton.onClick.AddListener(() => OnButtonClick("ButtonClicked"));
    }

    void OnButtonClick(string buttonName)
    {
        // Trigger the custom event.
        EventManager.TriggerEvent<ButtonClickEvent, string>(buttonName);
    }
}
