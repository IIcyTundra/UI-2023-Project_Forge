using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamePlayController : MonoBehaviour
{

    [SerializeField] private Text sensitivityTextValue = null;
    [SerializeField] private Slider sensitivitySlider = null;
    [SerializeField] private int defaultSensitivity = 4;
    public int mainControllerSensitivity = 4;

    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private GameObject resetPrompt = null;

    public void SetSensitivity(float sensitivity) {
        mainControllerSensitivity = Mathf.RoundToInt(sensitivity);
        sensitivityTextValue.text = sensitivity.ToString("0");
    }

    public void GamePlayApply(int num) {
        PlayerPrefs.SetFloat("masterSensitivity", mainControllerSensitivity);
        PlayerPrefs.Save();
        StartCoroutine(Confirmation(num));
    }

    public void Reset(string MenuType) {
        if (MenuType == "GamePlay") {
            sensitivityTextValue.text = defaultSensitivity.ToString("0");
            sensitivitySlider.value = defaultSensitivity;
            mainControllerSensitivity = defaultSensitivity;
            GamePlayApply(1);
        }
    }

    public IEnumerator Confirmation(int type) {
        if ( type == 1 ) {
            resetPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            resetPrompt.SetActive(false);
        }
        else {
            confirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationPrompt.SetActive(false);
        }
    }

}
