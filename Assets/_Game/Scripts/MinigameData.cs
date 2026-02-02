using UnityEngine;

// これを書くと、Unityの右クリックメニューに「Create > Yattemirin > MinigameData」が出現します
[CreateAssetMenu(fileName = "NewGameData", menuName = "Yattemirin/MinigameData")]
public class MinigameData : ScriptableObject
{
    [Header("ゲームの基本情報")]
    public string gameTitle;      // ゲームの名前（例：つみつみバーガー）
    
    [TextArea]
    public string description;    // 説明文（親向けなど）

    public Sprite icon;           // ボタンに表示するアイコン画像
    
    [Header("シーン設定")]
    public string sceneName;      // 移動先のシーン名（ロード用）
}