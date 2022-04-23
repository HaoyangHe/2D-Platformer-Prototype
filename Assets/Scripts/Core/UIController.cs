using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Holds UI component references & callback funcions
public class UIController : MonoBehaviour
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
        
        healthBarOriginSize = healthBar.rectTransform.sizeDelta;    
    }

    private void Start() 
    {
        Init();  

        // Subscribe to events. 
        GameManager.Instance.eventSystem.onSceneBegin += Init;
        GameManager.Instance.eventSystem.onPresetGateOpen += ClearSprite;
        GameManager.Instance.eventSystem.onPresetPlayerDying += InitPlayerRelated;
    }

    private void OnDestroy() 
    {
        // Make sure to unsubscribe.
        GameManager.Instance.eventSystem.onSceneBegin -= Init;
        GameManager.Instance.eventSystem.onPresetGateOpen -= ClearSprite;
        GameManager.Instance.eventSystem.onPresetPlayerDying -= InitPlayerRelated;
    }

    public void Init()
    {
        InitPlayerRelated();
        InitInventoryRelated();
    }

    private void InitPlayerRelated()
    {
        coinsText.text = GameManager.Instance.player.coinsCount.ToString();
        workSpace.Set(healthBarOriginSize.x * ((float)GameManager.Instance.player.startHP / GameManager.Instance.player.maxHP),
                                              healthBarOriginSize.y);
        healthBar.rectTransform.sizeDelta = workSpace;
    }

    private void InitInventoryRelated()
    {
        inventoryItem.sprite = inventoryBlank;
    }

    #region Callback Functions 
    public void SetHealth(int value = 10)
    {
        workSpace.Set(healthBarOriginSize.x * ((float)value / GameManager.Instance.player.maxHP),
                                              healthBarOriginSize.y);
        healthBar.rectTransform.sizeDelta = workSpace;
    }

    public void SetCoin(int value = 1)
    {
        coinsText.text = value.ToString();
    }

    public void SetSprite(InventoryItemData item)
    {
        inventoryItem.sprite = item.icon;
    }

    public void ClearSprite()
    {
        inventoryItem.sprite = inventoryBlank;
    }
    #endregion
}
