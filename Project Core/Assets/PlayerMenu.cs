using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Application.Quit();
    }
    public void ResartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
