using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeChaser : MonoBehaviour
{
    public Transform player;

    public float speed = 7f;
    public float slowedSpeed = 3f;
    public float slowDuration = 1f;

    public static bool gameWon = false;

    float slowTimer;
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        gameWon = false;
    }

    void Update()
    {
        if (player == null || gameWon) return;

        // restart if bee catches player
        if (Vector2.Distance(transform.position, player.position) < 0.5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        float currentSpeed = (slowTimer > 0) ? slowedSpeed : speed;

        if (slowTimer > 0)
            slowTimer -= Time.deltaTime;

        Move(direction, currentSpeed);
    }

    void Move(Vector2 direction, float moveSpeed)
    {
        Vector2 pos = transform.position;

        Vector2 newPosX = pos + new Vector2(direction.x, 0) * moveSpeed * Time.deltaTime;

        if (!Physics2D.OverlapBox(newPosX, col.bounds.size * 0.9f, 0f, LayerMask.GetMask("Obstacle")))
        {
            pos = newPosX;
        }
        else
        {
            slowTimer = slowDuration;
        }

        Vector2 newPosY = pos + new Vector2(0, direction.y) * moveSpeed * Time.deltaTime;

        if (!Physics2D.OverlapBox(newPosY, col.bounds.size * 0.9f, 0f, LayerMask.GetMask("Obstacle")))
        {
            pos = newPosY;
        }
        else
        {
            slowTimer = slowDuration;
        }

        transform.position = pos;
    }
}