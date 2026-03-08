using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene5Player : MonoBehaviour
{
    [Header("Launch Settings")]
    public Transform launchPoint;
    public float launchPower = 4f;
    public float maxDragDistance = 2f;

    [Header("Physics Settings")]
    public Vector2 velocity;
    public float gravity = -9.8f;
    public float frictionCoefficient = 0.3f;

    [Header("Platform Settings")]
    public Transform platform;
    public float platformWidth = 6f;

    [Header("UI")]
    public TextMeshProUGUI resultText;

    private bool isDragging = false;
    private bool hasLaunched = false;
    private bool onPlatform = false;
    private bool missionComplete = false;

    private Vector2 previousPosition;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (launchPoint != null)
            transform.position = launchPoint.position;

        previousPosition = transform.position;

        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (launchPoint == null || platform == null)
            return;

        if (missionComplete)
            return;

        if (!hasLaunched)
        {
            HandleDragAndLaunch();
        }
        else
        {
            previousPosition = transform.position;

            if (!onPlatform)
            {
                ApplyAirPhysics();
                CheckPlatformLanding();
                CheckMissedPlatform();
            }
            else
            {
                ApplyFrictionOnPlatform();
                CheckIfFellOffPlatform();
            }
        }
    }

    void HandleDragAndLaunch()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            float distance = Vector2.Distance(mouseWorld, transform.position);

            if (distance < 1f)
                isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 dragVector = mouseWorld - (Vector2)launchPoint.position;

            if (dragVector.magnitude > maxDragDistance)
                dragVector = dragVector.normalized * maxDragDistance;

            transform.position = (Vector2)launchPoint.position + dragVector;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            Vector2 launchDirection =
                (Vector2)launchPoint.position - (Vector2)transform.position;

            velocity = launchDirection * launchPower;
            hasLaunched = true;
        }
    }

    void ApplyAirPhysics()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -20f, 20f);

        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void CheckPlatformLanding()
    {
        float platformTopY = platform.position.y + (platform.localScale.y / 2f);
        float platformLeft = platform.position.x - (platformWidth / 2f);
        float platformRight = platform.position.x + (platformWidth / 2f);

        Vector2 currentPosition = transform.position;

        bool crossedPlatform =
            previousPosition.y > platformTopY &&
            currentPosition.y <= platformTopY;

        bool insideBounds =
            currentPosition.x >= platformLeft &&
            currentPosition.x <= platformRight;

        if (crossedPlatform && insideBounds)
        {
            float halfHeight = 0.5f;

            if (sr != null)
                halfHeight = sr.bounds.extents.y;

            Vector3 pos = transform.position;
            pos.y = platformTopY + halfHeight;
            transform.position = pos;

            velocity = new Vector2(velocity.x, 0f);
            onPlatform = true;
        }
    }

    void ApplyFrictionOnPlatform()
    {
        float normalForce = Mathf.Abs(gravity);
        float frictionAcceleration = frictionCoefficient * normalForce;

        if (velocity.x > 0)
        {
            velocity.x -= frictionAcceleration * Time.deltaTime;
            if (velocity.x < 0) velocity.x = 0;
        }
        else if (velocity.x < 0)
        {
            velocity.x += frictionAcceleration * Time.deltaTime;
            if (velocity.x > 0) velocity.x = 0;
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (Mathf.Abs(velocity.x) < 0.01f)
        {
            velocity.x = 0f;
            MissionCompleted();
        }
    }

    void CheckIfFellOffPlatform()
    {
        float platformLeft = platform.position.x - (platformWidth / 2f);
        float platformRight = platform.position.x + (platformWidth / 2f);

        if (transform.position.x < platformLeft || transform.position.x > platformRight)
            RestartScene();
    }

    void CheckMissedPlatform()
    {
        if (transform.position.y < platform.position.y - 3f)
            RestartScene();
    }

    void MissionCompleted()
    {
        missionComplete = true;

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = "MISSION COMPLETED";
        }

        Debug.Log("MISSION COMPLETED");
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}