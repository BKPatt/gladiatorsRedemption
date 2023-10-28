using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColosseumRespawn : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Colosseum");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
