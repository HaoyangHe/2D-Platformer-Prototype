using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetItemKey : Collectable
{
    public override void OnHandlePickupItem()
    {
        base.OnHandlePickupItem();
        GameManager.Instance.eventSystem.PresetItemKeyPickUpTrigger(itemData);
    }
}
