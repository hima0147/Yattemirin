using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    [Header("動かしたい扉を指定")]
    public RectTransform leftDoor;
    public RectTransform rightDoor;
    public CanvasGroup buttonPanelGroup;

    [Header("アニメーション設定")]
    public float openDuration = 1.0f;
    // public float moveDistance = 600f; // ★固定値をやめました

    private float originalLeftX;
    private float originalRightX;
    private float moveDistance; // ★自動計算用の変数に変更

    void Start()
    {
        if (leftDoor != null)
        {
            originalLeftX = leftDoor.anchoredPosition.x;
            // ★修正：扉の実際の幅を「移動距離」として自動計算する
            moveDistance = leftDoor.rect.width; 
        }
        if (rightDoor != null)
        {
            originalRightX = rightDoor.anchoredPosition.x;
        }
    }

    // ゲーム開始用：扉を開いて、完了したらシーンを読み込む
    public void LoadGameScene(string nextSceneName)
    {
        if (buttonPanelGroup != null)
        {
            buttonPanelGroup.blocksRaycasts = false;
            buttonPanelGroup.DOFade(0f, 0.5f);
        }

        // 左扉を moveDistance 分だけ左へ
        leftDoor.DOAnchorPosX(originalLeftX - moveDistance, openDuration)
            .SetDelay(0.2f)
            .SetEase(Ease.OutQuad);

        // 右扉を moveDistance 分だけ右へ
        rightDoor.DOAnchorPosX(originalRightX + moveDistance, openDuration)
            .SetDelay(0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                Debug.Log(nextSceneName + " へ移動します");
                SceneManager.LoadScene(nextSceneName);
            });
    }

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
