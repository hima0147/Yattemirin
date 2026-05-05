using UnityEngine;

public class SuikaPlayerController : MonoBehaviour
{
    [Header("果物プレハブ（テスト用に0番と1番を登録）")]
    public GameObject[] fruitPrefabs;

    [Header("落下設定")]
    public float spawnY = 5.0f; // 果物が出現する高さ（Y座標）
    public float limitX = 2.5f; // 左右に動ける限界の幅（X座標）

    private GameObject _currentFruit;
    private Rigidbody2D _currentRb;
    private bool _canDrop = true;

    void Update()
    {
        // ゲーム中じゃない時（タイトル画面など）は操作させない
        if (!SuikaGameManager.Instance.IsPlaying) return;

        if (!_canDrop) return;

        // 画面をタッチ（クリック）した瞬間
        if (Input.GetMouseButtonDown(0))
        {
            SpawnFruit();
        }
        // タッチしたまま指を動かしている間
        else if (Input.GetMouseButton(0) && _currentFruit != null)
        {
            MoveFruit();
        }
        // 指を離した瞬間
        else if (Input.GetMouseButtonUp(0) && _currentFruit != null)
        {
            DropFruit();
        }
    }

    void SpawnFruit()
    {
        // まずはテストで、0番か1番の果物をランダムに出す
        int randomIndex = Random.Range(0, 2);
        _currentFruit = Instantiate(fruitPrefabs[randomIndex]);

        _currentRb = _currentFruit.GetComponent<Rigidbody2D>();
        
        // 生成された瞬間は落ちないように重力を0にする
        _currentRb.gravityScale = 0f;
        // 物理演算の回転も一旦止める（指で動かす時にプルプルしないように）
        _currentRb.freezeRotation = true;

        MoveFruit(); // すぐに指の位置へ移動させる
    }

    void MoveFruit()
    {
        // 画面のタッチ位置を、ゲーム内の座標に変換
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 左右の壁を突き抜けないように制限をかける
        float clampedX = Mathf.Clamp(mousePos.x, -limitX, limitX);

        // 果物の位置を更新（高さは固定）
        _currentFruit.transform.position = new Vector3(clampedX, spawnY, 0f);
    }

    void DropFruit()
    {
        // 重力を1に戻して落下させる！
        _currentRb.gravityScale = 1f;
        // 回転の固定も解除して、自然に転がるようにする
        _currentRb.freezeRotation = false;

        _currentFruit = null;
        _canDrop = false; // 連続で落とせないようにする

        // 1.5秒後に再び次の果物を落とせるようにする（クールダウン）
        Invoke(nameof(ResetDrop), 1.5f);
    }

    void ResetDrop()
    {
        _canDrop = true;
    }
}
