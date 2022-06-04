using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigPlayerDirectionDisplayer : MonoBehaviour
{
    public MergeBehaviour mergeBehaviour;

    public Transform player1Direction;
    public Transform player2Direction;

    public SpriteRenderer player1Arrow;
    public SpriteRenderer player2Arrow;

    public float lerpSpeed = 40;

    public float minMergeIconThres = 0.9f;
    public Ease arrowGrowEase = Ease.OutElastic;
    public float arrowGrowSpeed = .5f;
    public float arrowGrowFactor = 2f;
    public Ease arrowShrinkEase = Ease.OutElastic;


    bool bigArrowShowing;

    Vector3 initArrowScale;
    Color initArrowColor;

    void Start()
    {
        initArrowScale = player1Arrow.transform.localScale;
        initArrowColor = player1Arrow.color;
    }

    // Update is called once per frame
    void Update()
    {


        player1Direction.forward = Vector3.Slerp(player1Direction.forward, mergeBehaviour.player1Dir, Time.deltaTime * lerpSpeed);
        player2Direction.forward = Vector3.Slerp(player2Direction.forward, mergeBehaviour.player2Dir, Time.deltaTime * lerpSpeed);



        if (mergeBehaviour.player1Dir != Vector3.zero && mergeBehaviour.player2Dir != Vector3.zero && mergeBehaviour.dot > minMergeIconThres && !bigArrowShowing)
        {
            player2Arrow.enabled = false;
            player1Arrow.color = Color.white;
            player1Arrow.DOColor(Color.white, arrowGrowSpeed).SetEase(arrowGrowEase);
            bigArrowShowing = true;
            player1Arrow.transform.DOScale(initArrowScale * arrowGrowFactor, arrowGrowSpeed).SetEase(arrowGrowEase);
        }
        else if ((mergeBehaviour.player1Dir == Vector3.zero || mergeBehaviour.player2Dir == Vector3.zero || mergeBehaviour.dot <= minMergeIconThres) && bigArrowShowing)
        {
            player2Arrow.enabled = true;
            player1Arrow.DOColor(initArrowColor, arrowGrowSpeed).SetEase(arrowShrinkEase);
            player1Arrow.transform.DOScale(initArrowScale, arrowGrowSpeed).SetEase(arrowShrinkEase);
            bigArrowShowing = false;
        }

        player1Arrow.enabled = mergeBehaviour.player1Dir != Vector3.zero;

        if (!bigArrowShowing)
            player2Arrow.enabled = mergeBehaviour.player2Dir != Vector3.zero;
    }


}
