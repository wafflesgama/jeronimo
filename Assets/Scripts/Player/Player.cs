using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Player1;
    public static Player Player2;

    public SmallPlayerMovController smallMovController;
    public SmallPlayerAnimController smallAnimController;


    private void Awake()
    {
        if (Player1 == null)
            Player1 = this;
        else
            Player2 = this;


    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
