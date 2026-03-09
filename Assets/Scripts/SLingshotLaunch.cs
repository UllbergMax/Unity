using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SlingshotLaunch : MonoBehaviour
{
    public float forceMultiplier = 10f;

    public TextMeshProUGUI powerText;
    public TextMeshProUGUI angleText;

    Rigidbody2D rb;

    Vector2 startMousePos;
    bool dragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Keep player still until launch
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(mouseWorld, transform.position) < 1f)
            {
                dragging = true;
                startMousePos = mouseWorld;
            }
        }

        // While dragging → calculate power and angle
        if (dragging)
        {
            Vector2 dragVector = startMousePos - mouseWorld;

            float power = dragVector.magnitude * forceMultiplier;

            float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;

            powerText.text = "Power: " + power.ToString("F0");
            angleText.text = "Angle: " + angle.ToString("F0");
        }

        // Release → launch
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;

            Vector2 dragVector = startMousePos - mouseWorld;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(dragVector * forceMultiplier, ForceMode2D.Impulse);
        }

        // Restart if player falls off screen
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}