using NuiN.NExtensions;
using NuiN.SpleenTween;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterSurfaceZoneValueChanger : MonoBehaviour
{
    [SerializeField] WaterSurface waterSurface;
    [SerializeField] EnumDictionary<OceanZone, WaterSurfaceZoneDataSO> waterZoneDatas;
    [SerializeField] float valueChangeTweenDuration = 5f;
    [SerializeField] Ease valueChangeTweenEase;
    
    ITween _valueChangeTween;

    void Awake()
    {
        SetZoneDataValuesInstant(OceanZone.Calm);
    }

    void OnEnable()
    {
        GameEvents.OnZoneChanged += OnZoneChanged;
    }

    void OnDisable()
    {
        GameEvents.OnZoneChanged -= OnZoneChanged;
    }

    void OnZoneChanged(OceanZone newZone)
    {
        var newData = waterZoneDatas[newZone].Data;
        
        

        _valueChangeTween?.Stop();
        _valueChangeTween = SpleenTween.Value0To1(valueChangeTweenDuration, lerp =>
        {
            waterSurface.timeMultiplier = Mathf.LerpUnclamped(waterSurface.timeMultiplier, newData.timeMultiplier, lerp);
            waterSurface.largeWindSpeed = Mathf.LerpUnclamped(waterSurface.largeWindSpeed, newData.distantWindSpeed, lerp);
            waterSurface.largeBand1Multiplier = Mathf.LerpUnclamped(waterSurface.largeBand1Multiplier, newData.secondBandAmplitudeDimmer, lerp);
            waterSurface.ripplesWindSpeed = Mathf.LerpUnclamped(waterSurface.ripplesWindSpeed, newData.localWindSpeed, lerp);
            
            waterSurface.largeCurrentSpeedValue = Mathf.LerpUnclamped(waterSurface.largeCurrentSpeedValue, newData.currentSpeed, lerp);
            waterSurface.repetitionSize = Mathf.LerpUnclamped(waterSurface.repetitionSize, newData.repetitionSize, lerp);
        }).SetEase(valueChangeTweenEase);
    }

    void SetZoneDataValuesInstant(OceanZone newZone)
    {
        var data = waterZoneDatas[newZone].Data;
        waterSurface.timeMultiplier = data.timeMultiplier;
        waterSurface.largeWindSpeed = data.distantWindSpeed;
        waterSurface.largeCurrentSpeedValue = data.currentSpeed;
        waterSurface.largeBand1Multiplier = data.secondBandAmplitudeDimmer;
        waterSurface.ripplesWindSpeed = data.localWindSpeed;
        waterSurface.repetitionSize = data.repetitionSize;
    }
}