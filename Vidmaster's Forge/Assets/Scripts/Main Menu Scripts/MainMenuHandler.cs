using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

   public void Play(){
        SceneManager.LoadScene("DM3");
    }

    public void Shop(){
        SceneManager.LoadScene("ItemsAndShopTest");
    }

    public void Weapons(){
        SceneManager.LoadScene("Weapon Testing Scene");
    }

    public void QuitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }


   
}
