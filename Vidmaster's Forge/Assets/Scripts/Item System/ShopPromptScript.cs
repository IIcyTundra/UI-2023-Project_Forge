using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPromptScript : MonoBehaviour
{
    public GameObject textToShow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textToShow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textToShow.SetActive(false);
        }
    }
}
