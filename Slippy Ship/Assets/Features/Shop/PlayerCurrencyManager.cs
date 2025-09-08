using UnityEngine;

public class PlayerCurrencyManager : MonoBehaviour
{
    public static PlayerCurrencyManager Instance { get; private set; }
    public int Currency { get; private set; }

    [SerializeField] int startingCurrency = 100;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        Currency = startingCurrency;
    }

    public void IncreaseCurrency(int amount)
    {
        Currency += amount;
    }

    public bool TryBuyItem(int cost)
    {
        if(!CanAfford(cost)) return false;
        Currency -= cost;
        return true;
    }

    public bool CanAfford(int cost) => Currency >= cost;
}