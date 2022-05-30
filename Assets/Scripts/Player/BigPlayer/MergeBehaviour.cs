using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBehaviour : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    private Transform Player1Position;
    private Transform Player2Position;

    public float mergeDistance = 0.5f;

    private bool isMerged;

    // Start is called before the first frame update
    void Start()
    {
        Player1Position = Player1.transform.GetChild(0).transform;
        Player2Position = Player2.transform.GetChild(0).transform;
    }

    private void MergePlayers()
    {

    }

    private void UnmergePlayers()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMerged)
        {

        }
        else
        {
            float distance = Vector3.Distance(Player1Position.position, Player2Position.position);

            if(distance < mergeDistance)
            {
                MergePlayers();
            }
        }
    }
}
