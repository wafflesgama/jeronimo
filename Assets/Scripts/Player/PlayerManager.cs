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
        RefreshDevices();
    }
    void Start()
    {
    }

    private void RefreshDevices()
    {

        devices = InputSystem.devices.ToArray();
        uiManager.SetDevicesList(devices.Select(x => x.name + ",").ToArray());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
