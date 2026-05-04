using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SuikaCameraFitter : MonoBehaviour
{
    void Start()
    {
        AdjustCameraSize();
    }

    // エディタのGameビューで画面サイズをドラッグで変えた時もリアルタイムに追従させます
    void Update()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // 基準となるゲームのサイズ（PPU100想定）
        float targetWidth = 13.44f;
        float targetHeight = 22.57f;
        
        float targetAspect = targetWidth / targetHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        Camera cam = GetComponent<Camera>();

        if (currentAspect < targetAspect)
        {
            // 端末が基準より縦長の場合（Pixel 10 Pro XLなど）
            // 横幅が絶対に画面内に収まるように、カメラのSizeを自動で広げる
            cam.orthographicSize = (targetWidth / 2f) / currentAspect;
        }
        else
        {
            // 端末が基準より横長の場合
            // 縦幅を固定する
            cam.orthographicSize = targetHeight / 2f;
        }
    }
}