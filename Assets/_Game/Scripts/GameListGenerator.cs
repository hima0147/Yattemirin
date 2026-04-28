using UnityEngine;
using System.Collections.Generic;

public class GameListGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public List<MinigameData> gameDataList;

    // Hierarchyにあるエレベーターコントローラーをここに入れる
    public ElevatorController elevatorController; 

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        foreach (var data in gameDataList)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            GameButton buttonScript = newButton.GetComponent<GameButton>();

            if (buttonScript != null)
            {
                // データと一緒にエレベーターの参照も渡す！
                buttonScript.Setup(data, elevatorController);
                
                // ボタンコンポーネントのOnClickに、自動で関数を登録する
                // (これでInspectorでポチポチ設定しなくて済みます)
                newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonScript.OnClickButton);
            }
        }
    }
}