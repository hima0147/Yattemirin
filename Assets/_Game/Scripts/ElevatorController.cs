using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; // シーン移動に必要！

public class ElevatorController : MonoBehaviour
{
    [Header("動かしたい扉を指定")]
    public RectTransform leftDoor;
    public RectTransform rightDoor;
    public CanvasGroup buttonPanelGroup;

    [Header("アニメーション設定")]
    public float openDuration = 1.0f;
    public float moveDistance = 600f;

    private float originalLeftX;
    private float originalRightX;

    void Start()
    {
        if (leftDoor != null) originalLeftX = leftDoor.anchoredPosition.x;
        if (rightDoor != null) originalRightX = rightDoor.anchoredPosition.x;
    }

    // ゲーム開始用：扉を開いて、完了したらシーンを読み込む
    public void LoadGameScene(string nextSceneName)
    {
        // 1. ボタンを消す
        if (buttonPanelGroup != null)
        {
            buttonPanelGroup.blocksRaycasts = false; // 連打防止
            buttonPanelGroup.DOFade(0f, 0.5f);
        }

        // 2. 扉を開く
        // OnComplete(...) の中に、アニメ終了後にやりたいことを書く
        leftDoor.DOAnchorPosX(originalLeftX - moveDistance, openDuration)
            .SetDelay(0.2f)
            .SetEase(Ease.OutQuad);

        rightDoor.DOAnchorPosX(originalRightX + moveDistance, openDuration)
            .SetDelay(0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                // 3. 開ききったらシーン移動！
                Debug.Log(nextSceneName + " へ移動します");
                SceneManager.LoadScene(nextSceneName);
            });
    }

    // デバッグ用（右クリックメニュー）
    [ContextMenu("テスト：扉を開く")]
    public void TestOpen()
    {
        leftDoor.DOAnchorPosX(originalLeftX - moveDistance, openDuration).SetEase(Ease.OutQuad);
        rightDoor.DOAnchorPosX(originalRightX + moveDistance, openDuration).SetEase(Ease.OutQuad);
    }

    [ContextMenu("テスト：扉を閉じる")]
    public void TestClose()
    {
        leftDoor.DOAnchorPosX(originalLeftX, openDuration).SetEase(Ease.InQuad);
        rightDoor.DOAnchorPosX(originalRightX, openDuration).SetEase(Ease.InQuad);
        if (buttonPanelGroup != null) buttonPanelGroup.DOFade(1f, 0.5f).SetDelay(openDuration);
    }
}