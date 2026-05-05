using UnityEngine;
using UnityEngine.EventSystems; // 追加：UIをタッチしているかの判定に必要

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
    private int _debugFruitIndex = 0;

    // 追加：UIを操作中かどうかのフラグ
    private bool _isInteractingWithUI = false;

    void Start()
    {
        if (guideLine != null) guideLine.enabled = false;
    }

    void Update()
    {
        if (!SuikaGameManager.Instance.IsPlaying) return;
        if (!_canDrop || _currentFruit == null) return;

        UpdateGuideLine();

        // 1. 画面をタッチした瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // イベントシステムを使って、指（マウス）の下にUIがあるかチェック
            // （※エディタのクリックと、実機のタッチの両方に対応する安全な書き方です）
            if (EventSystem.current.IsPointerOverGameObject() || 
               (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                _isInteractingWithUI = true; // UI操作中としてマーク
                return; // ここで処理を止める（果物は動かさない）
            }
        }

        // 2. 指を離した瞬間
        if (Input.GetMouseButtonUp(0))
        {
            // UI操作中だった場合は、落とさずにフラグをリセットして終了
            if (_isInteractingWithUI)
            {
                _isInteractingWithUI = false;
                return;
            }
            
            DropFruit();
        }

        // 3. タッチしたまま指を動かしている間
        if (Input.GetMouseButton(0))
        {
            // UI操作中でなければ果物を追従させる
            if (!_isInteractingWithUI)
            {
                MoveFruit();
            }
        }
    }

    public void StartGame()
    {
        _nextFruitIndex = Random.Range(0, dropFruitPrefabs.Length);
        _debugFruitIndex = 0;
        PrepareCurrentFruit();
    }

    void PrepareCurrentFruit()
    {
        if (SuikaGameManager.Instance.isDebugMode)
        {
            _currentFruit = Instantiate(SuikaGameManager.Instance.allFruitPrefabs[_debugFruitIndex]);
        }
        else
        {
            _currentFruit = Instantiate(dropFruitPrefabs[_nextFruitIndex]);
            _nextFruitIndex = Random.Range(0, dropFruitPrefabs.Length);
        }

        _currentFruit.transform.position = new Vector3(0f, spawnY, 0f);
        _currentRb = _currentFruit.GetComponent<Rigidbody2D>();
        _currentRb.gravityScale = 0f;
        _currentRb.freezeRotation = true;

        UpdateNextFruitUI();

        if (guideLine != null) guideLine.enabled = true;
        _canDrop = true;
        _isInteractingWithUI = false; // 念のためここでもリセット
    }

    public void CycleDebugFruit()
    {
        if (!SuikaGameManager.Instance.isDebugMode || _currentFruit == null || !_canDrop) return;

        _debugFruitIndex++;
        if (_debugFruitIndex >= SuikaGameManager.Instance.allFruitPrefabs.Length)
        {
            _debugFruitIndex = 0;
        }

        Vector3 currentPos = _currentFruit.transform.position;
        Destroy(_currentFruit);

        _currentFruit = Instantiate(SuikaGameManager.Instance.allFruitPrefabs[_debugFruitIndex]);
        _currentFruit.transform.position = currentPos;
        _currentRb = _currentFruit.GetComponent<Rigidbody2D>();
        _currentRb.gravityScale = 0f;
        _currentRb.freezeRotation = true;

        UpdateNextFruitUI();
    }

    void UpdateNextFruitUI()
    {
        Sprite nextSprite;
        if (SuikaGameManager.Instance.isDebugMode)
        {
            nextSprite = SuikaGameManager.Instance.allFruitPrefabs[_debugFruitIndex].GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            nextSprite = dropFruitPrefabs[_nextFruitIndex].GetComponent<SpriteRenderer>().sprite;
        }
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
        _currentRb.gravityScale = 1f;
        _currentRb.freezeRotation = false;

        _currentFruit = null;
        _canDrop = false;

        if (guideLine != null) guideLine.enabled = false;

        Invoke(nameof(PrepareCurrentFruit), 1.5f);
    }
}