using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] GameObject popupRoot;

    [SerializeField] Button skiffButton;
    [SerializeField] Button sailboatButton;

    bool _wasInShopArea;
    
    void Start()
    {
        SetShopOpen(false);
    }

    void OnEnable()
    {
        PlayerInputManager.Controls.MenuActions.OpenShop.performed += TryOpenShop;
        PlayerInputManager.Controls.MenuActions.CloseMenu.performed += TryCloseShop;
        
        skiffButton.onClick.AddListener(() => FleetUpgradesManager.Instance.SwitchToShip(ShipType.Skiff));
        sailboatButton.onClick.AddListener(() => FleetUpgradesManager.Instance.SwitchToShip(ShipType.Sailboat));
    }

    void OnDisable()
    {
        PlayerInputManager.Controls.MenuActions.OpenShop.performed -= TryOpenShop;
        PlayerInputManager.Controls.MenuActions.CloseMenu.performed -= TryCloseShop;
        
        skiffButton.onClick.RemoveAllListeners();
        sailboatButton.onClick.RemoveAllListeners();
    }

    void TryOpenShop(InputAction.CallbackContext ctx)
    {
        if (!ShopArea.IsInShopArea) return;
        SetShopOpen(!root.activeSelf);
    }

    void TryCloseShop(InputAction.CallbackContext ctx)
    {
        if (!root.activeSelf) return;
        SetShopOpen(false);
    }

    void LateUpdate()
    {
        if (_wasInShopArea && !ShopArea.IsInShopArea)
        {
            SetShopOpen(false);
        }
        _wasInShopArea = ShopArea.IsInShopArea;
        
        popupRoot.SetActive(ShopArea.IsInShopArea && !root.activeSelf);
    }

    void SetShopOpen(bool isOpen)
    {
        root.SetActive(isOpen);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        if (isOpen) PlayerInputManager.Controls.Actions.Disable();
        else PlayerInputManager.Controls.Actions.Enable();
    }
}