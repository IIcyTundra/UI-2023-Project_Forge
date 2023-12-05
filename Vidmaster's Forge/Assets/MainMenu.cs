using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSettings(){
        SceneManager.LoadSceneAsync("SettingsMenu");
    }

    public void LoadFireFight(){
        SceneManager.LoadSceneAsync("Firefight");
    }

    public void LoadTestRange(){
        SceneManager.LoadSceneAsync("WeaponTesting");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
