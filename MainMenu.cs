using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject menuScreen;

    public void Level1()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void Level3()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }


    //Quit game/exit play mode if in Editor
    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)
    }
}
