using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public LevelUiManager uiManager;

    bool paused = false,pauseFreeze;

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


    public async void PauseResume()
    {
        if (pauseFreeze) return;
        pauseFreeze = true;

        paused = !paused;


        if (paused)
        {
            uiManager.Pause();
            playerManager.RefreshDevices();
            //Time.timeScale = 0;
        }
        else
        {
            uiManager.Resume();
            //Time.timeScale = 1f;
        }

        await Task.Delay(150);
        pauseFreeze = false;

    }
}
