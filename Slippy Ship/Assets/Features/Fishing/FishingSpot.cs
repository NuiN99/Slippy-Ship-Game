using NuiN.NExtensions;
using NuiN.SpleenTween;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FishingSpot : MonoBehaviour
{
    [SerializeField] WaterDeformer waterDeformer;
    [SerializeField] Transform birdsRoot;
    [SerializeField] int minFish;
    [SerializeField] int maxFish;

    [Header("Circling")]
    [SerializeField] float birdsCirclingSpeed = 10f;

    [Header("Bobbing")]
    [SerializeField] float birdsBobbingSpeed = 1f;
    [SerializeField] float birdsBobbingHeight = 0.5f;

    [Header("Positioning")]
    [SerializeField] float birdSpawnRadius = 5f;
    [SerializeField] float birdMinHeight = 3f;
    [SerializeField] float birdMaxHeight = 6f;

    bool _playerIsInZone;
    Vector3[] _initialPositions;
    int _fishLeft;

    void Awake()
    {
        _fishLeft = Random.Range(minFish, maxFish + 1);        
        
        int count = birdsRoot.childCount;
        _initialPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            Transform bird = birdsRoot.GetChild(i);

            Vector2 randomCircle = Random.insideUnitCircle * birdSpawnRadius;

            float randomHeight = Random.Range(birdMinHeight, birdMaxHeight);

            Vector3 newLocalPos = new Vector3(randomCircle.x, randomHeight, randomCircle.y);

            bird.localPosition = newLocalPos;
            _initialPositions[i] = newLocalPos;
            
            bird.rotation = Quaternion.LookRotation(VectorUtils.Direction(bird.position, birdsRoot.position.With(y:0)));
        }
    }

    void Start()
    {
        SpleenTween.PosAxis(transform, Axis.y, 250, 0f, 2.5f).SetEase(Ease.OutCubic);
    }

    void OnEnable()
    {
        GameEvents.OnFishDepleted += DepleteFish;
        GameEvents.OnZoneChanged += OnZoneChanged;
    }

    void OnDisable()
    {
        GameEvents.OnFishDepleted -= DepleteFish;
        GameEvents.OnZoneChanged -= OnZoneChanged;
        
        FishingManager.Instance.SetIsInFishingSpot(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (_fishLeft <= 0)
        {
            _playerIsInZone = false;
            FishingManager.Instance.SetIsInFishingSpot(false);
            return;
        }
        
        _playerIsInZone = true;
        FishingManager.Instance.SetIsInFishingSpot(true);
    }

    void OnTriggerExit(Collider other)
    {
        _playerIsInZone = false;
        FishingManager.Instance.SetIsInFishingSpot(false);
    }

    void Update()
    {
        birdsRoot.Rotate(Vector3.up, birdsCirclingSpeed * Time.deltaTime, Space.Self);

        for (int i = 0; i < birdsRoot.childCount; i++)
        {
            Transform bird = birdsRoot.GetChild(i);
            Vector3 basePos = _initialPositions[i];

            float phase = i * 1.37f;

            float bob = Mathf.Sin(Time.time * birdsBobbingSpeed + phase) * birdsBobbingHeight;

            bird.localPosition = new Vector3(basePos.x, basePos.y + bob, basePos.z);
        }
    }

    void DepleteFish()
    {
        if (!_playerIsInZone) return;
        _fishLeft--;

        if (_fishLeft <= 0)
        {
            TweenAndSpawnNew();
        }
    }

    void OnZoneChanged(OceanZone newZone)
    {
        TweenAndSpawnNew();
    }

    void TweenAndSpawnNew()
    {
        waterDeformer.deepFoamDimmer = 0f;
        waterDeformer.surfaceFoamDimmer = 0f;
        SpleenTween.AddPosAxis(transform, Axis.y, 250f, 5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            FishingManager.Instance.SpawnNewFishingSpot(Player.Instance.transform.position);
            Destroy(gameObject);
        });
    }
}
