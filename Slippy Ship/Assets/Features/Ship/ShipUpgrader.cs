using UnityEngine;

public class ShipUpgrader : MonoBehaviour
{
    [SerializeField] ShipEngine engine;
    [SerializeField] FishingRod[] fishingRods;

    public void SetStats(ShipStats shipStats)
    {
        for (int i = 0; i < fishingRods.Length; i++)
        {
            FishingRod rod = fishingRods[i];
            rod.gameObject.SetActive(i < shipStats.numRods);

            rod.stats.catchInterval = shipStats.catchInterval;
            rod.stats.catchAmount = shipStats.catchAmount;

            if (rod.isActiveAndEnabled)
            {
                rod.Initialize();
            }
        }

        engine.stats.engineWeight = shipStats.stability;
        
        engine.stats.adjustSteeringSpeed = shipStats.turningAccel;
        engine.stats.steeringSpeedMult = shipStats.turningForce;

        engine.stats.adjustThrottleSpeed = shipStats.throttleAccel;
        engine.stats.maxForce = shipStats.throttleForce;
    }
}