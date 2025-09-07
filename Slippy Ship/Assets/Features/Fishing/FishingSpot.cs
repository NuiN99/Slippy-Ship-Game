using UnityEngine;

public class FishingSpot : MonoBehaviour
{
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
        }
    }

    void OnEnable()
    {
        GameEvents.OnFishDepleted += DepleteFish;
    }

    void OnDisable()
    {
        GameEvents.OnFishDepleted -= DepleteFish;
        FishingManager.Instance.SetIsInFishingSpot(false);
    }

    void OnTriggerStay(Collider other)
    {
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
            FishingManager.Instance.SpawnNewFishingSpot();
            Destroy(gameObject);
        }
    }
}
