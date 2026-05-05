using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuikaUIManager : MonoBehaviour
{
    [Header("表示パネル")]
    public GameObject panelTitle;
    public GameObject panelReady;
    public GameObject panelGo;
    public GameObject panelGameInfo;
    public GameObject panelGameOver;

    [Header("テキスト・アイコン")]
    public TextMeshProUGUI scoreText;
    public Image nextFruitIcon;

    [Header("ゲームオーバー画面用")] // 追加：リザルト表示用
    public TextMeshProUGUI resultScoreText;
    public TextMeshProUGUI rankText;

    [Header("背景（余白）の色変更")]
    public Camera mainCamera;
    public Color titleBackgroundColor = Color.white;
    public Color gameBackgroundColor = Color.white;

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateNextFruitIcon(Sprite fruitSprite)
    {
        if (nextFruitIcon != null)
        {
            nextFruitIcon.sprite = fruitSprite;
            Color c = nextFruitIcon.color;
            c.a = 1f;
            nextFruitIcon.color = c;
        }
    }

    public void ShowTitleScreen()
    {
        panelTitle.SetActive(true);
        panelReady.SetActive(false);
        panelGo.SetActive(false);
        panelGameInfo.SetActive(false);
        if(panelGameOver != null) panelGameOver.SetActive(false);
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
        if(panelGameOver != null) panelGameOver.SetActive(false);
        if (mainCamera != null) mainCamera.backgroundColor = gameBackgroundColor;
    }

    // 変更：スコアを受け取ってリザルト画面に反映する
    public void ShowGameOverScreen(int finalScore)
    {
        if(panelGameOver != null) panelGameOver.SetActive(true);

        // 最終スコアの表示
        if(resultScoreText != null)
        {
            resultScoreText.text = finalScore.ToString() + "点";
        }

        // スコアに応じた称号の決定と表示
        if(rankText != null)
        {
            string rank = "ブロンズ"; // デフォルト
            if (finalScore >= 3000) rank = "レジェンド";
            else if (finalScore >= 1500) rank = "ゴールド";
            else if (finalScore >= 500) rank = "シルバー";
            
            rankText.text = rank;
        }
    }
}