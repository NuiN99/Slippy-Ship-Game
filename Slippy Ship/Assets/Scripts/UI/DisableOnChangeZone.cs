using UnityEngine;

public class DisableOnChangeZone : MonoBehaviour
{
    void OnEnable()
    {
        GameEvents.OnZoneChanged += Disable;
    }

    void OnDisable()
    {
        GameEvents.OnZoneChanged -= Disable;
    }

    void Disable(OceanZone _)
    {
        gameObject.SetActive(false);
    }
}
