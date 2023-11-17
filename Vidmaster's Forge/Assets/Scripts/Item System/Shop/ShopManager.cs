using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Hertzole.ScriptableValues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopWindow, highlighter;
    [SerializeField] private Button purchaseButton, resetButton;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private List<ShopListing> itemListings;
    [SerializeField] private TextMeshProUGUI itemDescription, itemDescriptionName, creditText;
    [SerializeField] private ConfirmDialogue confirmDialogue;
    [SerializeField] private ScriptableInt playerCredits;
    [HideInInspector] public Item selectedItem;
    private int selectedSlotNumber = -1, refreshCost = 90;
    private List<Item> shopContents = new List<Item>();
    
    
    private void Start()
    {
        // Subscribe to the dialog's events
        confirmDialogue.OnConfirmEvent += HandleConfirm;
        confirmDialogue.OnCancelEvent += HandleCancel;

        RefreshBalance();
    }
    private void Update()
    {
        //TEMPORARY INPUT KEYS FOR TESTING PURPOSES
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Resetting Shop");
            GenerateShopContents();
        }else if (Input.GetKeyDown(KeyCode.J)){
            shopWindow.SetActive(!shopWindow.activeInHierarchy);
            if (shopWindow.activeInHierarchy) { GenerateShopContents(); }
        }
    }

    //generates and populates 6 new listings of items
    public void GenerateShopContents()
    {
        shopContents.Clear();
        GetRandomItems();
        
        for (int i = 0; i < itemListings.Count(); i++)
        {
            itemListings[i].CreateNewListing(shopContents[i]);
        }
    }

    // Function to generate random items based on rarity. Need to implement a check if item is in inventory or if item is already in shop, and if so, generate a new item
    public void GetRandomItems()
    {
        for (int i = 0; i < itemListings.Count(); i++)
        {
            ItemRarity rarity = GetRandomRarity();
            var itemsOfRarity = itemDatabase.items.Where(item => item.itemData.GetItemRarity() == rarity).ToList();
            if (itemsOfRarity.Count > 0)
            {
                int randomIndex = Random.Range(0, itemsOfRarity.Count);
                shopContents.Add(itemsOfRarity[randomIndex].itemData);
            }
        }
        shopContents = shopContents.OrderBy(item => item.GetItemRarity()).ToList();
    }

    // Temporary weighted Item Rarity generator until I find better way to do so
    private ItemRarity GetRandomRarity()
    { 
        float rarityRoll = Random.Range(0f, 1f);
        if(rarityRoll <= 0.50){ //50 % chance for common
            return ItemRarity.Common;
        }else if (rarityRoll <= 0.75){ //25 % chance for uncommon
            return ItemRarity.Uncommon;
        }else if (rarityRoll <= .90){ //15 % chance for rare
            return ItemRarity.Rare;
        }else{ //remaining 10% chance for legendary
            return ItemRarity.Legendary;
        }
    }

    // Clears the Description Panel on the right hand side, can also be used to unselect
    private void ClearDescriptionPanel()
    {
        itemDescription.SetText("");
        itemDescriptionName.color = Color.black;
        itemDescriptionName.SetText("");
        selectedItem = null;
        purchaseButton.interactable = true;
        purchaseButton.gameObject.SetActive(false);
        selectedSlotNumber = 0;
        highlighter.SetActive(false);
    }

    // Returns the summery of the transaction, isPurchase is true for purchasing, false for refreshing shop
    public string GetTransactionSummery(bool isPurchase)
    {
        int total = 0;
        if(isPurchase){
            total = selectedItem.ItemCost;
        }else{
            total = refreshCost;
        }
        return $"  {playerCredits.Value}\n- {total}\n  {playerCredits.Value-total}";
    }

    //Selects item when clicked, currently uses buttons, not sure what best method is
    public void SelectItemListing(int slotNum)
    {
        
        //Check if slot clicked is already the selected slot, if so, deselect it, else select the slot and set the selectedSlotNum to the slotNum of clicked slot
        if(selectedSlotNumber == slotNum){ ClearDescriptionPanel(); return; }
        selectedSlotNumber = slotNum;

        //Moves the higlighter to the selected item and sets the selected item to the item clicked
        MoveHighlighter(slotNum);
        selectedItem = itemListings[slotNum-1].l_currItemData; 

        //Populates the description panel
        Debug.Log(selectedItem);
        itemDescriptionName.SetText(selectedItem.ItemName);
        
        itemDescriptionName.color = selectedItem.GetRarityColor();;
        itemDescription.SetText(selectedItem.GetItemDescription());
        purchaseButton.gameObject.SetActive(true);
        if (selectedItem.ItemCost > playerCredits.Value){
            purchaseButton.interactable = false;
        }
    }
    private void PurchaseSelectedItem()
    {
        playerCredits.Value-=selectedItem.ItemCost;
        RefreshBalance();
        ClearDescriptionPanel();
        // Need to remove the listing
        // Need to give the player an item, may be good to use the 
    }

    // Refreshes the balance text and if the bal is < the cost, greys out the refresh button
    private void RefreshBalance()
    {
        creditText.SetText(playerCredits.Value.ToString());
        if (refreshCost > playerCredits.Value)
        {
            resetButton.interactable = false;
        }
    }

    //Moves Item Higlighter to the passed slotNumber
    private void MoveHighlighter(int slotNum)
    {
        highlighter.SetActive(true);
        highlighter.transform.position = itemListings[slotNum-1].transform.position;
    }

    /// <summary>
    /// The following methods are all to handle a confirmation dialogue pop up any time an item is purchased, shop is closed, or refresh is pressed.
    /// This can also be expanded on but for now that is the only purposes
    /// </summary>
    private void HandleConfirm()
    {
        //If the dialogue is confirmed then check the type and perform any functions needed
        switch (confirmDialogue.currentConfirmationType)
        {
            case ConfirmDialogue.ConfirmationType.CloseShop:
                ClearDescriptionPanel();
                for (int i = 0; i < itemListings.Count; i++)
                {
                    itemListings[i].ClearListing();
                }
                shopWindow.SetActive(!shopWindow.activeInHierarchy);
                break;

            case ConfirmDialogue.ConfirmationType.RefreshShop:
                ClearDescriptionPanel();
                GenerateShopContents();
                playerCredits.Value-=refreshCost;
                RefreshBalance();
                break;

            case ConfirmDialogue.ConfirmationType.PurchaseItem:
                // Handle purchase item action here
                PurchaseSelectedItem();
                break;

            default:
                Debug.LogWarning("Unhandled confirmation type.");
                break;
        }

        // Hide the confirmation dialog
        confirmDialogue.gameObject.SetActive(false);
    }

    private void HandleCancel()
    {
        // Handle cancellation if needed
        // This code will execute if the user cancels

        // Hide the confirmation dialog
        confirmDialogue.gameObject.SetActive(false);
    }

    // Close shop button which prompts the user for confirmation
    public void CloseShop()
    {
        // Set the current confirmation type
        confirmDialogue.currentConfirmationType = ConfirmDialogue.ConfirmationType.CloseShop;
        confirmDialogue.ShowConfirmationDialog();
        // Show the confirmation dialog
        confirmDialogue.gameObject.SetActive(true);
    }

    // When the refresh shop button is clicked, prompt for confirmation before continuing
    public void RefreshShop()
    {
        // Set the current confirmation type
        confirmDialogue.currentConfirmationType = ConfirmDialogue.ConfirmationType.RefreshShop;
        confirmDialogue.ShowConfirmationDialog();

        // Show the confirmation dialog
        confirmDialogue.gameObject.SetActive(true);
    }

    // To be called when an item is purchased
    public void PurchaseItem()
    {
        // Set the current confirmation type
        confirmDialogue.currentConfirmationType = ConfirmDialogue.ConfirmationType.PurchaseItem;
        confirmDialogue.ShowConfirmationDialog();
        // Show the confirmation dialog
        confirmDialogue.gameObject.SetActive(true);
    }

   

}
