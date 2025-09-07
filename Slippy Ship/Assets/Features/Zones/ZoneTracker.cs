using System;
using NuiN.NExtensions;
using UnityEngine;

public class ZoneTracker : MonoBehaviour
{
    public static ZoneTracker Instance { get; private set; }
    public OceanZone CurrentZone { get; private set; }

    // must be a child of this object, also must set to trigger and only allow colliding with players boat
    [SerializeField] EnumDictionary<OceanZone, CollisionEventDispatcher> zoneVolumes;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        foreach (OceanZone value in Enum.GetValues(typeof(OceanZone)))
        {
            zoneVolumes[value].TriggerEnter += _ => OnEnteredZone(value);
        }
        
        OnEnteredZone(OceanZone.Calm);
    }

    void OnEnteredZone(OceanZone newZone)
    {
        if (CurrentZone == newZone) return;
        CurrentZone = newZone;
        GameEvents.InvokeZoneChanged(newZone);
        Debug.Log("Changed zone: " + newZone);
    }

    void OnDrawGizmos()
    {
        foreach (OceanZone value in Enum.GetValues(typeof(OceanZone)))
        {
            Gizmos.color = value switch
            {
                OceanZone.Calm => Color.cyan,
                OceanZone.Choppy => Color.yellow,
                OceanZone.Deadly => Color.red,
                _ => Color.white
            };
            
            Transform zoneTransform = zoneVolumes[value].transform;
            Gizmos.DrawWireCube(zoneTransform.position, zoneTransform.localScale);
        }
    }
}
