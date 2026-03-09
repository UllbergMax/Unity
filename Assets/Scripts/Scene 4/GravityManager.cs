using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float G = 5f;

    private GravityBody[] bodies;

    void FixedUpdate()
    {
        bodies = FindObjectsByType<GravityBody>(FindObjectsSortMode.None);

        foreach (GravityBody body in bodies)
        {
            if (body.CompareTag("Player"))
            {
                ApplyPlanetGravity(body);
            }
        }
    }

    void ApplyPlanetGravity(GravityBody player)
    {
        GravityBody[] planets = FindObjectsByType<GravityBody>(FindObjectsSortMode.None);

        foreach (GravityBody planet in planets)
        {
            if (planet.CompareTag("Planet") || planet.CompareTag("Pluto"))
            {
                Vector2 direction = planet.rb.position - player.rb.position;
                float distance = direction.magnitude;

                if (distance < 0.5f) return;

                float force = G * (player.mass * planet.mass) / (distance * distance);

                player.rb.AddForce(direction.normalized * force);
            }
        }
    }
}