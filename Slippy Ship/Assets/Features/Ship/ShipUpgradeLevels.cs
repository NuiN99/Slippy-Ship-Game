using System;
using UnityEngine;

[Serializable]
public struct ShipUpgradeLevels
{
    public int rodAmountLevel;
    public int catchAmountLevel;
    public int catchIntervalLevel;
    public int stabilityLevel;
    public int turningLevel;
    public int throttleLevel;

    public ShipUpgradeLevels Add(ShipUpgradeLevels other)
    {
        rodAmountLevel = Mathf.Min(rodAmountLevel + other.rodAmountLevel, 3);
        catchAmountLevel = Mathf.Min(catchAmountLevel + other.catchAmountLevel, 3);
        catchIntervalLevel = Mathf.Min(catchIntervalLevel + other.catchIntervalLevel, 3);
        stabilityLevel = Mathf.Min(stabilityLevel + other.stabilityLevel, 3);
        turningLevel = Mathf.Min(turningLevel + other.turningLevel, 3);
        throttleLevel = Mathf.Min(throttleLevel + other.throttleLevel, 3);

        return this;
    }
}