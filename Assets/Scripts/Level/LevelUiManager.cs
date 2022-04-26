using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LevelUiManager : MonoBehaviour
{
    UEventHandler eventHandler = new UEventHandler();

    public CursorManager cursorManager;

    [Header("Pause Screen")]
    public CanvasGroup pauseParentGroup;
    public CanvasGroup pauseMainGroup;
    public CanvasGroup pausePlayersGroup;
    public TextMeshProUGUI devicesText;

    void Start()
    {
        cursorManager.HideCursor();
    }
    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Resume()
    {
        pauseParentGroup.Hide();
        cursorManager.HideCursor();
    }

    public void Pause()
    {
        pauseParentGroup.Show();
        cursorManager.ShowCursor();
    }

    public void SetDevicesList(string[] devices)
    {
        string text = "";
        foreach (string device in devices)
            text += device + " | "; 

        devicesText.text = text;
    }


}
