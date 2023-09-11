using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnTestMapButton() {
        SceneManager.LoadScene(1);
    }

    public void OnWeaponTestingButton() {
        SceneManager.LoadScene(2);
    }

    public void OnQuitButton() {
        Application.Quit();
    }
}
