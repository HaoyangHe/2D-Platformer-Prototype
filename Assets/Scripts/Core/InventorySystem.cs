using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory;           // Allows adding InventoryItems from the inspector.      
    
    private static InventorySystem instance;        // Using GameManager.Instance.inventorySystem as far as posible is recommended.
    public static InventorySystem Instance => instance;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
        
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();

        if (inventory == null)
        {
            inventory = new List<InventoryItem>();
        }
        else 
        {
            foreach(InventoryItem item in inventory)
            {
                if (!itemDictionary.ContainsKey(item.data))
                {
                    itemDictionary.Add(item.data, item);
                }
                else 
                {
                    Debug.LogError("Please make sure InventoryItem: " + item.data.id + " is unique in inventory.");
                }
            }
        }
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem item))
        {
            return item;
        }
        return null;
    }

    public void Add(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem item))
        {
            item.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            itemDictionary.Add(referenceData, newItem);
            inventory.Add(newItem);
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem item))
        {
            item.RemoveFromStack();

            if (item.stackSize == 0)
            {
                itemDictionary.Remove(referenceData);
                inventory.Remove(item);
            }
        }
    }
}
