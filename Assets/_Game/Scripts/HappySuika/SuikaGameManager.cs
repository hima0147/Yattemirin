using UnityEngine;
using System.Collections;

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance;

    [Header("UI管理")]
    public SuikaUIManager uiManager;

    [Header("プレイヤー操作管理")]
    public SuikaPlayerController playerController;

    [Header("全11種類の果物プレハブを順番に登録(0〜10)")]
    public GameObject[] allFruitPrefabs;

    private int _currentScore = 0;
    private bool _isGamePlaying = false;
    
    // 追加：デバッグモードのフラグ
    public bool isDebugMode = false;

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
        isDebugMode = false; // タイトルに戻ったらデバッグモード解除
        _currentScore = 0;
        uiManager.UpdateScore(0);
        
        if (uiManager.nextFruitIcon != null)
        {
            Color c = uiManager.nextFruitIcon.color;
            c.a = 0f;
            uiManager.nextFruitIcon.color = c;
        }

        ClearAllFruits();
        uiManager.ShowTitleScreen();
    }

    // 通常のスタートボタン用
    public void OnClickStartButton()
    {
        StartCoroutine(StartGameSequence());
    }

    // 追加：デバッグモードでスタートするためのボタン用
    public void StartDebugGame()
    {
        isDebugMode = true;
        Debug.Log("★デバッグモードで起動します★");
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
        Debug.Log("ゲームオーバー！");
    }

    // 追加：スコアをタッチした時に呼ばれる強制ゲームオーバー
    public void ForceGameOver()
    {
        if (isDebugMode)
        {
            Debug.Log("★強制ゲームオーバー発動★");
            GameOver();
        }
    }

    private void ClearAllFruits()
    {
        SuikaFruit[] fruits = FindObjectsOfType<SuikaFruit>();
        foreach (SuikaFruit f in fruits)
        {
            Destroy(f.gameObject);
        }
    }
}