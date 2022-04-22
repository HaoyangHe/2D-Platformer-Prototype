using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public UIController uIController;
    public InventorySystem inventorySystem;
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

        if (inventorySystem == null && FindObjectOfType<InventorySystem>())
        {
            inventorySystem = FindObjectOfType<InventorySystem>();
        }

        if (player == null && FindObjectOfType<PresetPlayer>())
        {
            player = FindObjectOfType<PresetPlayer>();
        }
    }
}
