using UnityEngine;
using UnityEngine.UI; // 画像(Image)を扱うのに必要
using TMPro;          // 文字(TextMeshPro)を扱うのに必要

public class GameButton : MonoBehaviour
{
    [Header("UIパーツの割り当て")]
    public TextMeshProUGUI titleText; // タイトルを表示するテキスト
    public Image iconImage;           // アイコンを表示する画像

    // このボタンが担当するゲームデータ
    private MinigameData myData;

    // 外部（GameManagerなど）からデータをセットしてもらう関数
    public void Setup(MinigameData data)
    {
        myData = data;

        // データの中身をUIに反映
        if (titleText != null)
        {
            titleText.text = data.gameTitle;
        }

        if (iconImage != null && data.icon != null)
        {
            iconImage.sprite = data.icon;
        }
    }
    
    // ボタンが押された時の処理（後で使います）
    public void OnClickButton()
    {
        if (myData != null)
        {
            Debug.Log("これから遊ぶゲーム: " + myData.gameTitle);
            // ここにシーン移動の処理を後で書きます
        }
    }
    // ↓テスト用に追記（テストが終わったら消してOK）
    public MinigameData testData; // Inspectorでデータをセットする用

    // Inspectorの値が変わった時に自動で実行される関数
    void OnValidate()
    {
        if (testData != null)
        {
            Setup(testData);
        }
    }
}

