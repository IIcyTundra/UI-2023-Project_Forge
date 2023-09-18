using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Common Item", menuName = "Items/CommonItem")]
public class CommonItem : Item
{
    private Color32 rarityColor = Color.white;
    private readonly ItemRarity itemRarity = ItemRarity.Common;


    //Temporarily the same for every item rarity for testing purposes, eventually the plan is to have stats have color and more
    public override string GetItemDescription()
    {
        string statString = "";
        for(int i = 0; i < ItemStats.Count; i++)
        {
            statString += $"\n{ItemStats[i].StatModifier.ToString()} {ItemStats[i].StatToModify}";
        }

        return $"Rarity: {itemRarity} \nCost: {ItemCost} \n\nEffects: {statString}";
    }

    public override ItemRarity GetItemRarity(){ return itemRarity; }

    public override Color32 GetRarityColor(){ return rarityColor; }
}
