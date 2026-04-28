using UnityEngine;

public class SafeAreaAdjuster : MonoBehaviour
{
    void Start()
    {
        RectTransform panel = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;

        // スマホの安全な領域（セーフエリア）の比率を計算
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // パネルのアンカーを安全な領域に合わせる
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
    }
}
