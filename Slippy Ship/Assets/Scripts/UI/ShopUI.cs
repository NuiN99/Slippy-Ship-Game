using UnityEngine;
using UnityEngine.InputSystem;

public class ShopUI : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] GameObject popupRoot;

    bool _wasInShopArea;

    void Start()
    {
        SetShopOpen(false);
    }

    void OnEnable()
    {
        PlayerInputManager.Controls.MenuActions.OpenShop.performed += TryOpenShop;
        PlayerInputManager.Controls.MenuActions.CloseMenu.performed += TryCloseShop;
    }

    void OnDisable()
    {
        PlayerInputManager.Controls.MenuActions.OpenShop.performed -= TryOpenShop;
        PlayerInputManager.Controls.MenuActions.CloseMenu.performed -= TryCloseShop;
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