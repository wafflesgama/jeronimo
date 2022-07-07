using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UEventHandler;

[Serializable]
public struct Objective
{
    public int id;
    public string description;
    public bool collected;
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    public static Objective[] objectivesScore;
    public static int coinCont;

    private static DateTime? startTime;
    private static DateTime? endTime;

    public Objective[] objectives;

    bool paused = false, pauseFreeze;

    UEventHandler eventHandler = new UEventHandler();

    public UEvent OnGrabbedCoin = new UEvent();

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        coinCont = 0;
        startTime = DateTime.Now;
        objectivesScore = objectives;

        PlayerManager.current.player1.inputManager.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
        PlayerManager.current.player2.inputManager.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
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

    [ContextMenu("Game Win")]
    public async void GameWin()
    {
        endTime = DateTime.Now;
        objectivesScore = objectives;
        SoundController.current.StopAllEvents();
        SoundController.current.PlayOneShotEvent("event:/Stingers/Victory");
        LevelUiManager.current.FadeScreen(false);
        await Task.Delay(1500);
        SceneManager.LoadScene("Score");
    }

    [ContextMenu("Game Over")]
    public async void GameOver()
    {

        endTime = DateTime.Now;
        LevelUiManager.current.FadeScreen(false);
        await Task.Delay(1500);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void StealedObject(int id)
    {
        var objectiveCompleted = objectives.Where(x => x.id == id).FirstOrDefault();

        var objectiveCompletedIndex = Array.FindIndex(objectives, val => val.id == id);
        objectives[objectiveCompletedIndex].collected = true;

        LevelUiManager.current.ShowObjectiveDone(objectiveCompleted.description);
    }
    public void GrabbedCoin()
    {
        SoundController.current.PlayOneShotEvent("event:/Interactables/Coin");
        coinCont++;
        OnGrabbedCoin.TryInvoke();
    }

    public static TimeSpan GetGameDuration()
    {
        if (!startTime.HasValue) return TimeSpan.MinValue;

        if (!endTime.HasValue || endTime.Value < startTime.Value) return (DateTime.Now - startTime.Value);

        return endTime.Value - startTime.Value;
    }
}
