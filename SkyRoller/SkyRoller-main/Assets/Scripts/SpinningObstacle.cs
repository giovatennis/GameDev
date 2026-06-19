using UnityEngine;

// Hazard #3: Spinning Obstacle.
// A rotating arm/log (rotate this object's transform around the Y axis in
// Update, or use an Animator) that, on collision, shoves the player
// sideways and briefly locks out their input - risking knocking them off
// the platform edge. Use a normal (non-trigger) Collider for this one so
// physics collision gives us a contact point to push away from.
public class SpinningObstacle : MonoBehaviour
{
    public float rotationSpeed = 120f;   // degrees per second
    public float knockbackForce = 8f;
    public float controlLockDuration = 0.5f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            return;
        }

        // Push the player away from the obstacle's center, with a bit of
        // lift so they visibly get knocked aside rather than just slowed.
        Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
        pushDirection.y = 0.3f;

        playerMovement.ApplyKnockback(pushDirection * knockbackForce, controlLockDuration);
    }
}
