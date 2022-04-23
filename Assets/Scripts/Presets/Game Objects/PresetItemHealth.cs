using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetItemHealth : Collectable
{
    [Header("Item Attributes")]
    public int addPower = 10;
    
    public override void OnHandlePickupItem()
    {
        GameManager.Instance.eventSystem.PresetItemHealthPickUpTrigger(addPower);
    }
}
