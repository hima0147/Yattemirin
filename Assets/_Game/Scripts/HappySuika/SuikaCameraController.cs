using UnityEngine;
using UnityEngine.Rendering.Universal; // URP用の名前空間を追加

[RequireComponent(typeof(Camera))]
public class SuikaCameraController : MonoBehaviour
{
    private readonly float targetWidth = 1344f;
    private readonly float targetHeight = 2257f;
    public Color letterboxColor = new Color(0.627f, 0.847f, 0.937f); // #A0D8EF

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        SetupURPCameras();
        AdjustCameraRect();
    }

    void SetupURPCameras()
    {
        // 1. 余白用の背景カメラを生成
        GameObject bgCamObj = new GameObject("BackgroundCamera");
        Camera backgroundCamera = bgCamObj.AddComponent<Camera>();
        
        // 2. URP固有の追加データ（UniversalAdditionalCameraData）を取得
        var bgData = backgroundCamera.GetUniversalAdditionalCameraData();
        var mainData = mainCamera.GetUniversalAdditionalCameraData();

        // 3. 背景カメラを「Base」にし、メインカメラを「Overlay」に変更して重ねる
        backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        backgroundCamera.backgroundColor = letterboxColor;
        
        mainData.renderType = CameraRenderType.Overlay;
        bgData.cameraStack.Add(mainCamera); // 背景カメラの上にメインカメラを重ねる
    }

    void AdjustCameraRect()
    {
        float targetAspect = targetWidth / targetHeight;
        float currentAspect = (float)Screen.width / Screen.height;
        float scaleHeight = currentAspect / targetAspect;

        Rect rect = mainCamera.rect;

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }
        mainCamera.rect = rect;
    }
}