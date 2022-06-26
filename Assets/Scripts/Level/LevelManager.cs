using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UEventHandler;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    bool paused = false, pauseFreeze;

    UEventHandler eventHandler = new UEventHandler();

    public int coinCont;
    public UEvent OnGrabbedCoin = new UEvent(); 

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        coinCont = 0;
        PlayerManager.current.player1.inputManager.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
        PlayerManager.current.player1.inputManager.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
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
            LevelUiManager.current.Pause();
            PlayerManager.current.RefreshDevices();
            //Time.timeScale = 0;
        }
        else
        {
            LevelUiManager.current.Resume();
            //Time.timeScale = 1f;
        }

        await Task.Delay(150);
        pauseFreeze = false;

    }

    public async void GameOver()
    {
        LevelUiManager.current.FadeScreen(false);
        await Task.Delay(1500);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GrabbedCoin()
    {
        coinCont++;
        OnGrabbedCoin.TryInvoke();
    }
}
