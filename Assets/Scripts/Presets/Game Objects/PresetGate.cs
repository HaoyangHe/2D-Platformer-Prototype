using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetGate : MonoBehaviour
{
    [SerializeField] private InventoryItemData requiredInventoryItem;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            if (GameManager.Instance.inventorySystem.Get(requiredInventoryItem) != null)
            {
                GameManager.Instance.eventSystem.PresetGateOpenTrigger();
                GameManager.Instance.inventorySystem.Remove(requiredInventoryItem);
                OpenGate();
            }
        }
    }

    private void OpenGate()
    {
        Destroy(gameObject);
    }
}
