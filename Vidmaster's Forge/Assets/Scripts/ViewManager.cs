using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ViewManager : MonoBehaviour
{
    public void PlayScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        if(SceneManager.GetSceneByName("Firefight").isLoaded)
            SceneManager.UnloadSceneAsync("Firefight");
        if (SceneManager.GetSceneByName("WeaponTesting").isLoaded)
            SceneManager.UnloadSceneAsync("WeaponTesting");

        SceneManager.UnloadSceneAsync(sceneName);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

