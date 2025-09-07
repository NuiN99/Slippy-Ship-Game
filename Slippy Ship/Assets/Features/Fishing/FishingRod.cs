using System;
using System.Collections;
using NuiN.NExtensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingRod : BaseInteractable
{
    [Serializable]
    public struct Stats
    {
        public float onHookDuration;
        public FloatRange catchInterval;

        public Stats(float onHookDuration, FloatRange catchInterval)
        {
            this.onHookDuration = onHookDuration;
            this.catchInterval = catchInterval;
        }
    }

    public override bool IsInteractable => _fishIsHooked;

    [SerializeField] Rigidbody boatRB;
    [SerializeField] Transform fishSpawnPoint;
    [SerializeField] Stats stats;
        
    Coroutine _fishingRoutine;
    bool _fishIsHooked;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }
    
    public void Initialize()
    {
        _fishIsHooked = false;
        
        this.StopCoroutineSafe(_fishingRoutine);
        _fishingRoutine = StartCoroutine(FishingRoutine());
    }

    IEnumerator FishingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stats.catchInterval.Random());
            _fishIsHooked = true;
            yield return new WaitForSeconds(stats.onHookDuration);
            _fishIsHooked = false;
        }
    }
    

    public override void Interact()
    {
        base.Interact();
        _fishIsHooked = false;
        SpawnFish();
        
        this.StopCoroutineSafe(_fishingRoutine);
        _fishingRoutine = StartCoroutine(FishingRoutine());
    }

    void SpawnFish()
    {
        Fish fishPrefab = FishingManager.Instance.GetCurrentZoneFishPrefab();
        Fish spawnedFish = Instantiate(fishPrefab, fishSpawnPoint.position, Random.rotation);
        
        spawnedFish.SetVelocity(boatRB != null ? boatRB.linearVelocity : Vector3.zero);
    }
}
