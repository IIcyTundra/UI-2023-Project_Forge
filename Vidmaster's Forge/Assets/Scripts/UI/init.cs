using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class init : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<ViewManager>().PlayScene("MainMenu");
    }
}
