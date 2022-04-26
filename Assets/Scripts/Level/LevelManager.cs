using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public LevelUiManager uiManager;

    bool paused = false;

    UEventHandler eventHandler = new UEventHandler();
    void Start()
    {
        playerManager.player1Input.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
        playerManager.player2Input.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PauseResume()
    {
        paused = !paused;

        if (paused)
        {
            uiManager.Pause();
            //Time.timeScale = 0;
        }
        else
        {
            uiManager.Resume();
            //Time.timeScale = 1f;
        }



    }
}
