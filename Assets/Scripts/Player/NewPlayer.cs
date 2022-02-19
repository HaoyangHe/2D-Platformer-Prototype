using UnityEngine;

public class NewPlayer : PhysicsObject
{
    [SerializeField] private float maxSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0) * maxSpeed;

        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = 10;
    }
}
