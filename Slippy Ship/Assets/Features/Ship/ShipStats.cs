using System;
using NuiN.NExtensions;

[Serializable]
public struct ShipStats
{
    public int numRods;
    public IntRange catchAmount;
    public FloatRange catchInterval;
    public float stability;
    public float turningAccel;
    public float turningForce;
    public float throttleForce;
    public float throttleAccel;
}