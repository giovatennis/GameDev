using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Central hub for the endless mode: tracks survival score, knows when the
// player has lost, and handles the restart flow. Put this on a single empty
// GameObject in the scene (e.g. "GameManager") and wire up the UI fields
// in the Inspector.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public Transform player;           // the rolling ball
    public TextMeshProUGUI scoreText;  // in-game survival score label
    public GameObject gameOverPanel;   // panel containing "You Died" + restart button
    public TextMeshProUGUI finalScoreText; // optional, shown on the game over panel

    [Header("Scoring")]
    public float scoreMultiplier = 1f; // tune how fast the score climbs relative to distance

    bool isGameOver;
    float startZ;
    float currentScore;

    void Awake()
    {
        // Simple singleton so hazards/platforms/deathzones can reach this
        // without manual reference dragging everywhere.
        Instance = this;
    }

    void Start()
    {
        if (player != null)
        {
            startZ = player.position.z;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver || player == null)
        {
            return;
        }

        // Distance-based survival score: how far forward the player has rolled.
        float distance = player.position.z - startZ;
        currentScore = Mathf.Max(currentScore, distance * scoreMultiplier);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(currentScore);
        }
    }

    public float CurrentScore => currentScore;

    public bool IsGameOver => isGameOver;

    // Called by DeathZone (or any other "you lose" trigger) when the player
    // falls off the platforms.
    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "You survived: " + Mathf.FloorToInt(currentScore);
        }

        // Freeze gameplay rather than instantly reloading, so the player can
        // see their score and choose to restart.
        Time.timeScale = 0f;
    }

    // Hook this up to the Restart button's OnClick().
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
