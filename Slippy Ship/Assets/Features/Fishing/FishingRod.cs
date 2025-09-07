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
        public int minFish;
        public int maxFish;

        public Stats(float onHookDuration, FloatRange catchInterval, int minFish, int maxFish)
        {
            this.onHookDuration = onHookDuration;
            this.catchInterval = catchInterval;
            this.minFish = minFish;
            this.maxFish = maxFish;
        }
    }

    public override bool IsInteractable => _fishIsHooked;

    [SerializeField] Rigidbody boatRB;
    [SerializeField] GameObject hookedObj;
    [SerializeField] Transform fishSpawnPoint;
    [SerializeField] float normalFishRotZ;
    [SerializeField] float hookedFishRotZ;
    [SerializeField] Stats stats;
        
    Coroutine _fishingRoutine;
    bool _fishIsHooked;
    float _initialRotZ;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }
    
    public void Initialize()
    {
        _fishIsHooked = false;
        hookedObj.SetActive(false);
        transform.localRotation = transform.localRotation.With(z: normalFishRotZ);
        
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
            
            yield return new WaitForSeconds(stats.catchInterval.Random());
            _fishIsHooked = true;
            transform.localRotation = transform.localRotation.With(z: hookedFishRotZ);
            hookedObj.SetActive(true);
            
            yield return new WaitForSeconds(stats.onHookDuration);
            _fishIsHooked = false;
            hookedObj.SetActive(false);
            transform.localRotation = transform.localRotation.With(z: normalFishRotZ);
            
            GameEvents.InvokeFishDepleted();
        }
    }
    

    public override void Interact()
    {
        base.Interact();
        _fishIsHooked = false;
        hookedObj.SetActive(false);
        SpawnFish();
        transform.localRotation = transform.localRotation.With(z: normalFishRotZ);
        
        this.StopCoroutineSafe(_fishingRoutine);
        _fishingRoutine = StartCoroutine(FishingRoutine());
    }

    void SpawnFish()
    {
        int numFish = Random.Range(stats.minFish, stats.maxFish + 1);

        for (int i = 0; i < numFish; i++)
        {
            Fish fishPrefab = FishingManager.Instance.GetCurrentZoneFishPrefab();
            Fish spawnedFish = Instantiate(fishPrefab, fishSpawnPoint.position, Random.rotation);
            spawnedFish.SetVelocity(boatRB.linearVelocity + transform.up * 5f);
        }
        
        GameEvents.InvokeFishDepleted();
    }
}
