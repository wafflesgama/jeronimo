using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigPlayerDirectionDisplayer : MonoBehaviour
{
    public PlayerInputManager player1Input;
    public PlayerInputManager player2Input;

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

    public float dotProduct;


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
        var move1 = player1Input.input_move.value;
        var moveRet1 = new Vector3(move1.x, 0, move1.y);
        var transformedMov1 = player1Input.playerCamera.transform.TransformDirection(moveRet1);
        transformedMov1.y = 0;

        var move2 = player2Input.input_move.value;
        var moveRet2 = new Vector3(move2.x, 0, move2.y);
        var transformedMov2 = player2Input.playerCamera.transform.TransformDirection(moveRet2);
        transformedMov2.y = 0;

        player1Direction.forward = Vector3.Slerp(player1Direction.forward, transformedMov1, Time.deltaTime * lerpSpeed);
        player2Direction.forward = Vector3.Slerp(player2Direction.forward, transformedMov2, Time.deltaTime * lerpSpeed);

        var dot = Vector3.Dot(player1Direction.forward, player2Direction.forward);
        dotProduct = dot;
        if (dot > minMergeIconThres && !bigArrowShowing)
        {
            player2Arrow.enabled = false;
            player1Arrow.color = Color.white;
            player1Arrow.DOColor(Color.white, arrowGrowSpeed).SetEase(arrowGrowEase);
            bigArrowShowing = true;
            player1Arrow.transform.DOScale(initArrowScale * arrowGrowFactor, arrowGrowSpeed).SetEase(arrowGrowEase);
        }
        else if (dot <= minMergeIconThres && bigArrowShowing)
        {
            player2Arrow.enabled = true;
            player1Arrow.DOColor(initArrowColor, arrowGrowSpeed).SetEase(arrowShrinkEase);
            player1Arrow.transform.DOScale(initArrowScale, arrowGrowSpeed).SetEase(arrowShrinkEase);
            bigArrowShowing = false;
        }

    }

    public float getDotProduct()
    {
        return dotProduct;
    }
}
