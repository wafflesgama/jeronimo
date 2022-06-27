using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

public class LevelUiManager : MonoBehaviour
{
    public static LevelUiManager current;
    UEventHandler eventHandler = new UEventHandler();

    public CursorManager cursorManager;

    [Header("Single Objective")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI objectiveCrossText;
    public Transform objectiveParent;
    public float objectiveAnimOffset = 2f;
    public float objectiveAnimDuration = 0.5f;
    public Ease objectiveShowEase = Ease.OutBack;
    public Ease objectiveHideEase = Ease.InBack;
    private float objectiveInitYPos;

    [Header("Fade Screen")]
    public Animator fader;

    [Header("Pause Screen")]
    public CanvasGroup pauseParentGroup;
    public CanvasGroup pauseMainGroup;
    public CanvasGroup pausePlayersGroup;

    [Header("Player Assign Screen")]
    public TextMeshProUGUI devicesText;
    public PlayerAssignArea player1Area;
    public PlayerAssignArea player2Area;
    public PlayerAssignArea devicesArea;
    public GameObject keyboardDeviceItem;
    public GameObject controllerdDeviceItem;


    List<Transform> devices;

    private void Awake()
    {
        current = this;
        devices = new List<Transform>();
        objectiveInitYPos = objectiveParent.position.y;
        objectiveParent.position -= new Vector3(0, objectiveAnimOffset);
    }

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


    public void FadeScreen(bool fadeIn)
    {
        fader.SetBool("FadeIn", fadeIn);
    }
    public void Resume()
    {
        Debug.Log("Resume");
        pauseParentGroup.Hide();
        cursorManager.HideCursor();
    }

    public void Pause()
    {
        Debug.Log("Pause");
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

    public async void ShowObjectiveDone(string objective)
    {
        objectiveCrossText.gameObject.SetActive(false);
        objectiveText.text = objective;
        objectiveParent.DOMoveY(objectiveInitYPos, objectiveAnimDuration).SetEase(objectiveShowEase);
        await Task.Delay(700);
        objectiveCrossText.gameObject.SetActive(true);
        await Task.Delay(2000);
        objectiveParent.DOMoveY(objectiveInitYPos - objectiveAnimOffset, objectiveAnimDuration).SetEase(objectiveHideEase);
    }

    public void ClearDevices()
    {
        foreach (var device in devices)
            Destroy(device.gameObject);

        devices.Clear();
    }

    public void AddAvailableDevice(int id, bool isController)
    {
        var device = CreateDevice(id, isController);
        //device.transform.SetParent(devicesArea.transform);
        devicesArea.OnDrag(device, false);
    }

    public void AddPlayerDevice(int id, bool isController, bool player1)
    {
        var device = CreateDevice(id, isController);

        if (player1)
        {
            player1Area.OnDrag(device);
            //player1Area.PlaceInArea(device);
        }
        else
        {
            player2Area.OnDrag(device);
            //player2Area.PlaceInArea(device);

        }
    }
    private DraggableItem CreateDevice(int id, bool isController)
    {
        GameObject obj;
        obj = isController ? GameObject.Instantiate(controllerdDeviceItem) : GameObject.Instantiate(keyboardDeviceItem);
        obj.name = $"{id}- Device";
        devices.Add(obj.transform);
        return obj.GetComponent<DraggableItem>();
    }


}
