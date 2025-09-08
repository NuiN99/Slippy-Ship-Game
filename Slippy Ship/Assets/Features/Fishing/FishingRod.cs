using System;
using System.Collections;
using NuiN.NExtensions;
using NuiN.SpleenTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingRod : MonoBehaviour
{
    [Serializable]
    public struct Stats
    {
        public float onHookDuration;
        public FloatRange catchInterval;
        public IntRange catchAmount;

        public Stats(float onHookDuration, FloatRange catchInterval, IntRange catchAmount)
        {
            this.onHookDuration = onHookDuration;
            this.catchInterval = catchInterval;
            this.catchAmount = catchAmount;
        }
    }

    [SerializeField] Rigidbody boatRB;
    //[SerializeField] GameObject hookedObj;
    [SerializeField] Transform fishSpawnPoint;
    [SerializeField] float normalFishRotZ;
    [SerializeField] float hookedFishRotZ;
    public Stats stats;
        
    Coroutine _fishingRoutine;
    bool _fishIsHooked;
    float _initialRotZ;
    Quaternion _initialRotation;

    ITween _rotTween;

    protected void Awake()
    {
        _initialRotation = transform.localRotation;
        Initialize();
    }
    
    public void Initialize()
    {
        SetHooked(false);
        
        this.StopCoroutineSafe(_fishingRoutine);
        _fishingRoutine = StartCoroutine(FishingRoutine());
    }

    IEnumerator FishingRoutine()
    {
        while (true)
        {
            if (FishingManager.Instance == null || !FishingManager.Instance.IsInFishingSpot)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }
            
            SetHooked(true);
            yield return new WaitForSeconds(stats.onHookDuration);
            SetHooked(false);
            
            yield return new WaitForSeconds(stats.catchInterval.Random());
        }
    }

    void SpawnFish()
    {
        int numFish = stats.catchAmount.Random();

        for (int i = 0; i < numFish; i++)
        {
            Fish fishPrefab = FishingManager.Instance.GetCurrentZoneFishPrefab();
            Fish spawnedFish = Instantiate(fishPrefab, fishSpawnPoint.position, Random.rotation);
            spawnedFish.SetVelocity(boatRB.linearVelocity + transform.up * 5f);
        }
        
        GameEvents.InvokeFishDepleted();
    }
    
    void SetHooked(bool hooked)
    {
        _fishIsHooked = hooked;

        _rotTween?.Stop();

        Quaternion targetEuler = _initialRotation * Quaternion.Euler(0, 0, hooked ? hookedFishRotZ : normalFishRotZ);
        _rotTween = SpleenTween.LocalRot(transform, transform.localRotation, targetEuler, hooked ? 0.1f : 0.25f).SetEase(hooked ? Ease.OutCubic : Ease.InCubic);

        if (hooked)
        {
            _rotTween.OnComplete(SpawnFish);
        }
    }
}
