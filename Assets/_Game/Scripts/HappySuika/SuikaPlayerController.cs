using UnityEngine;

public class SuikaPlayerController : MonoBehaviour
{
    [Header("落とす候補の果物（レベル0〜4くらいを登録）")]
    public GameObject[] dropFruitPrefabs;

    [Header("落下設定")]
    public float spawnY = 5.0f;
    public float limitX = 2.5f;

    [Header("落下予測線（ガイドライン）")]
    public LineRenderer guideLine;

    private GameObject _currentFruit;
    private Rigidbody2D _currentRb;
    private bool _canDrop = false;
    private int _nextFruitIndex;

    void Start()
    {
        if (guideLine != null) guideLine.enabled = false;
    }

    void Update()
    {
        if (!SuikaGameManager.Instance.IsPlaying) return;

        if (!_canDrop || _currentFruit == null) return;

        // 常に現在の果物の位置から真下へ線を引く
        UpdateGuideLine();

        // 画面をタッチしている間は、指のX座標に合わせて果物が動く
        if (Input.GetMouseButton(0))
        {
            MoveFruit();
        }
        // 指を離した瞬間に落下する
        else if (Input.GetMouseButtonUp(0))
        {
            DropFruit();
        }
    }

    // ゲーム開始時にGameManagerから呼ばれる
    public void StartGame()
    {
        // 最初の「ネクスト」を裏でランダムに決める
        _nextFruitIndex = Random.Range(0, dropFruitPrefabs.Length);
        
        // すぐに「現在の果物」として画面上部に出現させる
        PrepareCurrentFruit();
    }

    void PrepareCurrentFruit()
    {
        // 記憶していた「ネクスト」を上部に出現させる
        _currentFruit = Instantiate(dropFruitPrefabs[_nextFruitIndex]);
        _currentFruit.transform.position = new Vector3(0f, spawnY, 0f); // 最初は真ん中に待機

        _currentRb = _currentFruit.GetComponent<Rigidbody2D>();
        _currentRb.gravityScale = 0f; // まだ落ちない
        _currentRb.freezeRotation = true; // 回転させない

        // すぐに次の「ネクスト」を新たに決めてUIを更新する
        _nextFruitIndex = Random.Range(0, dropFruitPrefabs.Length);
        UpdateNextFruitUI();

        if (guideLine != null) guideLine.enabled = true; // 線を表示
        _canDrop = true; // 操作可能にする
    }

    void UpdateNextFruitUI()
    {
        Sprite nextSprite = dropFruitPrefabs[_nextFruitIndex].GetComponent<SpriteRenderer>().sprite;
        SuikaGameManager.Instance.uiManager.UpdateNextFruitIcon(nextSprite);
    }

    void MoveFruit()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mousePos.x, -limitX, limitX);
        _currentFruit.transform.position = new Vector3(clampedX, spawnY, 0f);
    }

    void UpdateGuideLine()
    {
        if (guideLine != null && _currentFruit != null)
        {
            Vector3 pos = _currentFruit.transform.position;
            guideLine.SetPosition(0, pos);
            guideLine.SetPosition(1, new Vector3(pos.x, -10f, 0f));
        }
    }

    void DropFruit()
    {
        _currentRb.gravityScale = 1f; // 重力をオンにして落下！
        _currentRb.freezeRotation = false;

        _currentFruit = null;
        _canDrop = false;

        if (guideLine != null) guideLine.enabled = false; // 落下中は線を消す

        // 1.5秒後に再び次の果物を上部に準備する
        Invoke(nameof(PrepareCurrentFruit), 1.5f);
    }
}