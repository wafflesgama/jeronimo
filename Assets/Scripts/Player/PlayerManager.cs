using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerInputManager player1Input;
    public PlayerInputManager player2Input;

    //public UnityEngine.InputSystem.PlayerInputManager inputManager;

    public LevelUiManager uiManager;

    InputDevice[] devices;

    public bool isSelectingPlayers;
    private void Awake()
    {
    }
    void Start()
    {
    }

    public void RefreshDevices()
    {
        uiManager.ClearDevices();

        var player1Device = player1Input.GetMainInput();


        var player2Device = player2Input.gameObject.activeSelf ? player2Input.GetMainInput() : null;

        if (player1Device != null)
            uiManager.AddPlayerDevice(player1Device.deviceId, player1Device.name != "Keyboard", true);

        if (player2Device != null)
            uiManager.AddPlayerDevice(player2Device.deviceId, player2Device.name != "Keyboard", false);


        devices = InputSystem.devices.ToArray();

        foreach (var device in devices)
        {
            if (device != player1Device && device != player2Device && device.name != "Mouse" && device.name != "Pen")
            {
                uiManager.AddAvailableDevice(device.deviceId, device.name != "Keyboard");
            }


        }
        uiManager.SetDevicesList(devices.Select(x => x.name + ",").ToArray());
    }

    public void SwitchPlayerInput(int playerIndex, int controllerId)
    {
        if (playerIndex == 1)
        {
            player1Input.ChangeInputDevice(controllerId);
        }
        else
        {
            player2Input.ChangeInputDevice(controllerId);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
