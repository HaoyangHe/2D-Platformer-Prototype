using UnityEngine;

public class Collectable : MonoBehaviour 
{
    [Header("Item Datas")]
    public InventoryItemData itemData;
    
    public virtual void OnHandlePickupItem()
    {
        InventorySystem.Instance.Add(itemData);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            OnHandlePickupItem();
            Destroy(gameObject);
        }
    }
}
