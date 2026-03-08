using UnityEngine;

public class Scene5CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 3f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, 0) + offset;
        desiredPosition.y = Mathf.Max(desiredPosition.y, 0);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}