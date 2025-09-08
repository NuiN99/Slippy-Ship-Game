using TMPro;
using UnityEngine;

public class PlayerCurrencyUI : MonoBehaviour
{
    [SerializeField] TMP_Text currencyText;

    void LateUpdate()
    {
        currencyText.SetText(PlayerCurrencyManager.Instance.Currency.ToString());
    }
}
