using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PresetPlayer : MonoBehaviour
{
    [Header("Attributes")]
    public int maxHP = 100;
    public int startHP = 50;
    public int originalCoins = 0;

    // Current Status
    [System.NonSerialized] public int healthPoint;
    [System.NonSerialized] public int coinsCount;
    
    // Components & References
    // An alternative choice for you to access callbacks else where rather than subscribe to the events via the EventSystem.
    [System.NonSerialized] public PlayerCallbacks callbacks;    

    private static PresetPlayer instance;           // Using GameManager.Instance.player as far as posible is recommended.
    public static PresetPlayer Instance => instance;
    
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
        
        LoadAttributes();
    }

    private void Start() 
    {
        if (gameObject.GetComponent<PlayerCallbacks>() != null)
        {
            callbacks = gameObject.GetComponent<PlayerCallbacks>();
        }
        else 
        {
            Debug.LogError("Player callbacks han't been set yet.");
        }
    }

    public void SaveAttributes()
    {
        startHP = healthPoint;
        originalCoins = coinsCount;
    }
    
    public void LoadAttributes()
    {
        healthPoint = startHP;
        coinsCount = originalCoins;
        SetSpawnLocation();
    }

    public void SetSpawnLocation()
    {
        transform.position = GameObject.Find("Spawn Location").transform.position;
    }
}
