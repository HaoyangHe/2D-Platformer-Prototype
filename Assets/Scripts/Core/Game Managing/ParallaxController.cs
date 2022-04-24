using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform camTransform;
    private Vector2 oldCameraPosition;
    private ParallaxLayer[] parallaxLayers;

    private void Start()
    {
        camTransform = Camera.main.transform;
        oldCameraPosition = camTransform.position;
        parallaxLayers = transform.GetComponentsInChildren<ParallaxLayer>();
    }

    private void LateUpdate()
    {
        if (camTransform.position.x != oldCameraPosition.x || camTransform.position.y != oldCameraPosition.y)
        {
            Vector2 camPositionChange = (Vector2)camTransform.position - oldCameraPosition;
            MoveLayers(camPositionChange);
            oldCameraPosition = camTransform.position;
        }
    }
    
    void MoveLayers(Vector2 positionChange)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.MoveLayer(positionChange);
        }
    }
}
