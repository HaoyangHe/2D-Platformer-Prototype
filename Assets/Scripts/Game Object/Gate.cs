using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string requiredInventoryName;
    
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (NewPlayer.Instance.HasInventoryItem(requiredInventoryName))
            {
                NewPlayer.Instance.RemoveInventoryItem(requiredInventoryName);
                OpenGate();
            }
        }
    }

    private void OpenGate()
    {
        Destroy(gameObject);
    }
}
