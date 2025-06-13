using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro.Examples;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scoring")]
    private int totalScore = 0;
    public TMP_Text scoreText;
    public TMP_Text finalScore;
    

    [Header("Shots")]
    private int maxShots = 10;
    private int shotsFired = 0;
    public TMP_Text shotsText;

    [Header("UI")]
    public BowDrawback bowScript;
    public GameObject gameOverPanel;
    public GameObject congratulationsPanel;
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    [Header("Waves")]
    public GameObject[] targetWaves; // Assign in Inspector
    private int currentWaveIndex = 0;
    private int targetsRemainingInWave = 0;

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

        StartWave(currentWaveIndex);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
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

    void Congratulations()
    {
        if (congratulationsPanel != null)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true;
            if (bowScript != null)
                bowScript.enabled = false;// Make it visible
            congratulationsPanel.SetActive(true);
            if (finalScore != null)
                finalScore.text = "Final Score: " + totalScore;
        }
    }
    void EndGame()
    {
        
        if (gameOverPanel != null)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true;
            if (bowScript != null)
                bowScript.enabled = false;// Make it visible
            gameOverPanel.SetActive(true);
        }
            
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (bowScript != null)
            bowScript.enabled = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (bowScript != null)
            bowScript.enabled = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    // === NEW WAVE LOGIC ===

    void StartWave(int index)
    {
        if (currentWaveIndex > 3)
        {
            Congratulations();
        }
        shotsFired = 0;
        if (index >= targetWaves.Length) return;

        GameObject wave = targetWaves[index];
        wave.SetActive(true);
        targetsRemainingInWave = wave.GetComponentsInChildren<Target>().Length;
        UpdateUI();
    }

    public void OnTargetDestroyed()
    {
        targetsRemainingInWave--;
        if (targetsRemainingInWave <= 0)
        {
            currentWaveIndex++;
            StartWave(currentWaveIndex);
        }
    }
}
