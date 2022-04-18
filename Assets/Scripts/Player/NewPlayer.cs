using UnityEngine;

public class NewPlayer : PhysicsObject
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float jumpPower = 10.0f;

    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0) * maxSpeed;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = jumpPower;
    }
}
