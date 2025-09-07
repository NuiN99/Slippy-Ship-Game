using NuiN.NExtensions;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance { get; private set; }
    [ShowInInspector] public bool IsInFishingSpot { get; private set; }

    [SerializeField] FishingSpot fishingSpotPrefab;
    [SerializeField] float fishingSpotSpawnBuffer = 50f;
    [SerializeField] EnumDictionary<OceanZone, Fish> zoneFishPrefabs;
    [SerializeField] EnumDictionary<OceanZone, FloatRange> fishingSpotSpawnDistanceRanges;

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

    public void SpawnNewFishingSpot(Vector3 oldPos)
    {
        Transform zoneTransform = ZoneTracker.Instance.GetCurrentZoneTransform();
        Vector3 zoneCenter = zoneTransform.position;
        Vector3 halfExtents = zoneTransform.localScale * 0.5f;

        halfExtents.x -= fishingSpotSpawnBuffer;
        halfExtents.z -= fishingSpotSpawnBuffer;

        const int maxAttempts = 20;
        Vector3 newPos = oldPos;

        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            float dist = fishingSpotSpawnDistanceRanges[ZoneTracker.Instance.CurrentZone].Random();

            Vector3 candidate = oldPos + dir * dist;

            if (Mathf.Abs(candidate.x - zoneCenter.x) <= halfExtents.x &&
                Mathf.Abs(candidate.z - zoneCenter.z) <= halfExtents.z)
            {
                newPos = candidate;
                break;
            }
        }

        FishingSpot newSpot = Instantiate(fishingSpotPrefab, newPos, Quaternion.identity);
    }
}