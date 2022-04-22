using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    void Start()
    {
        cinemachineVirtualCamera.Follow = NewPlayer.Instance.transform;
    }
}
