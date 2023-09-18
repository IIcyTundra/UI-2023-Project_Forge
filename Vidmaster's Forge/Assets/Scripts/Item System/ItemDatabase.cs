using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Items/Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemGen<Item>[] items;
}