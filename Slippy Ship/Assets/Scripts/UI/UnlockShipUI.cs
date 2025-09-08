using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockShipUI : MonoBehaviour
{
    [SerializeField] GameObject blockingObj;
    [SerializeField] Button unlockBtn;
    [SerializeField] TMP_Text costText;
    [SerializeField] int cost;
    [SerializeField] Color costColorCanAfford = Color.white;
    [SerializeField] Color costColorCantAfford = Color.grey;
        
    void LateUpdate()
    {
        bool canAfford = PlayerCurrencyManager.Instance.CanAfford(cost);
        unlockBtn.interactable = canAfford;
        costText.text = cost.ToString();
        costText.color = canAfford ? costColorCanAfford : costColorCantAfford;
    }

    void OnEnable()
    {
        unlockBtn.onClick.AddListener(TryPurchaseShip);
    }

    void OnDisable()
    {
        unlockBtn.onClick.RemoveListener(TryPurchaseShip);
    }

    void TryPurchaseShip()
    {
        if(!PlayerCurrencyManager.Instance.TryBuyItem(cost)) return;
        blockingObj.SetActive(false);
    }
}
