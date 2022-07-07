using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public SmallPlayerMovController smallMovController;
    public SmallPlayerAnimController smallAnimController;
    public SmallPlayerVFXManager smallVfxManager;
    public SmallPlayerUiManager smallUiManager;

    public Collider reviveZone;
    public float reviveCounter;

    public bool isRevivingOther;
    public bool isKnocked { get; private set; }

    private void Awake()
    {
        reviveCounter = 0;
        reviveZone.gameObject.SetActive(false);
    }
    void Start()
    {

    }

    [ContextMenu("Knock")]
    public void KnockTest()
    {
        KnockPlayer(smallVfxManager.transform.position);
    }


    public void GrabObject()
    {
        smallAnimController.HoldObj();
    }

    public void KnockPlayer(Vector3 knockPos)
    {
        if (isKnocked) return;

        reviveCounter = 0;
        isKnocked = true;
        reviveZone.gameObject.SetActive(true);

        smallMovController.gameObject.layer = 7;  //KnockedPlayer Layer
        smallMovController.FreezePlayer();
        smallAnimController.Knock();
        smallVfxManager.Knock(knockPos);

    }

    [ContextMenu("Recover")]
    public void RecoverPlayer()
    {
        if (!isKnocked) return;

        isKnocked = false;
        reviveZone.gameObject.SetActive(false);

        smallMovController.gameObject.layer = 6;  //Player Layer
        smallMovController.FreezePlayer(unfreeze: true);
        smallAnimController.Recover();
        smallVfxManager.Recover();
    }


    public void StartRevive()
    {
    }

    public void StopRevive()
    {
    }


    public void StartBeingRevived()
    {
        smallUiManager.ShowReviveMeter();
    }
    public void UpdateBeingRevived()
    {
        smallUiManager.UpdateReviveMeter(reviveCounter / 100.0f);
    }

    public void StopBeingRevived()
    {
        smallUiManager.HideReviveMeter();
    }

    public async void StealObject()
    {
        smallMovController.FreezePlayer();
        smallAnimController.Steal();
        await Task.Delay(800);
        smallMovController.FreezePlayer(true);
    }

}
