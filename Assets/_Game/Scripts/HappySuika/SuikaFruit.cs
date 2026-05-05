using UnityEngine;

public class SuikaFruit : MonoBehaviour
{
    [Header("果物のレベル (0=ポテト, 1=次, ... 10=最大)")]
    public int fruitLevel = 0;

    private bool _hasMerged = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasMerged || !SuikaGameManager.Instance.IsPlaying) return;

        SuikaFruit otherFruit = collision.gameObject.GetComponent<SuikaFruit>();
        if (otherFruit != null)
        {
            if (!otherFruit._hasMerged && this.fruitLevel == otherFruit.fruitLevel)
            {
                // レベル10未満（スイカ未満）なら通常通り進化
                if (this.fruitLevel < 10)
                {
                    MergeWith(otherFruit);
                }
                // レベル10（最大の果物同士）なら消滅させる特別処理
                else if (this.fruitLevel == 10)
                {
                    VanishMaxFruits(otherFruit);
                }
            }
        }
    }

    void MergeWith(SuikaFruit other)
    {
        this._hasMerged = true;
        other._hasMerged = true;

        Vector3 spawnPosition = (this.transform.position + other.transform.position) / 2f;

        SuikaGameManager.Instance.EvolveFruit(this.fruitLevel + 1, spawnPosition);

        Destroy(this.gameObject);
        Destroy(other.gameObject);
    }

    // 追加：最大の果物（スイカ）同士がぶつかった時の処理
    void VanishMaxFruits(SuikaFruit other)
    {
        this._hasMerged = true;
        other._hasMerged = true;

        // ダブルスイカ達成の特別ボーナススコア！（点数は自由に変更してください）
        SuikaGameManager.Instance.AddScore(1000); 

        // 進化はさせず、両方ともシーンから削除して箱のスペースを空ける
        Destroy(this.gameObject);
        Destroy(other.gameObject);
        
        Debug.Log("最大の果物同士が合体して消滅しました！ボーナスゲット！");
    }
}