using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBehaviour : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject BigPlayer;

    public BigPlayerDirectionDisplayer displayer;

    private Transform Player1Position;
    private Transform Player2Position;
    private Transform BigPlayerPosition;


    private SmallPlayerMovController Player1Controller;
    private SmallPlayerMovController Player2Controller;

    public PlayerManager PlayerManager;

    public float mergeDistance = 1.3f;
    public float mergeDot = -0.8f;

    public bool isMerged = false;

    public float unmergeTimer = 1f;

    public float dot { get; private set; }
    public Vector3 player1Dir;
    public Vector3 player2Dir;


    // Start is called before the first frame update
    void Start()
    {
        Player1Position = Player1.transform.GetChild(0).transform;
        Player2Position = Player2.transform.GetChild(0).transform;
        BigPlayerPosition = BigPlayer.transform.GetChild(0).transform;

        Player1Controller = Player1.transform.GetChild(0).GetComponent<SmallPlayerMovController>();
        Player2Controller = Player2.transform.GetChild(0).GetComponent<SmallPlayerMovController>();

        BigPlayer.SetActive(false);
    }



    public bool getIsMerged()
    {
        return isMerged;
    }

    private void MergePlayers()
    {
        unmergeTimer = 1f;
        isMerged = true;
        Player1.SetActive(false);
        Player2.SetActive(false);
        BigPlayer.SetActive(true);
        BigPlayerPosition.position = Player1Position.position;

    }

    public void UnmergePlayers()
    {
        unmergeTimer = 1f;
        isMerged = false;
        Player1.SetActive(true);
        Player2.SetActive(true);
        BigPlayer.SetActive(false);
        SelectUnmergePositions(BigPlayerPosition.transform.position);
        PlayerManager.RefreshDevices();
    }

    void LateUpdate()
    {
        var move1 = PlayerManager.player1.inputManager.input_move.value;
        var moveRet1 = new Vector3(move1.x, 0, move1.y);
        player1Dir = Camera.main.transform.TransformDirection(moveRet1);
        player1Dir.y = 0;
        player1Dir.Normalize();

        var move2 = PlayerManager.player2.inputManager.input_move.value;
        var moveRet2 = new Vector3(move2.x, 0, move2.y);
        player2Dir = Camera.main.transform.TransformDirection(moveRet2);
        player2Dir.y = 0;
        player2Dir.Normalize();

        dot = Vector3.Dot(player1Dir, player2Dir);


        //Debug.Log("Dot " + dot);

        if (isMerged)
        {

            if (dot < mergeDot && unmergeTimer < 0)
            {
                UnmergePlayers();
            }
            else if (dot < mergeDot)
            {
                unmergeTimer -= Time.deltaTime;
            }
            else
            {
                unmergeTimer = 1f;
            }
        }
        else
        {
            float distance = Vector3.Distance(Player1Position.position, Player2Position.position);

            if (distance <= mergeDistance && dot < mergeDot)
            {
                MergePlayers();
            }
        }
    }

    private void SelectUnmergePositions(Vector3 playerPos)
    {
        float radius = 4f;


        while (true)
        {
            Vector3 tempPos = playerPos + (Vector3)(radius * Random.insideUnitCircle);

            Player1Position.position = tempPos;
            if (Player1Controller.isGrounded)
            {
                break;
            }
        }


        while (true)
        {
            Vector3 tempPos = playerPos + (Vector3)(radius * Random.insideUnitCircle);

            Player2Position.position = tempPos;
            if (Player2Controller.isGrounded && Vector3.Distance(tempPos, Player1Position.position) >= 1.5f)
            {
                break;
            }
        }

        return;
    }
}
