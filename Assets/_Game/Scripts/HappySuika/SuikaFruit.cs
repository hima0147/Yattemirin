using UnityEngine;

public class SuikaFruit : MonoBehaviour
{
    [Header("果物のレベル (0:ポテト 〜 10:スイカ)")]
    public int fruitLevel = 0;
    
    [HideInInspector] public bool hasDropped = false;
    [HideInInspector] public bool isMerging = false;
}