using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject playerCanvas;
    public GameObject interactUI;
    public GameObject dialogText;
    public GameObject buttonPanel;
    public static bool isPaused;
    public DialogManager dialogManager;

    private bool isDialog = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        isDialog = dialogManager.isInDialogue;
        dialogManager.isInDialogue = true;

        interactUI.SetActive(false);
        interactUI.transform.parent.gameObject.SetActive(false);
        Text uiText = interactUI.GetComponentInChildren<Text>(true);
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }

        dialogText.SetActive(false);
        Text dialogTextContents = dialogText.GetComponentInChildren<Text>(true);
        if (dialogTextContents != null)
        {
            dialogTextContents.gameObject.SetActive(false);
        }

        buttonPanel.SetActive(false);
        Text buttonText = buttonPanel.GetComponentInChildren<Text>(true);
        if (buttonText != null)
        {
            buttonText.gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Colosseum")
        {
            GameObject healthBar = playerCanvas.transform.Find("Health Bar").gameObject;
            GameObject enemyHealthBar = playerCanvas.transform.Find("Enemy Health Bar").gameObject;

            healthBar.SetActive(false);
            Text healthText = healthBar.GetComponentInChildren<Text>(true);
            if (healthText != null)
            {
                healthText.gameObject.SetActive(false);
            }

            enemyHealthBar.SetActive(false);
            Text enemyText = enemyHealthBar.GetComponentInChildren<Text>(true);
            if (enemyText != null)
            {
                enemyText.gameObject.SetActive(false);
            }
        }

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        dialogManager.isInDialogue = isDialog;

        interactUI.SetActive(true);
        interactUI.transform.parent.gameObject.SetActive(true);
        Text uiText = interactUI.GetComponentInChildren<Text>(false);
        if (uiText != null)
        {
            uiText.gameObject.SetActive(true);
        }

        dialogText.SetActive(true);
        Text dialogTextContents = dialogText.GetComponentInChildren<Text>(true);
        if (dialogTextContents != null)
        {
            dialogTextContents.gameObject.SetActive(true);
        }

        buttonPanel.SetActive(true);
        Text buttonText = buttonPanel.GetComponentInChildren<Text>(true);
        if (buttonText != null)
        {
            buttonText.gameObject.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "Colosseum")
        {
            GameObject healthBar = playerCanvas.transform.Find("Health Bar").gameObject;
            GameObject enemyHealthBar = playerCanvas.transform.Find("Enemy Health Bar").gameObject;

            healthBar.SetActive(true);
            Text healthText = healthBar.GetComponentInChildren<Text>(true);
            if (healthText != null)
            {
                healthText.gameObject.SetActive(true);
            }

            enemyHealthBar.SetActive(true);
            Text enemyText = enemyHealthBar.GetComponentInChildren<Text>(true);
            if (enemyText != null)
            {
                enemyText.gameObject.SetActive(true);
            }
        }

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
