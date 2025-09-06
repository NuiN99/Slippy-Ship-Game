using UnityEngine;

public class SceneCursorOptions : MonoBehaviour
{
    enum UpdateType
    {
        Constant,
        Start
    }
    
    [SerializeField] CursorLockMode lockState;
    [SerializeField] bool visible;

    [SerializeField] UpdateType updateType;
    
    void Start()
    {
        if (updateType != UpdateType.Start) return;
        
        Cursor.lockState = lockState;
        Cursor.visible = visible;
    }

    void Update()
    {
        if (updateType != UpdateType.Constant) return;
        
        Cursor.lockState = lockState;
        Cursor.visible = visible;
    }
    
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RestoreCursorUponExitPlaymode()
    {
        Application.quitting += RestoreCursor;
    }

    static void RestoreCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Application.quitting -= RestoreCursor;
    }
#endif
}
