using UnityEngine;

[CreateAssetMenu(fileName = "New Uncommon Item", menuName = "Items/UncommonItem")]
public class UncommonItem : Item
{
    private Color32 rarityColor = Color.green;
    private readonly ItemRarity itemRarity = ItemRarity.Uncommon;

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
