using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implement how Player react when picking up items
public class PresetPickUp : PickUpCallbacks
{
    private PresetPlayer player;

    public PresetPickUp()
    {
        player = GameManager.Instance.player;
    }

    public void AddHealth(int value = 10)
    {
        player.healthPoint = Mathf.Clamp(player.healthPoint + value, 0, player.maxHP);
        GameManager.Instance.uIController.AddHealth(player.healthPoint);
    }

    public void AddCoin(int value = 1)
    {
        player.coinsCount += value;
        GameManager.Instance.uIController.AddCoin(player.coinsCount);
    }

    public void PickUpKey(InventoryItemData item)
    {
        GameManager.Instance.uIController.PickUpKey(item);
    }

    public void UseKey()
    {
        GameManager.Instance.uIController.UseKey();
    }
}
