using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Health
    public int maxHealth = 3;
    private int currentHealth;

    //Score
    private int score = 0;

    //UI
    public TextMeshProUGUI scoreText;
    public HealthUI healthUI;

    //Audio
    public AudioSource audioSource;
    public AudioClip enemyExplosionSound;
    public AudioClip playerExplosionSound;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void PlayerHitByBullet()
    {
        if (isGameOver) 
        {
            return;
        }

        currentHealth--;
        healthUI.RemoveIcon();

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    public void PlayerHitByMeteor()
    {
        if (isGameOver)
        { 
            return;
        }
        TriggerGameOver();
    }

    public void EnemyDestroyed(Vector3 position)
    {
        if (audioSource != null && enemyExplosionSound != null)
        {
            audioSource.PlayOneShot(enemyExplosionSound);
        }

        AddScore(100);
    }

    void TriggerGameOver()
    {
        if (isGameOver) 
        {
            return;
        }
        isGameOver = true;

        if (audioSource != null && playerExplosionSound != null)
        {
            audioSource.PlayOneShot(playerExplosionSound);
        }

        // Small delay so the explosion sound can play before reload
        Invoke(nameof(ReloadScene), 1.5f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
