using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using static SoundUtils;
public class MainMenuManager : MonoBehaviour
{
    //public Image fade;
    public float fadeSpeed = .5f;
    public AudioSource audioSource;
    public CanvasGroup mainGroup;
    public CanvasGroup creditsGroup;
    public CanvasGroup settingsGroup;
    public Animator fadeAnimator;

    public Sound clickSound;
    public Sound fadeInSound;
    public Sound fadeOutSound;

    //int menuState = -1;
    //bool inTransition;
    CanvasGroup currentGroup;
    void Awake()
    {
        //fade.color = Color.black;
        currentGroup = mainGroup;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //audioSource.PlaySound(fadeInSound);
    }

    public void GoToMain() => ChangeScreen(mainGroup);
    public void GoToSettings() => ChangeScreen(settingsGroup);
    public void GoToCredits() => ChangeScreen(creditsGroup);


   
    public async void ExitGame()
    {
        //audioSource.PlaySound(fadeOutSound);
        SoundController.current.PlayOneShotEvent("event:/SFX/Settings Pop-up in");
        Cursor.visible = false;
        //audioSource.PlaySound(clickSound);
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(800);
        Application.Quit();
    }

    public async void GoToGame()
    {
        SoundController.current.PlayOneShotEvent("event:/SFX/Click Button");
        Cursor.visible = false;
        //audioSource.PlaySound(clickSound);
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(800);
        SceneManager.LoadScene("Inside");
    }
    //private void FadeScreen(bool fadeIn) => fade.DOFade(fadeIn ? 1 : 0, fadeSpeed).SetEase(Ease.InOutQuad);


    private async void ChangeScreen(CanvasGroup outGroup)
    {
        //audioSource.PlaySound(clickSound);
        SoundController.current.PlayOneShotEvent("event:/SFX/Click Button");
        //audioSource.PlaySound(fadeOutSound);
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(800);
        currentGroup.gameObject.SetActive(false);
        outGroup.gameObject.SetActive(true);
        currentGroup = outGroup;
        //audioSource.PlaySound(fadeInSound);
        fadeAnimator.SetBool("FadeIn", true);
    }


}
