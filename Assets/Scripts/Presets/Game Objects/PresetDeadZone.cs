using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetDeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        { 
            GameManager.Instance.eventSystem.PresetPlayerDyingTrigger();
        }
    }
}
