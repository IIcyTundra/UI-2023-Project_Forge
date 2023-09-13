
using UnityEngine;

[CreateAssetMenu(fileName = "UncommonItem", menuName = "Item")]
public class UncommonItem : Item
{
    private void Awake()
    {
        rarity = ItemRarity.Uncommon;
    }
}
