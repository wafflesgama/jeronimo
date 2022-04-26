using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SmallPlayerVFXManager : MonoBehaviour
{
    public SmallPlayerMovController mov;

    public VisualEffect dustVfx;
    public int dustRate;
    public AnimationCurve dustCurve;
    int dustRateVar, dustJumpVar, dustLandVar;
    bool jumpFlag=true;

    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        dustRateVar = Shader.PropertyToID("Rate");
        dustJumpVar = Shader.PropertyToID("OnJump");
        dustLandVar = Shader.PropertyToID("OnLand");
    }
    void Start()
    {
        mov.OnJump.Subscribe(eventHandler, Jump);
        mov.OnLand.Subscribe(eventHandler, Land);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    private void Jump()
    {
        //if (!jumpFlag) return;

        dustVfx.SendEvent(dustJumpVar);
        //jumpFlag= false;
    }
    private void Land()
    {
        //jumpFlag= true;
        dustVfx.SendEvent(dustLandVar);
    }
    private void LateUpdate()
    {
        var speedFraction = mov.horizontalVelMag / mov.goalVelocity;
        float dustAmount =dustCurve.Evaluate(speedFraction) * dustRate;
        dustAmount = mov.isGrounded ? dustAmount : 0;
        dustVfx.SetFloat(dustRateVar, dustAmount);
    }
}
    