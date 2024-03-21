using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerMenu : MonoBehaviour
{
    public Slider MouseSentivitySlider;
    public Slider MusicSlider;
    public PlayerCam playercam;
    public MusicPlayer Music;
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
        
        float sliderValue = MouseSentivitySlider.value;

        
        playercam.sensX = sliderValue;
        playercam.sensY = sliderValue;
    }

    public void ChangeMusicVolume()
    {
        float volume = MusicSlider.value;
        Music.audioSource.volume = volume; 
    }
}
