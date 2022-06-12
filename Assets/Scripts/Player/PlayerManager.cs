using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    public Player player1;
    public Player player2;

    MergeBehaviour mergeBehaviour;
    InputDevice[] devices;

    private void Awake()
    {
        current = this;

        mergeBehaviour = GetComponent<MergeBehaviour>();
    }
    void Start()
    {
    }

    public void RefreshDevices()
    {
        LevelUiManager.current.ClearDevices();

        var player1Device = player1.inputManager.GetMainInput();


        var player2Device = player2.inputManager.gameObject.activeSelf ? player2.inputManager.GetMainInput() : null;

        if (player1Device != null)
            LevelUiManager.current.AddPlayerDevice(player1Device.deviceId, player1Device.name != "Keyboard", true);

        if (player2Device != null)
            LevelUiManager.current.AddPlayerDevice(player2Device.deviceId, player2Device.name != "Keyboard", false);


        devices = InputSystem.devices.ToArray();

        foreach (var device in devices)
        {
            if (device != player1Device && device != player2Device && device.name != "Mouse" && device.name != "Pen")
            {
                LevelUiManager.current.AddAvailableDevice(device.deviceId, device.name != "Keyboard");
            }


        }
        LevelUiManager.current.SetDevicesList(devices.Select(x => x.name + ",").ToArray());
    }

    public void SwitchPlayerInput(int playerIndex, int controllerId)
    {
        if (playerIndex == 1)
        {
            player1.inputManager.ChangeInputDevice(controllerId);
        }
        else
        {
            player2.inputManager.ChangeInputDevice(controllerId);
        }
    }

    public void KnockPlayers(Transform playerKnocked, Vector3 knockPos)
    {
        if (mergeBehaviour.isMerged)
        {
            mergeBehaviour.UnmergePlayers();
            return;
        }

        if (playerKnocked.parent.name == player1.name)
            player1.KnockPlayer(knockPos);
        else
            player2.KnockPlayer(knockPos);


        if (player1.isKnocked && player2.isKnocked)
        {
            //Reset Game
            LevelManager.current.GameOver();
        }



    }
}
