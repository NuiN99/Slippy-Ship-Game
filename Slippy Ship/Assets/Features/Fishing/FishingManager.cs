using NuiN.NExtensions;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance { get; private set; }
    [ShowInInspector] public bool IsInFishingSpot { get; private set; }

    [SerializeField] FishingSpot fishingSpotPrefab;
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

    public void SetIsInFishingSpot(bool isIn)
    {
        IsInFishingSpot = isIn;
    }

    public Fish GetCurrentZoneFishPrefab()
    {
        return zoneFishPrefabs[ZoneTracker.Instance.CurrentZone];
    }

    public void SpawnNewFishingSpot()
    {
        FishingSpot newSpot = Instantiate(fishingSpotPrefab);
    }
}