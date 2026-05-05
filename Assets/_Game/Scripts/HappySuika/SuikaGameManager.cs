using UnityEngine;
using System.Collections;

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance;

    [Header("UI管理")]
    public SuikaUIManager uiManager;

    [Header("プレイヤー操作管理")]
    public SuikaPlayerController playerController; // 追加：Playerへの指示用

    [Header("全11種類の果物プレハブを順番に登録(0〜10)")]
    public GameObject[] allFruitPrefabs;

    private int _currentScore = 0;
    private bool _isGamePlaying = false;

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
        _currentScore = 0;
        uiManager.UpdateScore(0);
        
        // タイトル画面ではネクストアイコンを透明にして隠しておく
        if (uiManager.nextFruitIcon != null)
        {
            Color c = uiManager.nextFruitIcon.color;
            c.a = 0f;
            uiManager.nextFruitIcon.color = c;
        }

        uiManager.ShowTitleScreen();
    }

    public void OnClickStartButton()
    {
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
        
        // プレイ画面になったら、プレイヤーに果物の準備を指示する
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
}