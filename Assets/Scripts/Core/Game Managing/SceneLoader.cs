using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int loadSceneIndex;

    private void Start() 
    {
        GameManager.Instance.eventSystem.SceneBeginTrigger();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.eventSystem.SceneEndingTrigger();      // Make sure this line of code runs first.
            SceneManager.LoadScene(loadSceneIndex);
        }
    }
}
