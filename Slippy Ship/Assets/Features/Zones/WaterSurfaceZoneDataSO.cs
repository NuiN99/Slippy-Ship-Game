using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Water/Zone Surface Data")]
public class WaterSurfaceZoneDataSO : ScriptableObject
{
    [Serializable]
    public struct WaterSurfaceZoneData
    {
        public float timeMultiplier;
        public float distantWindSpeed;
        public float currentSpeed;
        public float secondBandAmplitudeDimmer;
        public float localWindSpeed;
        public float repetitionSize;
    }
    
    [field: SerializeField] public WaterSurfaceZoneData Data { get; private set; }
}