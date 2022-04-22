using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private string loadSceneName;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == NewPlayer.Instance.gameObject)
        {
            SceneManager.LoadScene(loadSceneName);
            NewPlayer.Instance.SetSpawnLocation();
        }
    }
}
