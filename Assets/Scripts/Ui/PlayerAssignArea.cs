using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssignArea : DragArea
{
    public PlayerManager manager;
    public enum PlayerAreaType { Player1, Player2, Devices };
    public PlayerAreaType areaType;

    public PlayerAssignArea devicesArea;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnDrag(DraggableItem item, bool switchInput = true)
    {

        if (areaType == PlayerAreaType.Player1 || areaType == PlayerAreaType.Player2)
        {

            if (switchInput)
            {
                int inputId = int.Parse(item.name.Split('-')[0]);
                if (areaType == PlayerAreaType.Player1)
                    manager.SwitchPlayerInput(1, inputId);
                else
                    manager.SwitchPlayerInput(2, inputId);
            }

            if (itemPlaced != null && itemPlaced != item)
            {
                devicesArea.PlaceInArea(itemPlaced);
            }


        }
            PlaceInArea(item);
            SetObject(item);
    }


}
