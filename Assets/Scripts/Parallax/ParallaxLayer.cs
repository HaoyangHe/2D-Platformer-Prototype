using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(-1.0f, 1.0f)] 
    public float parallaxEffect = 0.0f;     // Moves in the same direction as the camera if parallaxEffect is greater than 0

    private Vector3 curPosition;

    public void MoveLayer(Vector2 positionChange)
    {
        curPosition = transform.localPosition;
        curPosition.x += positionChange.x * parallaxEffect;
        curPosition.y += positionChange.y * parallaxEffect;
        transform.localPosition = curPosition;
    }
}
