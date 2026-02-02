using UnityEngine;
using DG.Tweening; // DOTweenを使うための宣言

public class ElevatorController : MonoBehaviour
{
    [Header("動かしたい扉を指定")]
    public RectTransform leftDoor;
    public RectTransform rightDoor;

    [Header("ボタンパネル（消すために必要）")]
    public CanvasGroup buttonPanelGroup; // 追加：透明度を操るためのコンポーネント

    [Header("アニメーション設定")]
    public float openDuration = 1.0f;
    public float moveDistance = 600f;

    private float originalLeftX;
    private float originalRightX;

    void Start()
    {
        // 最初の位置を記憶
        if (leftDoor != null) originalLeftX = leftDoor.anchoredPosition.x;
        if (rightDoor != null) originalRightX = rightDoor.anchoredPosition.x;
    }

    [ContextMenu("テスト：扉を開く")]
    public void OpenDoors()
    {
        // 1. ボタンパネルをフェードアウト（透明にする）
        // DOFade(目標値, 時間)
        if (buttonPanelGroup != null)
        {
            buttonPanelGroup.DOFade(0f, 0.5f).OnComplete(() => {
                // 透明になりきったら、見えないボタンを押せないように判定を消す
                buttonPanelGroup.blocksRaycasts = false; 
            });
        }

        // 2. 扉を開く
        // SetDelay(0.2f) で、ボタンが消え始めて少ししてから扉が動き出すとかっこいい
        leftDoor.DOAnchorPosX(originalLeftX - moveDistance, openDuration).SetDelay(0.2f).SetEase(Ease.OutQuad);
        rightDoor.DOAnchorPosX(originalRightX + moveDistance, openDuration).SetDelay(0.2f).SetEase(Ease.OutQuad);
    }

    [ContextMenu("テスト：扉を閉じる")]
    public void CloseDoors()
    {
        // 扉を閉じる
        leftDoor.DOAnchorPosX(originalLeftX, openDuration).SetEase(Ease.InQuad);
        rightDoor.DOAnchorPosX(originalRightX, openDuration).SetEase(Ease.InQuad);

        // ボタンパネルを復活させる
        if (buttonPanelGroup != null)
        {
            // 扉が閉まりきった頃(openDuration後)にフェードイン開始
            buttonPanelGroup.DOFade(1f, 0.5f).SetDelay(openDuration).OnComplete(() => {
                buttonPanelGroup.blocksRaycasts = true; // ボタンを押せるように戻す
            });
        }
    }
}