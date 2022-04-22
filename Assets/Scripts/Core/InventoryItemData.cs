using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Presets/Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
}
