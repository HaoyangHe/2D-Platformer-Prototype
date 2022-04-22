using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetItemCoin : Collectable
{
    [Header("Item Attributes")]
    public int amount = 1;

    public override void OnHandlePickupItem()
    {
        GameManager.Instance.player.pickUpCallbacks.AddCoin(amount);
    }
}
