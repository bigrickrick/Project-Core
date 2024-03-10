using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerMenu : MonoBehaviour
{
    public Slider slider;
    public PlayerCam playercam;
    public void GoToMainMenu()
    {
        Application.Quit();
    }
    public void ResartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeSensitivity()
    {
        
        float sliderValue = slider.value;

        
        playercam.sensX = sliderValue;
        playercam.sensY = sliderValue;
    }
}
