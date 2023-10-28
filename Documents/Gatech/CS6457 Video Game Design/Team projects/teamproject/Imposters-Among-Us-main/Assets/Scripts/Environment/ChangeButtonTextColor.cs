using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonTextColor : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        Text buttonText = yourButton.GetComponentInChildren<Text>();
        buttonText.color = Color.white;
    }
}
