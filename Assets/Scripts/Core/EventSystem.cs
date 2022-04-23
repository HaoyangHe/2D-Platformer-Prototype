using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour
{
    private static EventSystem instance;
    public static EventSystem Instance => instance;

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
    }
    
    // Add events like this:
    // public event Action onItemPickUp;
    // public void ItemPickUpTrigger()
    // {
    //     onItemPickUp?.();
    // }   
    // And subscribe to it using:
    // GameManager.Instance.eventSystem.onItemPickUp += FunctionName;

    // GameManaging events
    public event Action onSceneBegin;
    public void SceneBeginTrigger()
    {
        onSceneBegin?.Invoke();
    }

    public event Action onSceneEnding;
    public void SceneEndingTrigger()
    {
        onSceneEnding?.Invoke();
    }

    // Game object trigger events.
    public event Action<int> onPresetItemCoinPickUp;
    public void PresetItemCoinPickUpTrigger(int value)
    {
        onPresetItemCoinPickUp?.Invoke(value);
    }

    public event Action<int> onPresetItemHealthPickUp;
    public void PresetItemHealthPickUpTrigger(int value)
    {
        onPresetItemHealthPickUp?.Invoke(value);
    }

    public event Action<InventoryItemData> onPresetItemKeyPickUp;
    public void PresetItemKeyPickUpTrigger(InventoryItemData item)
    {
        onPresetItemKeyPickUp?.Invoke(item);
    }

    public event Action onPresetGateOpen;
    public void PresetGateOpenTrigger()
    {
        onPresetGateOpen?.Invoke();
    }

    public event Action onPresetPlayerDying;
    public void PresetPlayerDyingTrigger()
    {
        onPresetPlayerDying?.Invoke();
    }
}
