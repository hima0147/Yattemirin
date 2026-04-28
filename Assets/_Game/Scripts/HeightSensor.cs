using UnityEngine;

public class HeightSensor : MonoBehaviour
{
    // Enter（入った瞬間）ではなく Stay（センサーの中にいる間ずっと）に変更
    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        
        // 具材が落ちている最中ではなく、積み上がって「ほぼ止まっている」状態かチェック
        if (rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.1f) 
        {
            BurgerSpawner spawner = FindFirstObjectByType<BurgerSpawner>();
            if (spawner != null)
            {
                spawner.FinishBurger();
            }
        }
    }
}
