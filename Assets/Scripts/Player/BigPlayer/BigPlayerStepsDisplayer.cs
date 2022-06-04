using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BigPlayerStepsDisplayer : MonoBehaviour
{
    public BigPlayerMovController movController;

    public Image stepsCounter;

    public Image leftStepArea;

    public Image leftStepArrow;
    public Image rightStepArrow;

    public float lerpCountSpeed = 9;

    public float areaFillSpeed = .1f;
    public float areaFillAmount = .2f;

    public float arrowScaleAmount = .2f;
    public float arrowScaleSpeed = .2f;
    public Ease arrowScaleEase = Ease.OutQuad;

    public float shakeDuration = .5f;
    public float shakeStrength = 1f;

    UEventHandler eventHandler = new UEventHandler();


    void Start()
    {
        movController.OnStep.Subscribe(eventHandler, Step);
        movController.OnReset.Subscribe(eventHandler, Reset);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        float stepsProgress = (float)movController.stepCounter / (float)movController.maxStepCountBonus;

        stepsCounter.fillAmount = Mathf.Lerp(stepsCounter.fillAmount, stepsProgress, Time.deltaTime * lerpCountSpeed);
    }

    void Step(bool right)
    {
        if (!right)
        {
            leftStepArea.DOFillAmount(0.5f - areaFillAmount, areaFillSpeed).SetEase(arrowScaleEase);

            leftStepArrow.transform.DOScale(0, arrowScaleSpeed).SetEase(arrowScaleEase);
            rightStepArrow.transform.DOScale(1 + 1 * arrowScaleAmount, arrowScaleSpeed).SetEase(arrowScaleEase);

            //rightStepArrow.transform.DOPunchScale(Vector3.one* arrowScaleAmount, arrowScaleSpeed).SetLoops(1, LoopType.Yoyo);
        }
        else
        {
            leftStepArea.DOFillAmount(0.5f + areaFillAmount, areaFillSpeed).SetEase(arrowScaleEase);

            leftStepArrow.transform.DOScale(1 + 1 * arrowScaleAmount, arrowScaleSpeed).SetEase(arrowScaleEase);
            rightStepArrow.transform.DOScale(0, arrowScaleSpeed).SetEase(arrowScaleEase);

            //leftStepArea.DOFillAmount(0.5f + areaFillAmount, areaFillSpeed).SetLoops(1, LoopType.Yoyo);
            //leftStepArrow.transform.DOPunchScale(Vector3.one * arrowScaleAmount, arrowScaleSpeed).SetLoops(1, LoopType.Yoyo);
        }
    }

    void Reset()
    {
        transform.DOShakePosition(shakeDuration, shakeStrength);

        leftStepArea.DOFillAmount(0.5f, areaFillSpeed).SetEase(arrowScaleEase);
        leftStepArrow.transform.DOScale(1, arrowScaleSpeed).SetEase(arrowScaleEase);
        rightStepArrow.transform.DOScale(1, arrowScaleSpeed).SetEase(arrowScaleEase);

    }
}
