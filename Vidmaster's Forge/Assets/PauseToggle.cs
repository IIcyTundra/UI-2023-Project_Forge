using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseToggle : MonoBehaviour
{
    bool isPaused;

    private void Awake()
    {
        isPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!SceneManager.GetSceneByName("SettingsMenu").isLoaded)
                SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);

            else
                SceneManager.UnloadSceneAsync("SettingsMenu");

            isPaused = !isPaused;
            Time.timeScale = (isPaused ? 0f : 1f);

            Debug.Log(Time.timeScale);
        }
    }
}
