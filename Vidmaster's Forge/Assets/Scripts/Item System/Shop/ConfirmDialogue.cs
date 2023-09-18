using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI confirmationDialogue, staticPurchaseText, creditsRemaining;
    
    public enum ConfirmationType
    {
        CloseShop,
        RefreshShop,
        PurchaseItem
    }

    public ConfirmationType currentConfirmationType;
    public event Action OnConfirmEvent;
    public event Action OnCancelEvent;
    public bool userConfirmed = false;

    // Call this method to show the confirmation dialog
    public void ShowConfirmationDialog()
    {
        staticPurchaseText.gameObject.SetActive(true);
        creditsRemaining.gameObject.SetActive(true);
        // Set the confirmation message based on the current type
        switch (currentConfirmationType)
        {
            case ConfirmationType.CloseShop:
                confirmationDialogue.SetText("Are you sure you want to close the shop?");
                staticPurchaseText.gameObject.SetActive(false);
                creditsRemaining.gameObject.SetActive(false);
                break;
            case ConfirmationType.RefreshShop:
                confirmationDialogue.SetText("Are you sure you want to refresh the shop?");
                break;
            case ConfirmationType.PurchaseItem:
                confirmationDialogue.SetText("Are you sure you want to purchase this item?");
                break;
            default:
                confirmationDialogue.SetText("Are you sure?");  
                break;
        }
    }

    // Call this method when the user confirms
    public void Confirm()
    {
        userConfirmed = true;
        OnConfirmEvent?.Invoke(); // Trigger the confirmation event
        gameObject.SetActive(false); // Hide the confirmation dialog
        
    }

    // Call this method when the user cancels
    public void Cancel()
    {
        userConfirmed = false;
        OnCancelEvent?.Invoke(); // Trigger the cancellation event
        gameObject.SetActive(false); // Hide the confirmation dialog
        
    }
}
