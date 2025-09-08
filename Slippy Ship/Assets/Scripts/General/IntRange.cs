using System;
using NuiN.NExtensions;
using UnityEngine;

[Serializable]
public class IntRange
{
    [field: SerializeField] public int Min { get; private set; }
    [field: SerializeField] public int Max { get; private set; }

    public float Lerp(float lerp) => Mathf.Lerp(Min, Max, lerp);
    public int Random() => UnityEngine.Random.Range(Min, Max + 1);

    public IntRange(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
