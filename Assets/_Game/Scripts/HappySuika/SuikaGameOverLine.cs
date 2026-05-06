using UnityEngine;
using System.Collections.Generic;

public class SuikaGameOverLine : MonoBehaviour
{
    [Header("ゲームオーバーになるまでの時間（秒）")]
    public float gameOverTime = 2.0f;

    // 線に触れている果物それぞれの「滞在時間」を記録する辞書
    private Dictionary<Collider2D, float> fruitTimers = new Dictionary<Collider2D, float>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit"))
        {
            // ★修正ポイント：まだ落としていない（プレイヤーが掴んでいて重力が0の）果物は無視する
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null && rb.gravityScale == 0f) return;

            // タイマーのカウントアップ
            if (!fruitTimers.ContainsKey(collision))
            {
                fruitTimers[collision] = 0f;
            }
            fruitTimers[collision] += Time.deltaTime;

            // 2秒を超えたらゲームオーバー
            if (fruitTimers[collision] >= gameOverTime)
            {
                SuikaGameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 果物が線から離れたら、その果物のタイマーをリセット（削除）する
        if (fruitTimers.ContainsKey(collision))
        {
            fruitTimers.Remove(collision);
        }
    }
}
