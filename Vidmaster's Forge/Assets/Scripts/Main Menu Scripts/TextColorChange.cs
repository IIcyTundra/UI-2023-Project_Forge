using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorChange : MonoBehaviour
{
    public Text returnText;
    private Color returnColor;
    private FontStyle originalColor;
    private int originalFontSize;
    private bool isSelected = false; 

    void Start()
    {
        returnColor = returnText.color;
        originalColor = returnText.fontStyle;   
        originalFontSize = returnText.fontSize;
    }


    public void changeWithHover() {
        if (!isSelected) {
            returnText.color = Color.gray;
            returnText.fontSize = 45;
        }
    }

    public void changeBack() {
        if (!isSelected) {
            returnText.color = returnColor;
            returnText.fontSize = originalFontSize;
        }
    }

    public void changeWithSelect() {
        isSelected = true;
        returnText.color = Color.white;
        returnText.fontSize = 45;
    }

    public void changeWithDeselect() {
        isSelected = false;
        returnText.color = returnColor;
        returnText.fontSize = originalFontSize;
    }

    public void DeselectAll()
    {
        isSelected = false;
        returnText.color = returnColor;
        returnText.fontSize = originalFontSize;
    }

}
