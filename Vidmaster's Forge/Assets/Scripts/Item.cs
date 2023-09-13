
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public class Item : ScriptableObject
{
    public ItemRarity rarity;
    public string ItemName;
    public List<ItemStats> Stats = new List<ItemStats>();

    private int numStatsToAdd;
    public virtual void GenerateStats()
    {

        // Clear existing Stats
        Stats.Clear();

        // Add Stats based on rarity
        switch (rarity)
        {
            case ItemRarity.Common: numStatsToAdd = 1; break;
            case ItemRarity.Uncommon: numStatsToAdd = 2; break;
            case ItemRarity.Rare: numStatsToAdd = 3; break;
            case ItemRarity.Legendary: numStatsToAdd = 4; break;
        }

        for (int i = 0; i < numStatsToAdd; i++)
        {
            Stats.Add(new ItemStats());
        }
    }
}




[System.Serializable]
public class ItemStats
{
    public float StatModifier;
}
