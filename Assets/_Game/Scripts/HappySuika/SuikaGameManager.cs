using UnityEngine;

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance; // どこからでも呼べるようにする

    public SuikaUIManager uiManager;
    private int _currentScore = 0;
    private bool _isGamePlaying = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowTitle();
    }

    // タイトル画面を表示
    public void ShowTitle()
    {
        _isGamePlaying = false;
        _currentScore = 0;
        uiManager.UpdateScore(0);
        uiManager.SetUIState(false);
    }

    // ゲーム開始（ボタンから呼ばれる）
    public void GameStart()
    {
        _isGamePlaying = true;
        uiManager.SetUIState(true);
        Debug.Log("ゲーム開始！");
    }

    // スコア加算
    public void AddScore(int amount)
    {
        _currentScore += amount;
        uiManager.UpdateScore(_currentScore);
    }
}