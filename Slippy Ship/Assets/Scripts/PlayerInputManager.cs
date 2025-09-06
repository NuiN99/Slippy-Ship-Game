using NuiN.CommandConsole;
using UnityEngine;

public static class PlayerInputManager
{
    public static PlayerControls Controls { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        Controls = new PlayerControls();
        Controls.Enable();
        
        CommandConsoleEvents.OnOpen += Controls.Disable;
        CommandConsoleEvents.OnClose += Controls.Enable;

        Application.quitting += OnQuit;
    }

    static void OnQuit()
    {
        Controls.Disable();
        Application.quitting -= OnQuit;
        
        CommandConsoleEvents.OnOpen -= Controls.Disable;
        CommandConsoleEvents.OnClose -= Controls.Enable;
    }
}
