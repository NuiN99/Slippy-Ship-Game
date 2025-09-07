using System.Collections;
using NuiN.NExtensions;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [field: SerializeField] public int SellAmount { get; private set; } = 1;

    [SerializeField] Rigidbody rb;
    [SerializeField] FloatRange flopDuration;
    [SerializeField] FloatRange flopInterval;
    [SerializeField] FloatRange flopForce;
    [SerializeField] Transform tailTransform;
    [SerializeField] float fishScaleVariance;
    [SerializeField] float gravity = -10f;

    Timer _flopIntervalTimer;

    bool _isGrounded;
    Vector3 _groundNormal;

    protected void Awake()
    {
        _flopIntervalTimer = new Timer(flopInterval.Random());

        float scaleVariance = Random.Range(1 - fishScaleVariance, 1 + fishScaleVariance);
        transform.localScale *= scaleVariance;
        rb.mass *= scaleVariance * 2f;
    }

    IEnumerator Start()
    {
        float floppingTime = flopDuration.Random();
        while (floppingTime > 0)
        {
            floppingTime -= Time.deltaTime;
            
            if (_flopIntervalTimer.IsComplete && _isGrounded)
            {
                _flopIntervalTimer.Restart();
                Flop();
            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }

    void Flop()
    {
        Vector3 dir = _groundNormal + (Random.insideUnitSphere.normalized * 0.25f);
        Vector3 force = dir * flopForce.Random();
        rb.AddForceAtPosition(force, tailTransform.position, ForceMode.VelocityChange);
    }

    void OnCollisionStay(Collision other)
    {
        _isGrounded = true;
        _groundNormal = other.AverageNormal();
    }

    void OnCollisionExit(Collision other)
    {
        _isGrounded = false;
    }

    public void SetVelocity(Vector3 vel)
    {
        rb.linearVelocity = vel;
    }
}