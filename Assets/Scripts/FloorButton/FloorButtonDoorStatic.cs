using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UEventHandler;
using DG.Tweening;

public class FloorButtonDoorStatic : MonoBehaviour
{
    //public static bool[] sensor = Enumerable.Repeat(true, 50).ToArray();

    public int numDoor;
    public float speed = 2;
    float dist = 0;

    public float openAngle = 90f;
    public float openDuration = 1f;
    public Ease openEase = Ease.OutBack;
    public Ease closeEase = Ease.InBack;

    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 FinalPoint = new Vector3(2.0f, 0.0f, 0.0f);
    Vector3 FinalPosition;
    Vector3 StartPosition;

    UEventHandler eventHandler = new UEventHandler();

    void Start()
    {
        FloorButtonSensorStatic.OnPressed.Subscribe(eventHandler, OpenDoor);
        FloorButtonSensorStatic.OnReleased.Subscribe(eventHandler, CloseDoor);
        //sensor[numDoor] = false;
        StartPosition = transform.position;
        FinalPosition = transform.position + FinalPoint;
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    void Update()
    {
        //if(sensor[numDoor]){
        //    dist = Vector3.Distance(FinalPosition, transform.position);
        //    if(dist > 0.1){
        //        transform.position = Vector3.MoveTowards(transform.position, FinalPosition, speed * Time.deltaTime );
        //    }
        //}
        //else{
        //    dist = Vector3.Distance(StartPosition, transform.position);
        //    if(dist > 0.1){
        //        transform.position = Vector3.MoveTowards(transform.position, StartPosition, speed * Time.deltaTime );
        //    }
        //}
    }

    [ContextMenu("Open Doors")]
    public void OpenTest()
    {
        OpenDoor(numDoor);
    }
    private void OpenDoor(int door)
    {
        if (door != numDoor) return;
        SoundController.current.PlayOneShotEvent("event:/Interactables/Door Open");
        leftDoor.DOLocalRotate(new Vector3(0, openAngle, 0), openDuration, RotateMode.LocalAxisAdd).SetEase(openEase);
        rightDoor.DOLocalRotate(new Vector3(0, openAngle, 0), openDuration, RotateMode.LocalAxisAdd).SetEase(openEase);
    }

    [ContextMenu("Close Doors")]
    public void CloseTest()
    {
        CloseDoor(numDoor);
    }
    private void CloseDoor(int door)
    {
        Debug.Log("Yeet");
        if (door != numDoor) return;
        SoundController.current.PlayOneShotEvent("event:/Interactables/Door Closed");
        leftDoor.DOLocalRotate(new Vector3(0, -openAngle, 0), openDuration, RotateMode.LocalAxisAdd).SetEase(closeEase);
        rightDoor.DOLocalRotate(new Vector3(0, -openAngle, 0), openDuration, RotateMode.LocalAxisAdd).SetEase(closeEase);

    }




}

