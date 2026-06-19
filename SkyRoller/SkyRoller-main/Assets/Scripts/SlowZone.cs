using UnityEngine;

// Hazard #1: Slow Zone.
// A trigger volume (mud patch, slime, sticky tile, etc.) that cuts the
// player's forward speed for a duration, making it harder to react to
// upcoming hazards. Reuses PlayerMovement's existing speed-change system.
public class SlowZone : MonoBehaviour
{
    public float slowedSpeed = 3f;
    public float slowDuration = 2f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ActivateSpeedBoost(slowedSpeed, slowDuration);
            }
        }
    }
}
