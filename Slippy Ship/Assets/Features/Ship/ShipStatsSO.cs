using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ship/Ship Stats")]
public class ShipStatsSO : ScriptableObject
{
    [field: SerializeField] public ShipStats Data { get; private set; }
}