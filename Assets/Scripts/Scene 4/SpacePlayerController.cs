using UnityEngine;

public class SpacePlayerController : MonoBehaviour
{
    public float thrust = 8f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(moveX, moveY);

        rb.AddForce(input * thrust);
    }
}