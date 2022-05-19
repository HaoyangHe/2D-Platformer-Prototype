using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public UIController uIController;
    public EventSystem eventSystem;
    public InventorySystem inventorySystem;
    public AudioManager audioManager;
    public PresetPlayer player;
    
    private static GameManager instance;
    public static GameManager Instance => instance;

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

        DontDestroyOnLoad(gameObject);

        if (uIController == null && FindObjectOfType<UIController>())
        {
            uIController = FindObjectOfType<UIController>();
        }    

        if (eventSystem == null && FindObjectOfType<EventSystem>())
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        if (inventorySystem == null && FindObjectOfType<InventorySystem>())
        {
            inventorySystem = FindObjectOfType<InventorySystem>();
        }

        if (audioManager == null && FindObjectOfType<AudioManager>())
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        if (player == null && FindObjectOfType<PresetPlayer>())
        {
            player = FindObjectOfType<PresetPlayer>();
        }
    }
}
