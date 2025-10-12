using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -speed;
        }
        else rb.linearVelocityX = 0f;

    }
}
