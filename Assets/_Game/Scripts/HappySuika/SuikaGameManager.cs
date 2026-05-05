using UnityEngine;
using System.Collections;

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance;

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

    public void ShowTitle()
    {
        _isGamePlaying = false;
        _currentScore = 0;
        uiManager.UpdateScore(0);
        uiManager.ShowTitleScreen();
    }

    // スタートボタンが押されたら呼ばれる
    public void OnClickStartButton()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        // 1. Ready画像を表示して1.5秒待つ
        uiManager.ShowReadyScreen();
        yield return new WaitForSeconds(1.5f);

        // 2. Go!!画像に切り替えて0.5秒待つ
        uiManager.ShowGoScreen();
        yield return new WaitForSeconds(0.5f);

        // 3. プレイ画面UIに切り替えてゲーム開始
        uiManager.ShowGameScreen();
        _isGamePlaying = true;
        Debug.Log("ゲーム開始！");
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        uiManager.UpdateScore(_currentScore);
    }
}