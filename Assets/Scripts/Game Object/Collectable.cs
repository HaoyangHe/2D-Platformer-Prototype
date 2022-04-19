using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum ItemType { Coin, Health, Ammo, Inventory };
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private string inventoryName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            switch (itemType)
            { 
                case ItemType.Coin:
                    NewPlayer.Instance.AddCoin();
                    break;
                case ItemType.Health:
                    NewPlayer.Instance.AddHealth(10);
                    break;
                case ItemType.Ammo:
                    break;
                case ItemType.Inventory:
                    NewPlayer.Instance.AddInventoryItem(inventoryName, inventorySprite);
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
