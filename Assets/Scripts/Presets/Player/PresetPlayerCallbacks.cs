using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implement how Player react when picking up items
public class PresetPlayerCallbacks : MonoBehaviour, PlayerCallbacks
{
    private PresetPlayer player;

    private void Start() 
    {
        player = GameManager.Instance.player;

        // Subscribe to events
        GameManager.Instance.eventSystem.onPresetItemCoinPickUp += AddCoin;
        GameManager.Instance.eventSystem.onPresetItemHealthPickUp += AddHealth;
        GameManager.Instance.eventSystem.onPresetItemKeyPickUp += PickUpKey;
        GameManager.Instance.eventSystem.onSceneBegin += OnSceneBegin;
        GameManager.Instance.eventSystem.onSceneEnding += OnSceneEnding;
    }

    private void OnDestroy() 
    {
        // Make sure to unsubscribe from EventSystem.
        GameManager.Instance.eventSystem.onPresetItemCoinPickUp -= AddCoin;
        GameManager.Instance.eventSystem.onPresetItemHealthPickUp -= AddHealth;
        GameManager.Instance.eventSystem.onPresetItemKeyPickUp -= PickUpKey;
        GameManager.Instance.eventSystem.onSceneBegin -= OnSceneBegin;
        GameManager.Instance.eventSystem.onSceneEnding -= OnSceneEnding;
    }

    // Game Managing event callbacks
    public void OnSceneBegin()
    {
        player.LoadAttributes();
    }

    public void OnSceneEnding()
    {
        player.SaveAttributes();
    }

    // Player event callbacks
    public void AddCoin(int value = 1)
    {
        player.coinsCount += value;
        GameManager.Instance.uIController.SetCoin(player.coinsCount);
    }

    public void AddHealth(int value = 10)
    {
        player.healthPoint = Mathf.Clamp(player.healthPoint + value, 0, player.maxHP);
        GameManager.Instance.uIController.SetHealth(player.healthPoint);
    }

    public void PickUpKey(InventoryItemData item)
    {
        GameManager.Instance.uIController.SetSprite(item);
    }

    public void Damage(int value = 10)
    {
        player.healthPoint = Mathf.Clamp(player.healthPoint - value, 0, player.maxHP);
        GameManager.Instance.uIController.SetHealth(player.healthPoint);

        if (player.healthPoint == 0)
        {
            player.LoadAttributes();
            GameManager.Instance.eventSystem.PresetPlayerDyingTrigger();
        }
    }
}
