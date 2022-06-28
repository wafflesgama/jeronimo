using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class CoinsCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public UEventHandler eventHandler = new UEventHandler();

    [Header("Counter Punch Anim")]
    public Vector3 punchScale = Vector3.up;
    public float punchInDuration = .5f;
    public float punchOutDuration = .5f;
    public Ease punchInEase = Ease.OutBack;
    public Ease punchOutEase = Ease.InBack;

    [Header("Slide in/out Anim")]
    public float hidePosOffset = 1.5f;
    public float durationSlide = .5f;
    public Ease slideInEase = Ease.OutBack;
    public Ease slideOutEase = Ease.InBack;

    [Header("Slide persist time")]
    public int delayToHide = 1000;

    int showing;
    float initYPos;
    void Start()
    {
        showing = 0;
        initYPos = transform.position.y;
        transform.position = transform.position + new Vector3(0, -hidePosOffset, 0);
        counter.text = "0";
        LevelManager.current.OnGrabbedCoin.Subscribe(eventHandler, GrabbedCoin);
    }

    // Update is called once per frame
    void Update()
    {
    }


    async void GrabbedCoin()
    {
        counter.text = LevelManager.coinCont.ToString();

        transform.DOMoveY(initYPos, durationSlide).SetEase(slideInEase);
        counter.transform.DOScale(punchScale, punchInDuration).SetEase(punchInEase).OnComplete(() => counter.transform.DOScale(Vector3.one, punchOutDuration).SetEase(punchOutEase));
        showing++;
        await Task.Delay(1500);
        showing--;

        if (showing > 0) return;

        transform.DOMoveY(initYPos - hidePosOffset, durationSlide).SetEase(slideOutEase);


    }
}
