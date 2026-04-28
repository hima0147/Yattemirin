using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン移動（リトライ）に必要
using UnityEngine.EventSystems;

public class BurgerSpawner : MonoBehaviour
{
    public float moveLimitX = 2.0f; 
    public GameObject[] ingredientPrefabs;
    public GameObject topBunPrefab;
    public TextMeshProUGUI scoreText;
    public GameObject retryPopup; // ★追加：リトライ画面の親オブジェクト

    private GameObject currentIngredient; 
    private bool isWaiting = false;
    private bool isGameOver = false;
    private bool isSettingTopBun = false; // ★追加：今、上バンズを狙っている最中か

    void Start()
    {
        if (scoreText != null) scoreText.text = "";
        if (retryPopup != null) retryPopup.SetActive(false); // 最初は隠しておく
        PrepareNextIngredient();
    }

    void Update()
    {
        if (isWaiting || (isGameOver && !isSettingTopBun)) return;

        // ★追加：もしマウス（指）がUI（ボタンなど）の上にある時は、ゲームの移動・落下操作を無視する！
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 移動処理
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; 
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(mousePos);
            float newX = Mathf.Clamp(touchPos.x, -moveLimitX, moveLimitX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        // 離した時の処理
        if (Input.GetMouseButtonUp(0) && currentIngredient != null)
        {
            if (isSettingTopBun)
            {
                // 上バンズを狙っている時に離したら、ゲーム終了へ
                DropTopBun();
            }
            else
            {
                DropIngredient();
            }
        }
    }

    void PrepareNextIngredient()
    {
        if (isGameOver) return;
        int randomIndex = Random.Range(0, ingredientPrefabs.Length);
        SpawnIngredient(ingredientPrefabs[randomIndex]);
        isWaiting = false; 
    }

    void SpawnIngredient(GameObject prefab)
    {
        currentIngredient = Instantiate(prefab, transform.position, Quaternion.identity);
        currentIngredient.transform.SetParent(this.transform);
        Rigidbody2D rb = currentIngredient.GetComponent<Rigidbody2D>();
        if (rb != null) { rb.gravityScale = 0f; rb.linearVelocity = Vector2.zero; }
        Collider2D col = currentIngredient.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    void DropIngredient()
    {
        isWaiting = true; 
        currentIngredient.transform.SetParent(null);
        Rigidbody2D rb = currentIngredient.GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 1f;
        Collider2D col = currentIngredient.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        currentIngredient = null;
        Invoke("PrepareNextIngredient", 1.0f);
    }

    // 「完成！」ボタンやセンサーから呼ばれる
    public void FinishBurger()
    {
        if (isGameOver) return;
        isGameOver = true;
        isSettingTopBun = true; // 上バンズ狙いモード開始

        // ★修正：操作不能ロックを強制解除し、裏で動いている具材補充タイマーも止める
        isWaiting = false;
        CancelInvoke("PrepareNextIngredient");

        // 今持っている具材を消して、代わりに上バンズを手に持たせる
        if (currentIngredient != null) Destroy(currentIngredient);
        SpawnIngredient(topBunPrefab);
    }

    void DropTopBun()
    {
        isSettingTopBun = false; // 狙いモード終了
        
        currentIngredient.transform.SetParent(null);
        Rigidbody2D rb = currentIngredient.GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 1f;
        Collider2D col = currentIngredient.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        currentIngredient = null;

        // スコア表示へ
        Invoke("CalculateScore", 2.0f);
    }

    void CalculateScore()
    {
        GameObject[] allParts = GameObject.FindGameObjectsWithTag("Ingredient");
        int count = 0;
        foreach (GameObject part in allParts)
        {
            if (part.transform.position.y > -4.0f && Mathf.Abs(part.transform.position.x) < 1.5f)
            {
                count++;
            }
        }
        int finalScore = count * 100;
        if (scoreText != null) scoreText.text = "完成！\n" + finalScore + " 点！";

        // ★追加：1秒後にポップアップを表示
        Invoke("ShowRetryPopup", 1.0f);
    }

    void ShowRetryPopup()
    {
        if (retryPopup != null) retryPopup.SetActive(true);
    }

    // ★追加：リトライボタンから呼ばれる関数
    public void RetryGame()
    {
        // 今のシーンを最初から読み直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}