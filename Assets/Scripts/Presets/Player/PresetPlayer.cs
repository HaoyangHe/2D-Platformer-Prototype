using System.Collections.Generic;
using UnityEngine;

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
    [System.NonSerialized] public PresetPickUp pickUpCallbacks;

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

        Init();
    }

    private void Start() 
    {
        pickUpCallbacks = new PresetPickUp();       // Make sure NOT to put this line of code in Awake().
    }
    
    private void Init()
    {
        healthPoint = startHP;
        coinsCount = originalCoins;
    }
}
