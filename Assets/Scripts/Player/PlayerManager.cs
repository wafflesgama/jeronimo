using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;
    public SoundController soundController;

    public Player player1;
    public Player player2;

    MergeBehaviour mergeBehaviour;
    InputDevice[] devices;


    [Header("Revive")]
    public float reviveRate = 0.1f;
    public float reviveDropRate = 0.2f;


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
        {
            soundController.PlayOneShotEvent("event:/Core Gameplay/Player Kock");
            player1.KnockPlayer(knockPos);
        }
        else
        {
            soundController.PlayOneShotEvent("event:/Core Gameplay/Player Kock");
            player2.KnockPlayer(knockPos);
        }


        if (player1.isKnocked && player2.isKnocked)
        {
            //Reset Game
            soundController.StopAllEvents();
            LevelManager.current.GameOver();
        }

    }


    public void StartStopReviveOther(Player playerReviving)
    {
        if (playerReviving.isRevivingOther)
            StopReviveOther(playerReviving);
        else
            StartReviveOther(playerReviving);
    }

    public void StartReviveOther(Player playerReviving)
    {
        Player otherPlayer = playerReviving == player1 ? player2 : player1;

        if (!otherPlayer.isKnocked) return;

        playerReviving.isRevivingOther = true;

        soundController.PlayReviveSound();
        playerReviving.StartRevive();
        otherPlayer.StartBeingRevived();

    }

    public void StopReviveOther(Player playerReviving)
    {
        Player otherPlayer = playerReviving == player1 ? player2 : player1;

        playerReviving.isRevivingOther = false;

        playerReviving.StopRevive();
        otherPlayer.StopBeingRevived();
    }

    private void Update()
    {
        HandleRevive();
        HandleReviveDrop();
    }

    private void HandleRevive()
    {
        if (player1.isRevivingOther)
        {
            player2.reviveCounter += reviveRate;
            player2.UpdateBeingRevived();
            if (player2.reviveCounter >= 100)
            {
                soundController.StopReviveSound();
                StopReviveOther(player1);
                player2.RecoverPlayer();
            }
        }
        else if (player2.isRevivingOther)
        {
            player1.reviveCounter += reviveRate;
            player1.UpdateBeingRevived();
            if (player1.reviveCounter >= 100)
            {
                soundController.StopReviveSound();
                StopReviveOther(player2);
                player1.RecoverPlayer();
            }
        }
    }

    private void HandleReviveDrop()
    {
        if (!player2.isRevivingOther == player1.reviveCounter > 0)
        {
            player1.reviveCounter -= reviveDropRate;
            if (player1.reviveCounter < 0)
            {
                player1.reviveCounter = 0;
                soundController.StopReviveSound();
                player1.StopBeingRevived();
            }
        }
        else if (!player1.isRevivingOther == player2.reviveCounter > 0)
        {
            player2.reviveCounter -= reviveDropRate;
            if (player2.reviveCounter < 0)
            {
                player2.reviveCounter = 0;
                soundController.StopReviveSound();
                player2.StopBeingRevived();
            }
        }
    }
}
