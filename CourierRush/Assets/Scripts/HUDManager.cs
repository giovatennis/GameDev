using UnityEngine;
using TMPro;


public class HUDManager : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    
    public Color warningColor = Color.red;
    public float warningThreshold = 10f;

    Color defaultTimerColor;

    void Start()
    {
        if (timerText != null)
            defaultTimerColor = timerText.color;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // ── Timer ──
        if (timerText != null)
        {
            float t = GameManager.Instance.TimeRemaining;
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            timerText.text = string.Format("Time: {0}:{1:00}", minutes, seconds);
            timerText.color = t <= warningThreshold ? warningColor : defaultTimerColor;
        }

        // ── Score ──
        if (scoreText != null)
        {
            scoreText.text = "Deliveries: " + GameManager.Instance.Score;
        }
    }
}
