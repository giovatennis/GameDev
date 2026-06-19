using UnityEngine;

// Hazard #2: Reversed Controls Zone.
// A trigger volume (e.g. a "glitch tile" or electrified panel) that flips
// the player's steering input for a few seconds, forcing them to
// re-learn left/right while still trying to avoid falling off.
public class ReverseControlsZone : MonoBehaviour
{
    public float reverseDuration = 3f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ActivateReversedControls(reverseDuration);
            }
        }
    }
}
