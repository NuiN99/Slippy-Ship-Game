using System;
using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEngine;

public class FleetUpgradesManager : MonoBehaviour
{
    [SerializeField] EnumDictionary<ShipType, ShipUpgrader> fleet;
    
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl0;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl1;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl2;
    [SerializeField] EnumDictionary<ShipType, ShipStatsSO> shipStatsLvl3;
    
    Dictionary<ShipType, ShipUpgradeLevels> _shipUpgradeLevels = new();

    void Start()
    {
        foreach (ShipType shipType in Enum.GetValues(typeof(ShipType)))
        {
            ShipUpgrader shipUpgrader = fleet[shipType];
            if(shipUpgrader == null) continue;
            
            shipUpgrader.SetStats(shipStatsLvl3[shipType].Data);
        }
    }
}