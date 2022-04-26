using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelUiManager : MonoBehaviour
{
    UEventHandler eventHandler= new UEventHandler();

    public CursorManager cursorManager;

    [Header("Pause Screen")]
    public CanvasGroup pauseGroup;

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
        pauseGroup.Hide();
        cursorManager.HideCursor(); 
    }

    public void Pause()
    {
        pauseGroup.Show();
        cursorManager.ShowCursor();
    }


}
