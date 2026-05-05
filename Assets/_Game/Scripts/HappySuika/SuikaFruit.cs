using UnityEngine;

public class SuikaFruit : MonoBehaviour
{
    [Header("果物のレベル (0=ポテト, 1=次, ... 10=最大)")]
    public int fruitLevel = 0;

    private bool _hasMerged = false; // 同時に2つ進化してしまうのを防ぐフラグ

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 自分がすでに合体処理に入っている、またはゲーム中ではない場合は無視
        if (_hasMerged || !SuikaGameManager.Instance.IsPlaying) return;

        // ぶつかった相手が果物かどうかを確認
        SuikaFruit otherFruit = collision.gameObject.GetComponent<SuikaFruit>();
        if (otherFruit != null)
        {
            // 相手がまだ合体処理に入っておらず、かつ自分と同じレベルの果物かチェック
            if (!otherFruit._hasMerged && this.fruitLevel == otherFruit.fruitLevel)
            {
                // 最大レベル（10）同士の合体はこれ以上進化しないので処理を分ける場合もありますが、
                // 今回はレベル10未満なら合体させる
                if (this.fruitLevel < 10)
                {
                    MergeWith(otherFruit);
                }
            }
        }
    }

    void MergeWith(SuikaFruit other)
    {
        // 相手と自分を「合体済み」にして、二重に判定されるのを防ぐ
        this._hasMerged = true;
        other._hasMerged = true;

        // 2つの果物の「中間地点」を計算して、新しい果物の出現位置にする
        Vector3 spawnPosition = (this.transform.position + other.transform.position) / 2f;

        // GameManagerに「この位置で、次のレベルの果物を出して！」と依頼する
        SuikaGameManager.Instance.EvolveFruit(this.fruitLevel + 1, spawnPosition);

        // 自分と相手の果物をシーンから削除する
        Destroy(this.gameObject);
        Destroy(other.gameObject);
    }
}
