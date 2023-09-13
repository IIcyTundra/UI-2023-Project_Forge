
using UnityEngine;

[CreateAssetMenu(fileName = "UncommonItem", menuName = "Items/Uncommon Item")]
public class UncommonItem : Item
{
    private void Awake()
    {
        rarity = ItemRarity.Uncommon;
    }
}
