using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ShipWaterDeformation : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] WaterDeformer waterDeformer;
    [SerializeField] float minDeformation = 0.1f;
    [SerializeField] float maxDeformationVelocity;
    
    void Update()
    {
        waterDeformer.surfaceFoamDimmer = Mathf.Clamp(rb.linearVelocity.magnitude / maxDeformationVelocity, minDeformation, 1f);
    }
}