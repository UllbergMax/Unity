using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene5Player : MonoBehaviour
{
    public Transform launchPoint;
    public Transform platform;

    public float launchPower = 6f;
    public float maxDragDistance = 2f;

    public float gravity = -20f;
    public float friction = 5f;
    public float birdRadius = 0.25f;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultText2;

    private Vector2 velocity;
    private Vector2 previousPosition;

    private bool isDragging = false;
    private bool launched = false;
    private bool landed = false;
    private bool missionComplete = false;

    void Start()
    {
        transform.position = launchPoint.position;

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (resultText2 != null)
            resultText2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!launched)
        {
            DragAndLaunch();
        }
        else if (!missionComplete)
        {
            ApplyPhysics();
            DetectPlatformCollision();
            CheckPlatformEdge();
            CheckFall();
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

            Vector2 direction = (Vector2)launchPoint.position - (Vector2)transform.position;
            velocity = direction * launchPower;
        }
    }

    void ApplyPhysics()
    {
        previousPosition = transform.position;

        if (!landed)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            if (velocity.x > 0)
                velocity.x -= friction * Time.deltaTime;
            else if (velocity.x < 0)
                velocity.x += friction * Time.deltaTime;

            if (Mathf.Abs(velocity.x) < 0.01f)
                velocity.x = 0;
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (landed && velocity.x == 0 && !missionComplete)
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

    void DetectPlatformCollision()
    {
        float platformY = platform.position.y + platform.localScale.y / 2 + birdRadius;
        float width = platform.localScale.x;

        bool crossedPlatform =
            previousPosition.y > platformY &&
            transform.position.y <= platformY;

        bool insidePlatform =
            transform.position.x >= platform.position.x - width / 2 &&
            transform.position.x <= platform.position.x + width / 2;

        if (crossedPlatform && insidePlatform)
        {
            transform.position = new Vector2(transform.position.x, platformY);

            velocity = new Vector2(velocity.x, 0);

            landed = true;
        }
    }

    void CheckPlatformEdge()
    {
        if (!landed) return;

        float width = platform.localScale.x;

        bool outsidePlatform =
            transform.position.x < platform.position.x - width / 2 ||
            transform.position.x > platform.position.x + width / 2;

        if (outsidePlatform)
        {
            landed = false;
        }
    }

    void CheckFall()
    {
        if (transform.position.y < platform.position.y - 3f)
        {
            RestartScene();
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}