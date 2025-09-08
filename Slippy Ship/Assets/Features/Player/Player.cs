using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    void OnEnable()
    {
        Instance = this;
    }
}
