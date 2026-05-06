using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameButton : MonoBehaviour
{
    [Header("UIパーツ")]
    public TextMeshProUGUI titleText;
    public Image iconImage;

    private MinigameData myData;
    private ElevatorController elevator; // エレベーターへの連絡用

    // セットアップ時に、エレベーターの連絡先も教えてもらう
    public void Setup(MinigameData data, ElevatorController elevatorController)
    {
        myData = data;
        elevator = elevatorController; // 連絡先を覚える

        // ===============================================
        // ★修正：アイコンの有無で「文字」と「画像」の表示を切り替える
        // ===============================================
        if (data.icon != null)
        {
            // 【アイコンがある場合】（スイカゲームなど）
            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
                iconImage.gameObject.SetActive(true); // 画像枠を表示(ON)
            }
            if (titleText != null)
            {
                titleText.gameObject.SetActive(false); // テキストを隠す(OFF)
            }
        }
        else
        {
            // 【アイコンがない場合】（つみつみバーガーなど）
            if (titleText != null)
            {
                titleText.text = data.gameTitle;
                titleText.gameObject.SetActive(true); // テキストを表示(ON)
            }
            if (iconImage != null)
            {
                iconImage.gameObject.SetActive(false); // 画像枠を隠す(OFF)
            }
        }
    }

    // ボタン設定で「On Click」に登録する関数
    public void OnClickButton()
    {
        if (myData != null && elevator != null)
        {
            // エレベーターに「このシーンへ移動して！」と依頼
            elevator.LoadGameScene(myData.sceneName);
        }
    }
}