using NuiN.NExtensions;
using NuiN.SpleenTween;
using UnityEngine;

public class ShopArea : MonoBehaviour
{
    public static bool IsInShopArea { get; private set; }
    [SerializeField] Transform sellPoint;
    [SerializeField] float sellTweenDuration;
    [SerializeField] float sellTweenYHeight;
    [SerializeField] AnimationCurve sellTweenYEase;
     
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fish fish))
        {
            fish.SetKinematic();
            int sellAmount = fish.SellAmount;
            
            SpleenTween.PosAxis(fish.transform, Axis.x, sellPoint.position.x, sellTweenDuration);
            SpleenTween.PosAxis(fish.transform, Axis.z, sellPoint.position.z, sellTweenDuration);
            SpleenTween.PosAxis(fish.transform, Axis.y, sellPoint.position.y, sellTweenDuration).SetEase(sellTweenYEase);
            this.DoAfter(sellTweenDuration, () =>
            {
                PlayerCurrencyManager.Instance.IncreaseCurrency(sellAmount);
                
                if(fish != null) Destroy(fish.gameObject);
            });
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerShip"))
        {
            IsInShopArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerShip"))
        {
            IsInShopArea = false;
        }
    }

    void OnDisable()
    {
        IsInShopArea = false;
    }
}