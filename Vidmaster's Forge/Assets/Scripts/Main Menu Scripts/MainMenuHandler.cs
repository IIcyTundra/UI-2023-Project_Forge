using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
   public GameObject currentPanel;
    public GameObject nextPanel;
    public float delayTime = 1.5f; // Adjust this to your desired delay time
    public AudioClip buttonClickSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        StartCoroutine(SwitchPanelsWithDelay());
    }

    IEnumerator SwitchPanelsWithDelay()
    {
        // Check if AudioSource is assigned
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            yield break;
        }

        // Check if AudioClip is assigned to the AudioSource
        if (buttonClickSound == null)
        {
            Debug.LogError("AudioClip is not assigned to the AudioSource!");
            yield break;
        }

        // Play sound effect
        audioSource.PlayOneShot(buttonClickSound);

        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Disable the current panel and enable the next one
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        if (nextPanel != null)
        {
            nextPanel.SetActive(true);
        }
    }

}
