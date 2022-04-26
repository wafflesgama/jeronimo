using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssignArea : DragArea
{
    public enum PlayerAreaType { Player1, Player2, Devices };
    public PlayerAreaType areaType;
    public Transform positionPlacer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnDrag(Transform objTransform)
    {

        if(areaType == PlayerAreaType.Player1 || areaType== PlayerAreaType.Player2)
        {
            objTransform.position= positionPlacer.position;
        }


    }
}
