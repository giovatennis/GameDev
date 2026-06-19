using UnityEngine;

// Same trigger-based fall detection as before, but now it routes through
// GameManager so the score freezes and a game-over screen can show, instead
// of instantly reloading the scene.
public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                // Fallback in case this is tested in a scene without a GameManager.
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }
}
