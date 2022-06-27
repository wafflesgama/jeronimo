using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Stealable : MonoBehaviour, Interactable
{
    public int objectId;

    public float hideDuration;
    public Ease hideEase;

    public Vector3 displayOffset;

    public Vector3 GetOffset() => displayOffset;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void Interact(Player player)
    {
        transform.tag = "Untagged";
        LevelManager.current.StealedObject(objectId);
        player.StealObject();
        transform.DOScale(Vector3.zero, hideDuration).SetEase(hideEase);
        await Task.Delay(1500);

    }
}
