using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject confirmationBox;
    [SerializeField] private GameObject highlighter;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private List<ShopListing> itemListings;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemDescriptionName;
    [SerializeField] private ConfirmDialogue confirmDialogue;

    public Item selectedItem;
    private int selectedSlotNumber = -1;

    private void Start()
    {
        // Subscribe to the dialog's events
        confirmDialogue.OnConfirmEvent += HandleConfirm;
        confirmDialogue.OnCancelEvent += HandleCancel;
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
            if(shopWindow.activeInHierarchy){ GenerateShopContents(); }
        }
    }

    //generates and populates 6 new listings of items
    public void GenerateShopContents()
    {
        for (int i = 0; i < itemListings.Count(); i++)
        {
            itemListings[i].CreateNewListing(GetRandomItem());
        }
    }

    //Function to generate random items based on rarity. Need to implement a check if item is in inventory or if item is already in shop, and if so, generate a new item
    public Item GetRandomItem()
    {
        ItemRarity rarity = GetRandomRarity();
        var itemsOfRarity = itemDatabase.items.Where(item => item.itemData.GetItemRarity() == rarity).ToList();
        if (itemsOfRarity.Count > 0)
        {
            int randomIndex = Random.Range(0, itemsOfRarity.Count);
            return itemsOfRarity[randomIndex].itemData;
        }
        else
        {
            Debug.LogWarning("No items of the specified rarity found.");
            return null;
        }
    }

    //Temporary weighted Item Rarity generator until I find better way to do so
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

    //Clears the Description Panel on the right hand side, can also be used to unselect
    private void ClearDescriptionPanel()
    {
        itemDescription.SetText("");
        itemDescriptionName.color = Color.black;
        itemDescriptionName.SetText("");
        selectedItem = null;
        
        selectedSlotNumber = 0;
        highlighter.SetActive(false);
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
        itemDescriptionName.SetText(selectedItem.ItemName);
        
        itemDescriptionName.color = selectedItem.GetRarityColor();;
        itemDescription.SetText(selectedItem.GetItemDescription());
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
                break;

            case ConfirmDialogue.ConfirmationType.PurchaseItem:
                // Handle purchase item action here
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
