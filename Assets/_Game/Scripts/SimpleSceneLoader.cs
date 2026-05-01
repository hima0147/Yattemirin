using UnityEngine;
using UnityEngine.SceneManagement; // 画面移動に必要な魔法の呪文

public class SimpleSceneLoader : MonoBehaviour
{
    // ボタンから呼び出される「指定した名前の画面へ移動する」命令
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
