using UnityEngine;

public class TopDownPlayer : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask obstacleLayer;

    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);
    }

    void Move(Vector2 direction)
    {
        Vector2 pos = transform.position;

        // Move X
        Vector2 newPosX = pos + new Vector2(direction.x, 0) * speed * Time.deltaTime;

        if (!Physics2D.OverlapBox(newPosX, col.bounds.size * 0.9f, 0f, obstacleLayer))
        {
            pos = newPosX;
        }

        // Move Y
        Vector2 newPosY = pos + new Vector2(0, direction.y) * speed * Time.deltaTime;

        if (!Physics2D.OverlapBox(newPosY, col.bounds.size * 0.9f, 0f, obstacleLayer))
        {
            pos = newPosY;
        }

        transform.position = pos;
    }
}