using UnityEngine;

public class SuikaGameOverLine : MonoBehaviour
{
    [Header("ゲームオーバーになるまでの時間（秒）")]
    public float timeToGameOver = 2.0f;

    private float _stayTimer = 0f;

    // 果物が線に触れ続けている間、ずっと呼ばれる処理
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!SuikaGameManager.Instance.IsPlaying) return;

        // 触れているのが果物（SuikaFruitがついているか）を確認
        if (collision.GetComponent<SuikaFruit>() != null)
        {
            _stayTimer += Time.deltaTime; // タイマーを進める

            // 指定時間を超えたらゲームオーバー！
            if (_stayTimer >= timeToGameOver)
            {
                SuikaGameManager.Instance.GameOver();
                _stayTimer = 0f; // 重複して呼ばれないようにリセット
            }
        }
    }

    // 果物が線から離れたらタイマーをリセット
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<SuikaFruit>() != null)
        {
            _stayTimer = 0f;
        }
    }
}
