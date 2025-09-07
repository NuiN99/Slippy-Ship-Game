using NuiN.NExtensions;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance { get; private set; }

    [SerializeField] EnumDictionary<OceanZone, Fish> zoneFishPrefabs;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Fish GetCurrentZoneFishPrefab()
    {
        return zoneFishPrefabs[ZoneTracker.Instance.CurrentZone];
    }
}