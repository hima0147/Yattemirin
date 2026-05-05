using UnityEngine;
using UnityEngine.UI; // UIを操作するために追加
using TMPro;

public class SuikaUIManager : MonoBehaviour
{
    [Header("表示パネル")]
    public GameObject panelTitle;
    public GameObject panelReady;
    public GameObject panelGo;
    public GameObject panelGameInfo;

    [Header("テキスト・アイコン")]
    public TextMeshProUGUI scoreText;
    public Image nextFruitIcon; // GameObjectからImage型に変更

    [Header("背景（余白）の色変更")]
    public Camera mainCamera;
    public Color titleBackgroundColor = Color.white;
    public Color gameBackgroundColor = Color.white;

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    // 次の果物の画像アイコンを更新する処理
    public void UpdateNextFruitIcon(Sprite fruitSprite)
    {
        if (nextFruitIcon != null)
        {
            nextFruitIcon.sprite = fruitSprite;
            // 透明だったアイコンを不透明(アルファ値1)にして表示する
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
        if (mainCamera != null) mainCamera.backgroundColor = gameBackgroundColor;
    }
}