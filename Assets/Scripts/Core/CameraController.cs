using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    void Start()
    {
        cinemachineVirtualCamera.Follow = GameManager.Instance.player.transform;
    }
}
