using UnityEngine;
using TMPro;

public class SuikaUIManager : MonoBehaviour
{
    [Header("表示パネル")]
    public GameObject panelTitle;
    public GameObject panelGameInfo;

    [Header("テキスト等")]
    public TextMeshProUGUI scoreText;
    public GameObject nextFruitIcon; // 後で画像を表示する用

    // スコア表示を更新する
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    // 画面の表示を切り替える
    public void SetUIState(bool isPlaying)
    {
        panelTitle.SetActive(!isPlaying);   // プレイ中はタイトルを隠す
        panelGameInfo.SetActive(isPlaying); // プレイ中はスコア等を表示する
    }
}