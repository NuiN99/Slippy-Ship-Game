using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUpgradeButtonUI : MonoBehaviour
{
    const int MAX_LEVEL = 3;

    [SerializeField] ShipType shipType;
    
    [SerializeField] Button purchaseButton;
    [SerializeField] ShipUpgradeLevels upgradeLevel;
    [SerializeField] Image[] upgradeFills = new Image[MAX_LEVEL];
    [SerializeField] int[] costs = new int[MAX_LEVEL];

    [SerializeField] Color emptyFillColor = Color.grey;
    [SerializeField] Color upgradedFillColor = Color.cyan;

    [SerializeField] Image coinImage;
    [SerializeField] TMP_Text costText;
    [SerializeField] Color costColorCanAfford = Color.white;
    [SerializeField] Color costColorCantAfford = Color.grey;

    int _level;
    
    void OnEnable()
    {
        RefreshFills();
        purchaseButton.onClick.AddListener(OnClickUpgrade);
    }

    void OnDisable()
    {
        purchaseButton.onClick.RemoveListener(OnClickUpgrade);
    }

    void LateUpdate()
    {
        if (_level >= MAX_LEVEL)
        {
            purchaseButton.interactable = false;
            costText.gameObject.SetActive(false);
            coinImage.gameObject.SetActive(false);
            return;
        }
        
        int curCost = costs[_level];

        bool canAfford = PlayerCurrencyManager.Instance.CanAfford(curCost);

        costText.color = canAfford ? costColorCanAfford : costColorCantAfford;
        costText.SetText(curCost.ToString());
            
        purchaseButton.interactable = _level < MAX_LEVEL && canAfford;
    }

    void OnClickUpgrade()
    {
        int curCost = costs[_level];
        if (!PlayerCurrencyManager.Instance.TryBuyItem(curCost)) return;
        FleetUpgradesManager.Instance.UpgradeShip(shipType, upgradeLevel);
        
        _level++;

        RefreshFills();
    }

    void RefreshFills()
    {
        for (int i = 0; i < MAX_LEVEL; i++)
        {
            upgradeFills[i].color = i < _level ? upgradedFillColor : emptyFillColor;
        }
    }
}