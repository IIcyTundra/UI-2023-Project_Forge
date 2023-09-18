using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI l_itemName;
    [SerializeField] private TextMeshProUGUI l_itemCost;
    [SerializeField] private Image l_itemSprite;
    public Item l_currItemData;
    public bool isPurchased = false;

    //Clears current values and populates listing with itemdata passed in
    public void CreateNewListing(Item itemData) 
    {
        isPurchased = false;
        ClearListing();
        l_currItemData = itemData;
        l_itemCost.SetText(itemData.ItemCost.ToString());
        l_itemName.SetText(itemData.ItemName);
    }

    public void ClearListing()
    {
        l_itemName.SetText("");
        l_itemCost.SetText("");
        l_currItemData = null;
    }

    //Sets item to purchased and clears the listing
    public void PurchaseItem()
    {
        isPurchased = true;
        ClearListing();
    }
}
