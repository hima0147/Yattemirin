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

        if (titleText != null) titleText.text = data.gameTitle;
        if (iconImage != null && data.icon != null) iconImage.sprite = data.icon;
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