using UnityEngine;

public class FishKillbox : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Fish fish))
        {
            Destroy(fish.gameObject);
        }
    }
}
