using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScoreMenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI stealText;
    public TextMeshProUGUI timeText;

    public Animator fadeAnimator;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetupScore();
    }

    private void SetupScore()
    {
        coinsText.text = $"Coins collected: {LevelManager.coinCont}";

        var totalObjs = LevelManager.objectivesScore.Length;
        var objstolen = LevelManager.objectivesScore.Where(x => x.collected).Count();
        stealText.text = $"Objects stolen: {objstolen}/{totalObjs}";

        var gameDuration = LevelManager.GetGameDuration();
        timeText.text = $"Duration: {gameDuration.Hours}:{gameDuration.Minutes}:{gameDuration.Seconds}";

    }

    // Update is called once per frame
    void Update()
    {
        //Complete code
    }

    public async void GoToMainMenu()
    {
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(800);
        SceneManager.LoadScene("MainMenu");
    }
}
