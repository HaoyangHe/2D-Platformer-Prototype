using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewPlayer : PhysicsObject
{    
    [Header("Attributes")]
    public int attackPower = 10;
    [SerializeField] private float maxSpeed = 5.0f;  
    [SerializeField] private float jumpPower = 10.0f; 
    [SerializeField] private float attachDuration = 0.1f;
    [SerializeField] private int maxHealth = 100;

    [Header("Reference")]
    [SerializeField] private GameObject attackBox;
    [SerializeField] private Sprite inventoryBlank;
    [SerializeField] private string inventoryBlankName;
    private Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    private int ammo;
    private int health;
    private int coinsCollected;
    private int facingDirection = 1;
    
    // Singleton instantation
    private static NewPlayer instance;
    public static NewPlayer Instance => instance;

    #region Unity Callback Funtions
    private void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SetSpawnLocation();

        health = maxHealth;
        
        inventory.Add(inventoryBlankName, inventoryBlank);
        UpdateUI(inventoryBlankName);
    }

    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0) * maxSpeed;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = jumpPower;

        if (targetVelocity.x * facingDirection < 0)
            Flip();

        if (Input.GetButtonDown("Fire1"))
            StartCoroutine(ActivateAttack());
    }
    #endregion

    #region Other
    private void UpdateUI(string inventoryItemName = "")
    {
        
    }

    private IEnumerator ActivateAttack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(attachDuration);
        attackBox.SetActive(false);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Die()
    {
        SceneManager.LoadScene("Level1");
        Init();
        SetSpawnLocation();
    }
    #endregion

    #region Callbacks
    public void Init()
    {
        health = maxHealth;
        coinsCollected = 0;
        inventory.Clear();
        inventory.Add(inventoryBlankName, inventoryBlank);
        UpdateUI(inventoryBlankName);
    }

    public void SetSpawnLocation()
    {
        transform.position = GameObject.Find("Spawn Location").transform.position;
    }

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

    public void Damage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        UpdateUI();
        if (health == 0)
        {
            Die();
        }
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