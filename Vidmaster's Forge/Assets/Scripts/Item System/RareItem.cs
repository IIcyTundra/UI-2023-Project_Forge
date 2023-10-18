using UnityEngine;

[CreateAssetMenu(fileName = "New Rare Item", menuName = "Items/RareItem")]
public class RareItem : Item
{   
    private Color32 rarityColor = Color.yellow;
    private readonly ItemRarity itemRarity = ItemRarity.Rare;
    
    public override string GetItemDescription()
    {
        string statString = "";
        for(int i = 0; i < ItemStats.Count; i++)
        {
            statString += $"\n{ItemStats[i].StatModifier.ToString()} {ItemStats[i].StatToModify.name}";
        }

        return $"Rarity: {itemRarity} \nCost: {ItemCost} \n\nEffects: {statString}";
    }

    public override ItemRarity GetItemRarity(){ return itemRarity; }

    public override Color32 GetRarityColor(){ return rarityColor; }
}
