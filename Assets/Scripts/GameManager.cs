using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scoring")]
    public int totalScore = 0;
    public TMP_Text scoreText;

    [Header("Shots")]
    public int maxShots = 5;
    private int shotsFired = 0;
    public TMP_Text shotsText;

    [Header("UI")]
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void AddScore(int score)
    {
        totalScore += score;
        shotsFired++;

        UpdateUI();

        if (shotsFired >= maxShots)
            EndGame();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + totalScore;

        if (shotsText != null)
            shotsText.text = "Shots: " + shotsFired + " / " + maxShots;
    }

    void EndGame()
    {
        Debug.Log("Game Over");
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
