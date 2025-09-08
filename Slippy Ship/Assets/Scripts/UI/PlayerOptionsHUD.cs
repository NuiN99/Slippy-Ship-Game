using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionsHUD : MonoBehaviour
{
    [SerializeField] Image quitPromptFill;
    [SerializeField] Image resetShipPromptFill;

    [SerializeField] float holdTime;

    Timer _quitHoldTimer;
    Timer _resetHoldTimer;

    void Awake()
    {
        _quitHoldTimer = new Timer(holdTime);
        _resetHoldTimer = new Timer(holdTime);
    }

    void Update()
    {
        if (!PlayerInputManager.Controls.MenuActions.CloseMenu.IsPressed()) _quitHoldTimer.Restart();
        if (!PlayerInputManager.Controls.Actions.ResetShip.IsPressed()) _resetHoldTimer.Restart();

        if (_resetHoldTimer.IsComplete)
        {
            _resetHoldTimer.Restart();
            FleetUpgradesManager.Instance.ResetShip();
        }

        if (_quitHoldTimer.IsComplete)
        {
            Application.Quit();
        }

        quitPromptFill.fillAmount = _quitHoldTimer.Progress;
        resetShipPromptFill.fillAmount = _resetHoldTimer.Progress;
    }
}
