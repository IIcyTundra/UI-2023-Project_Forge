using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleHoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshPro;

    public Color hoverColor = Color.red;
    private Color originalColor;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        originalColor = textMeshPro.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change text color when the pointer enters
        textMeshPro.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert to the original text color when the pointer exits
        textMeshPro.color = originalColor;
    }
}