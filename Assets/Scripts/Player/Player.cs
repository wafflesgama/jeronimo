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


    public bool isKnocked { get; private set; }

    private void Awake()
    {

    }
    void Start()
    {

    }

    [ContextMenu("Knock")]
    public void KnockTest()
    {
        KnockPlayer(smallVfxManager.transform.position);
    }

    public void KnockPlayer(Vector3 knockPos)
    {
        if (isKnocked) return;

        isKnocked = true;

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

        smallMovController.gameObject.layer = 6;  //Player Layer
        smallMovController.FreezePlayer(unfreeze: true);
        smallAnimController.Recover();
        smallVfxManager.Recover();
    }



}
