using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

  
    public float totalTime = 60f;

    
    public int deliveryGoal = 5;


    public UnityEvent OnPackagePickedUp;
    public UnityEvent OnDeliveryCompleted;
    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;

    public float TimeRemaining { get; private set; }
    public int Score { get; private set; }
    public bool IsPlaying { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        TimeRemaining = totalTime;
        IsPlaying = true;
    }

    void Update()
    {
        if (!IsPlaying) return;

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            TriggerGameOver();
        }
    }

    public void RegisterPickup()
    {
        if (!IsPlaying) return;
        OnPackagePickedUp?.Invoke();
    }

    public void RegisterDelivery()
    {
        if (!IsPlaying) return;
        Score++;
        Debug.Log("Delivery registered! Score: " + Score + " / Goal: " + deliveryGoal);
        OnDeliveryCompleted?.Invoke();

        if (Score >= deliveryGoal)
            TriggerWin();
    }

    void TriggerGameOver()
    {
        IsPlaying = false;
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }

    void TriggerWin()
    {
        IsPlaying = false;
        Time.timeScale = 0f;
        OnGameWin?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}