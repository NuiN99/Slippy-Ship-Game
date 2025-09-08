using System;
using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEngine;

public class FleetUpgradesManager : MonoBehaviour
{
    public static FleetUpgradesManager Instance { get; private set; }
    
    [SerializeField] EnumDictionary<ShipType, ShipUpgrader> fleet;
    
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl0;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl1;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl2;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl3;
    
    Dictionary<ShipType, ShipUpgradeLevels> _shipUpgradeLevels = new();
    Dictionary<ShipType, Dictionary<int, ShipStats>> _levelToShipStats = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
        {
            var dict = new Dictionary<int, ShipStats>
            {
                { 0, shipStatsLvl0[shipType].Data },
                { 1, shipStatsLvl1[shipType].Data },
                { 2, shipStatsLvl2[shipType].Data },
                { 3, shipStatsLvl3[shipType].Data }
            };
            _levelToShipStats.Add(shipType, dict);
        }
    }

    void Start()
    {
        foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
        {
            _shipUpgradeLevels.Add(shipType, new ShipUpgradeLevels());
            
            ShipUpgrader shipUpgrader = fleet[shipType];
            if(shipUpgrader == null) continue;
            
            shipUpgrader.SetStats(shipStatsLvl0[shipType].Data);
        }
    }

    public void UpgradeShip(ShipType shipType, ShipUpgradeLevels addition)
    {
        ShipUpgradeLevels newLevels = _shipUpgradeLevels[shipType].Add(addition);
        _shipUpgradeLevels[shipType] = newLevels;
        
        ShipUpgrader ship = fleet[shipType];
        if (ship != null)
        {
            Dictionary<int, ShipStats> dict = _levelToShipStats[shipType];
            ShipStats stats = new()
            {
                numRods = dict[newLevels.rodAmountLevel].numRods,
                catchAmount = dict[newLevels.catchAmountLevel].catchAmount,
                catchInterval = dict[newLevels.catchIntervalLevel].catchInterval,
                stability = dict[newLevels.stabilityLevel].stability,
                turningAccel = dict[newLevels.turningLevel].turningAccel,
                turningForce = dict[newLevels.turningLevel].turningForce,
                throttleForce = dict[newLevels.throttleLevel].throttleForce,
                throttleAccel = dict[newLevels.throttleLevel].throttleAccel,
            };
            
            ship.SetStats(stats);
        }
    }
}