using FMOD.Studio;
using UnityEngine;

public class ZoneMusic : MonoBehaviour
{
    [SerializeField] FMODSoundPlayer music;

    EventInstance _instance;

    void OnEnable()
    {
        _instance = music.PlayEvent();

        GameEvents.OnZoneChanged += OnZoneChanged;
    }

    void OnDisable()
    {
        music.StopEvent(_instance);
        
        GameEvents.OnZoneChanged -= OnZoneChanged;
    }

    void OnZoneChanged(OceanZone newZone)
    {
        int zone = (int)newZone;
        _instance.setParameterByName("Zone", zone);
    }
}
