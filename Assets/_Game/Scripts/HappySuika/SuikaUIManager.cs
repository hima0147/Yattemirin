using UnityEngine;
using TMPro;

public class SuikaUIManager : MonoBehaviour
{
    [Header("表示パネル")]
    public GameObject panelTitle;
    public GameObject panelReady;
    public GameObject panelGo;
    public GameObject panelGameInfo;

    [Header("テキスト等")]
    public TextMeshProUGUI scoreText;
    public GameObject nextFruitIcon;

    [Header("背景（余白）の色変更")] // 追加：カメラの色を変えるための枠
    public Camera mainCamera;
    public Color titleBackgroundColor = Color.white; // タイトル用（後でピンクに）
    public Color gameBackgroundColor = Color.white;  // ゲーム用（後で水色に）

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowTitleScreen()
    {
        panelTitle.SetActive(true);
        panelReady.SetActive(false);
        panelGo.SetActive(false);
        panelGameInfo.SetActive(false);

        // カメラ（余白）の色をタイトル用の色にする
        if (mainCamera != null) mainCamera.backgroundColor = titleBackgroundColor;
    }

    public void ShowReadyScreen()
    {
        panelTitle.SetActive(false);
        panelReady.SetActive(true);
    }

    public void ShowGoScreen()
    {
        panelReady.SetActive(false);
        panelGo.SetActive(true);
    }

    public void ShowGameScreen()
    {
        panelGo.SetActive(false);
        panelGameInfo.SetActive(true);

        // カメラ（余白）の色をゲーム用の色に戻す
        if (mainCamera != null) mainCamera.backgroundColor = gameBackgroundColor;
    }
}