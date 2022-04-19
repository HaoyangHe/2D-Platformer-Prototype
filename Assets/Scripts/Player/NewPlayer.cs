using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : PhysicsObject
{    
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float jumpPower = 10.0f;
    [SerializeField] private int maxHealth = 100;

    // UI
    [SerializeField] private Text coinsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image inventoryItem;
    [SerializeField] private Sprite inventoryBlank;
    [SerializeField] private string inventoryBlankName;
    private Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    private Vector2 healthBarOriginSize;

    private int ammo;
    private int health;
    private int coinsCollected;

    // Singleton instantation
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get 
        {
            if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
            return instance;
        }
    }

    #region Unity Callback Funtions
    private void Start()
    {
        health = maxHealth;
        healthBarOriginSize = healthBar.rectTransform.sizeDelta;
        inventory.Add(inventoryBlankName, inventoryBlank);
        UpdateUI(inventoryBlankName);
    }

    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0) * maxSpeed;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = jumpPower;
    }
    #endregion

    #region Other
    private void UpdateUI(string inventoryItemName = "")
    {
        coinsText.text = coinsCollected.ToString();
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOriginSize.x * ((float)health / maxHealth), healthBarOriginSize.y);
        
        if (inventoryItemName != "")
        {
            inventoryItem.sprite = inventory[inventoryItemName];
        }
    }
    #endregion

    #region Callbacks
    public void AddCoin()
    { 
        coinsCollected++;
        UpdateUI();
    }

    public void AddHealth(int addHealth)
    {
        health = Mathf.Clamp(health + addHealth, 0, maxHealth);
        UpdateUI();
    }

    public void AddInventoryItem(string inventoryItemName, Sprite image)
    {
        inventory.Add(inventoryItemName, image);
        UpdateUI(inventoryItemName);
    }

    public void RemoveInventoryItem(string inventoryItemName)
    {
        inventory.Remove(inventoryItemName);
        UpdateUI(inventoryBlankName);
    }

    public bool HasInventoryItem(string inventoryItemName)
    {
        return inventory.ContainsKey(inventoryItemName);
    }
    #endregion
}