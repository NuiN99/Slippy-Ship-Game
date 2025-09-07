using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image crosshair;
    [SerializeField] Color normalColor;
    [SerializeField] Color hoveringColor;
    [SerializeField] float hoveringScaleMult = 1.5f;

    Vector2 _initialScale;
    
    void Awake()
    {
        _initialScale = crosshair.transform.localScale;
    }

    void LateUpdate()
    {
        crosshair.transform.localScale = _initialScale;
        crosshair.color = normalColor;
        
        if (PlayerInteraction.Instance == null || !PlayerInteraction.Instance.IsHovering) return;
        
        crosshair.transform.localScale = _initialScale * hoveringScaleMult;
        crosshair.color = hoveringColor;
    }
}
