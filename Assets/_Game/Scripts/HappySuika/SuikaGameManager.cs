using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // ★追加：シーン遷移に必要

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance;

    [Header("UI管理")]
    public SuikaUIManager uiManager;

    [Header("プレイヤー操作管理")]
    public SuikaPlayerController playerController;

    [Header("全11種類の果物プレハブを順番に登録(0〜10)")]
    public GameObject[] allFruitPrefabs;

    // ★追加：戻る先のメインメニューのシーン名（完全一致で指定）
    [Header("戻る先のシーン名")]
    public string mainMenuSceneName = "MainMenu"; 

    private int _currentScore = 0;
    private bool _isGamePlaying = false;
    public bool isDebugMode = false;
    private bool _isPaused = false; 

    public bool IsPlaying => _isGamePlaying;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowTitle();
    }

    public void ShowTitle()
    {
        _isGamePlaying = false;
        isDebugMode = false;
        _currentScore = 0;
        uiManager.UpdateScore(0);
        
        Time.timeScale = 1f; 
        _isPaused = false;

        if (uiManager.nextFruitIcon != null)
        {
            Color c = uiManager.nextFruitIcon.color;
            c.a = 0f;
            uiManager.nextFruitIcon.color = c;
        }

        if (playerController != null) playerController.StopGame();
        ClearAllFruits();
        uiManager.ShowTitleScreen();
    }

    public void OnClickStartButton()
    {
        StartCoroutine(StartGameSequence());
    }

    public void StartDebugGame()
    {
        isDebugMode = true;
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        uiManager.ShowReadyScreen();
        yield return new WaitForSeconds(1.5f);

        uiManager.ShowGoScreen();
        yield return new WaitForSeconds(0.5f);

        uiManager.ShowGameScreen();
        _isGamePlaying = true;
        
        if (playerController != null)
        {
            playerController.StartGame();
        }
    }

    public void EvolveFruit(int nextLevel, Vector3 spawnPosition)
    {
        if (nextLevel >= allFruitPrefabs.Length) return;
        Instantiate(allFruitPrefabs[nextLevel], spawnPosition, Quaternion.identity);
        int scoreToAdd = nextLevel * 10; 
        AddScore(scoreToAdd);
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        uiManager.UpdateScore(_currentScore);
    }

    public void GameOver()
    {
        if (!_isGamePlaying) return;
        _isGamePlaying = false;
        uiManager.ShowGameOverScreen(_currentScore);
    }

    public void ForceGameOver()
    {
        if (isDebugMode) GameOver();
    }

    // ==========================================
    // ポーズ機能関連
    // ==========================================
    
    public void OpenPauseMenu()
    {
        if (!_isGamePlaying || _isPaused) return;
        _isPaused = true;
        Time.timeScale = 0f; 
        uiManager.ShowPauseScreen(true);
    }

    public void ResumeGame()
    {
        if (!_isGamePlaying) return;
        _isPaused = false;
        Time.timeScale = 1f; 
        uiManager.ShowPauseScreen(false);
    }

    public void OpenHowToPlay()
    {
        uiManager.ShowHowToPlayScreen(true);
    }

    public void CloseHowToPlay()
    {
        uiManager.ShowHowToPlayScreen(false);
    }

    public void ReturnToTitleFromPause()
    {
        ResumeGame(); 
        ShowTitle();  
    }

    // ==========================================
    // リトライ確認機能関連
    // ==========================================

    public void OpenRetryConfirm()
    {
        if (!_isGamePlaying || _isPaused) return;
        _isPaused = true;
        Time.timeScale = 0f; 
        uiManager.ShowRetryConfirmScreen(true);
    }

    public void OnRetryNo()
    {
        uiManager.ShowRetryConfirmScreen(false);
        _isPaused = false;
        Time.timeScale = 1f; 
    }

    public void OnRetryYes()
    {
        uiManager.ShowRetryConfirmScreen(false);
        Time.timeScale = 1f; 
        _isPaused = false;
        _isGamePlaying = false; 
        _currentScore = 0;
        uiManager.UpdateScore(0);
        if (playerController != null) playerController.StopGame();
        ClearAllFruits();
        StartCoroutine(StartGameSequence());
    }

    private void ClearAllFruits()
    {
        SuikaFruit[] fruits = FindObjectsOfType<SuikaFruit>();
        foreach (SuikaFruit f in fruits)
        {
            Destroy(f.gameObject);
        }
    }

    // ★追加：メインメニュー（親アプリ）に戻る機能
    public void ReturnToMainMenu()
    {
        // 念のため時間を元に戻しておく
        Time.timeScale = 1f;
        
        // 指定したシーン名を読み込む
        Debug.Log(mainMenuSceneName + " シーンへ戻ります");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}