using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    [SerializeField] ShipEngine engine;

    void Update()
    {
        float throttleInput = PlayerInputManager.Controls.Actions.ShipThrottle.ReadValue<float>();
        float steeringInput = PlayerInputManager.Controls.Actions.ShipSteering.ReadValue<float>();

        engine.AdjustThrottle(throttleInput, Time.deltaTime);
        engine.AdjustSteering(steeringInput, Time.deltaTime);
    }
}
