using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene5Player : MonoBehaviour
{
    public Transform launchPoint;
    public Transform platform;

    public float launchPower = 6f;
    public float maxDragDistance = 2f;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultText2;

    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool launched = false;
    private bool missionComplete = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = launchPoint.position;

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (resultText2 != null)
            resultText2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!launched)
            DragAndLaunch();

        if (launched && !missionComplete)
        {
            // if bird falls off platform
            if (transform.position.y < platform.position.y - 3f)
            {
                RestartScene();
            }
        }
    }

    void FixedUpdate()
    {
        // check if bird stopped on platform
        if (launched && !missionComplete)
        {
            if (rb.linearVelocity.magnitude < 0.1f && transform.position.y > platform.position.y)
            {
                missionComplete = true;

                if (resultText != null)
                {
                    resultText.gameObject.SetActive(true);
                    resultText.text = "ALL SCENES COMPLETED!!";
                }

                if (resultText2 != null)
                {
                    resultText2.gameObject.SetActive(true);
                    resultText2.text = "Made by Max & Luca";
                }
            }
        }
    }

    void DragAndLaunch()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(mouse, transform.position) < 1f)
                isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 drag = mouse - (Vector2)launchPoint.position;

            if (drag.magnitude > maxDragDistance)
                drag = drag.normalized * maxDragDistance;

            transform.position = (Vector2)launchPoint.position + drag;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            launched = true;

            rb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 direction = (Vector2)launchPoint.position - (Vector2)transform.position;

            rb.linearVelocity = direction * launchPower;
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}