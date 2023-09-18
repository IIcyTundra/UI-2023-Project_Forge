using System.Collections.Generic;
using UnityEngine;

//Rarity Enum for items
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public abstract class Item : ScriptableObject
{
    [Header("Item Info")]
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private int itemCost;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private List<ItemStats> itemStats = new();
    
    public List<ItemStats> ItemStats => itemStats;
    public string ItemName => itemName;
    public int ItemCost => itemCost;
    public Sprite ItemIcon => itemIcon;
    public abstract string GetItemDescription();
    public abstract ItemRarity GetItemRarity();
    public abstract Color32 GetRarityColor();
    

    private int numStatsToAdd;
    public virtual void GenerateStats(){
        itemStats.Clear();
        switch (GetItemRarity())
        {
            case ItemRarity.Common: numStatsToAdd = 1; break;
            case ItemRarity.Uncommon: numStatsToAdd = 2; break;
            case ItemRarity.Rare: numStatsToAdd = 3; break;
            case ItemRarity.Legendary: numStatsToAdd = 4; break;
        }

        for (int i = 0; i < numStatsToAdd; i++)
        {
            itemStats.Add(new ItemStats());
        }
    }

}

[System.Serializable]
public class ItemGen<T> where T : Item
{
    public T itemData;
    public ItemGen(T data)
    {
        this.itemData = data;
    }
}

[System.Serializable]
public class ItemStats
{
    public bool isMultiplier = false; //If multiplier stat, true, if additive, false
    public float StatModifier;
    public string StatToModify;    //temporarily using a string to do this
}


