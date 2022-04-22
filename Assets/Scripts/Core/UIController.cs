using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Holds UI component references & callback funcions
public class UIController : MonoBehaviour, PickUpCallbacks
{
    [Header("UI references")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image inventoryItem;
    [SerializeField] private Sprite inventoryBlank;

    private Vector2 healthBarOriginSize;
    private Vector2 workSpace;

    private static UIController instance;           // Using GameManager.Instance.uIController as far as posible is recommended.
    public static UIController Instance => instance;

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

    private void Start() 
    {
        healthBarOriginSize = healthBar.rectTransform.sizeDelta;
        Init();
    }

    private void Init()
    {
        coinsText.text = GameManager.Instance.player.coinsCount.ToString();
        workSpace.Set(healthBarOriginSize.x * ((float)GameManager.Instance.player.startHP / GameManager.Instance.player.maxHP),
                                              healthBarOriginSize.y);
        healthBar.rectTransform.sizeDelta = workSpace;
        inventoryItem.sprite = inventoryBlank;
    }

    #region Callback Functions 
    public void AddHealth(int value = 10)
    {
        workSpace.Set(healthBarOriginSize.x * ((float)value / GameManager.Instance.player.maxHP),
                                              healthBarOriginSize.y);
        healthBar.rectTransform.sizeDelta = workSpace;
    }

    public void AddCoin(int value = 1)
    {
        coinsText.text = value.ToString();
    }

    public void PickUpKey(InventoryItemData item)
    {
        inventoryItem.sprite = item.icon;
    }

    public void UseKey()
    {
        inventoryItem.sprite = inventoryBlank;
    }
    #endregion
}
