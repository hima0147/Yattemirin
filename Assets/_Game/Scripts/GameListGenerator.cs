using UnityEngine;
using System.Collections.Generic; // List（リスト）を使うために必要

public class GameListGenerator : MonoBehaviour
{
    [Header("ボタンの金型（プレハブ）")]
    public GameObject buttonPrefab;

    [Header("ボタンを並べる場所")]
    public Transform buttonContainer; // ButtonPanelのこと

    [Header("登録するゲームデータ一覧")]
    public List<MinigameData> gameDataList; // ここにデータを登録していく

    void Start()
    {
        // ゲーム開始時にボタン生成を実行
        GenerateButtons();
    }

    void GenerateButtons()
    {
        // 登録されているデータの数だけループする
        foreach (var data in gameDataList)
        {
            // 1. プレハブから新しいボタンを作る（Instantiate = 実体化）
            // Instantiate(元ネタ, 親オブジェクト)
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

            // 2. 作ったボタンのScriptを取得して、データを渡す
            GameButton buttonScript = newButton.GetComponent<GameButton>();
            if (buttonScript != null)
            {
                buttonScript.Setup(data);
            }
        }
    }
}