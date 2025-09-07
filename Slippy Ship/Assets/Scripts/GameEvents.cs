using System;

public static class GameEvents
{
    public static event Action<OceanZone> OnZoneChanged = delegate { };
    public static void InvokeZoneChanged(OceanZone newZone) => OnZoneChanged.Invoke(newZone);
}
