using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SmallPlayerUiManager : MonoBehaviour
{
    public Image reviveMeter;
    public float showMeterDuration = .5f;
    public float hideMeterDuration = .5f;


    bool showReviveMeter;

    void Start()
    {
        HideReviveMeter(false);
    }

    void Update()
    {

    }

    public void HideReviveMeter(bool animate = true)
    {
        if (animate)
            reviveMeter.transform.DOScale(Vector3.zero, hideMeterDuration);
        else
            reviveMeter.transform.localScale = Vector3.zero;
    }

    public void ShowReviveMeter()
    {
        reviveMeter.transform.DOScale(Vector3.one, showMeterDuration);
    }

    public void UpdateReviveMeter(float value)
    {
        //if (!showReviveMeter) return;

        reviveMeter.fillAmount = value;
    }
}
