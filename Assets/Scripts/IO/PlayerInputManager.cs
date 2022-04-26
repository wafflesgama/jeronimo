using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UEventHandler;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : MonoBehaviour
{

    public string[] devices;
    public bool secondPlayer;

    public Camera playerCamera { get; private set; }

    private void Awake()
    {


        var playerInput = GetComponent<PlayerInput>();

        if (playerInput.camera == null)
            playerCamera = Camera.main;
        else
            playerCamera = playerInput.camera;
        //if (secondPlayer)
        //    playerInput.SwitchCurrentControlScheme(playerInput.devices[playerInput.devices.Count-1]);
        devices = playerInput.devices.Select(x => x.name).ToArray();
        //playerInput.actions.
    }
    // public BufferedButton input_bufferedJump = new BufferedButton { bufferTime = 2 };
    public Button<Vector2> input_move = new Button<Vector2>();
    public Button<Vector2> input_look = new Button<Vector2>();
    public Button<float> input_fire = new Button<float>();
    public Button<float> input_fireAlt = new Button<float>();
    public Button<float> input_jump = new Button<float>();
    public Button<float> input_sprint = new Button<float>();
    public Button<float> input_pause = new Button<float>();
    public Button<float> input_crouch = new Button<float>();
    public Button<float> input_interact = new Button<float>();
    public Button<float> input_weapon1 = new Button<float>();
    public Button<float> input_weapon2 = new Button<float>();
    public Button<float> input_weapon3 = new Button<float>();

    #region Button Base stuff
    //public delegate void ClickAction();

    public class Button<TValue>  //Suported data types--> float | Vector2 | Vector3 | Vector4
    {
        public TValue value { get; set; }
        //public event ClickAction Onpressed;
        //public event ClickAction Onreleased;
        public UEvent Onpressed = new UEvent();
        public UEvent Onreleased = new UEvent();

        public void Pressed() => Onpressed.TryInvoke();
        //public void Pressed() => Onpressed?.Invoke();
        public void Released() => Onreleased.TryInvoke();
        //public void Released() => Onreleased?.Invoke();
    }

    public class BufferedButton : MonoBehaviour  //Suported data types--> bool
    {
        public bool isPressed { get; set; }
        public float bufferTime { get; set; }

        Coroutine bufferRoutine;

        public void ClearButtonBuffer()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = null;
        }
        public void SetButtonPress()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = StartCoroutine(WaitToClearInput());
        }

        IEnumerator WaitToClearInput()
        {
            yield return new WaitForSeconds(bufferTime);
            isPressed = false;
        }
    }
    #endregion

    private void OnMove(InputValue inputValue) => SetInputInfo(input_move, inputValue);
    private void OnLook(InputValue inputValue) => SetInputInfo(input_look, inputValue);
    private void OnFire(InputValue inputValue) => SetInputInfo(input_fire, inputValue);
    private void OnFireAlt(InputValue inputValue) => SetInputInfo(input_fireAlt, inputValue);
    private void OnJump(InputValue inputValue) => SetInputInfo(input_jump, inputValue);
    private void OnSprint(InputValue inputValue) => SetInputInfo(input_sprint, inputValue);
    private void OnPause(InputValue inputValue) => SetInputInfo(input_pause, inputValue);
    private void OnCrouch(InputValue inputValue) => SetInputInfo(input_crouch, inputValue);
    private void OnInteract(InputValue inputValue) => SetInputInfo(input_interact, inputValue);
    private void OnWeapon1(InputValue inputValue) => SetInputInfo(input_weapon1, inputValue);
    private void OnWeapon2(InputValue inputValue) => SetInputInfo(input_weapon2, inputValue);
    private void OnWeapon3(InputValue inputValue) => SetInputInfo(input_weapon3, inputValue);


    #region Info Setters
    void SetInputInfo(Button<float> button, InputValue inputValue)
    {
        var value = inputValue.Get<float>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value == 0)
            button.Released();
        else if (oldValue == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector2> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector2>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector3> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector3>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    void SetInputInfo(Button<Vector4> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector4>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    #endregion

}
