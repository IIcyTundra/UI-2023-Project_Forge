using UnityEngine;

[CreateAssetMenu(fileName = "New Legendary Item", menuName = "Items/LegendaryItem")]
public class LegendaryItem : Item
{
    private Color32 rarityColor = new(255, 128, 0, 255);
    private readonly ItemRarity itemRarity = ItemRarity.Legendary;

    public override string GetItemDescription()
    {
        string statString = "";
        for(int i = 0; i < ItemStats.Count; i++)
        {
            if(ItemStats[i].StatModifier >= 0){
                statString += $"\n+{ItemStats[i].StatModifier.ToString()} {ItemStats[i].StatToModify.name}";
            }else{
                statString += $"\n{ItemStats[i].StatModifier.ToString()} {ItemStats[i].StatToModify.name}";
            }
        }

        return $"Rarity: {itemRarity} \nCost: {ItemCost} \n\nEffects: {statString}";
    }

    public override ItemRarity GetItemRarity(){ return itemRarity; }

    public override Color32 GetRarityColor(){ return new(255, 128, 0, 255); }
}
