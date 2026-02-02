using UnityEngine;
using DG.Tweening; // DOTweenを使うための宣言

public class ElevatorController : MonoBehaviour
{
    [Header("動かしたい扉を指定")]
    public RectTransform leftDoor;
    public RectTransform rightDoor;

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

    [ContextMenu("テスト：扉を開く")]
    public void OpenDoors()
    {
        // 扉を開く（DOTweenの魔法）
        leftDoor.DOAnchorPosX(originalLeftX - moveDistance, openDuration).SetEase(Ease.OutQuad);
        rightDoor.DOAnchorPosX(originalRightX + moveDistance, openDuration).SetEase(Ease.OutQuad);
    }

    [ContextMenu("テスト：扉を閉じる")]
    public void CloseDoors()
    {
        // 扉を閉じる
        leftDoor.DOAnchorPosX(originalLeftX, openDuration).SetEase(Ease.InQuad);
        rightDoor.DOAnchorPosX(originalRightX, openDuration).SetEase(Ease.InQuad);
    }
}