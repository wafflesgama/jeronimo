using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBehaviour : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject BigPlayer;

    private Transform Player1Position;
    private Transform Player2Position;
    private Transform BigPlayerPosition;

    private SmallPlayerMovController Player1Controller;
    private SmallPlayerMovController Player2Controller;

    public float mergeDistance = 1.3f;

    public bool isMerged = false;
    public bool unmerge = false;

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

    private void MergePlayers()
    {
        isMerged = true;
        Player1.SetActive(false);
        Player2.SetActive(false);
        BigPlayer.SetActive(true);
        
    }

    private void UnmergePlayers()
    {
        isMerged = false;
        Player1.SetActive(true);
        Player2.SetActive(true);
        BigPlayer.SetActive(false);
        selectUnmergePositions(BigPlayerPosition.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMerged)
        {
            if (unmerge)
            {
                
                unmerge = false;
                UnmergePlayers();
            }
        }
        else
        {
            float distance = Vector3.Distance(Player1Position.position, Player2Position.position);

            if(distance <= mergeDistance)
            {
                MergePlayers();
            }
        }
    }

    private bool goingOppositeDirections()
    {
        Vector3 pos1 = Vector3.Normalize(Player1Controller.getMoveDirection());
        Vector3 pos2 = Vector3.Normalize(Player2Controller.getMoveDirection());

        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(pos1,pos2));
        
        if(angle >= 0 && angle <= 90)
            return true;
        else
            return false;
    }

    private void selectUnmergePositions(Vector3 playerPos)
    {
        float radius = 4f;

        Debug.Log("Big Player");
        Debug.Log(playerPos);

        while (true)
        {
            Vector3 tempPos = playerPos + (Vector3)(radius * Random.insideUnitCircle);

            Player1Position.position = tempPos;
            if (Player1Controller.PlayerIsGrounded()) {
                break;
            }
        }

        Debug.Log("Player 1");
        Debug.Log(Player1Position.position);

        while (true)
        {
            Vector3 tempPos = playerPos + (Vector3)(radius * Random.insideUnitCircle);

            Player2Position.position = tempPos;
            if (Player2Controller.PlayerIsGrounded() && Vector3.Distance(tempPos, Player1Position.position) >= 1.5f)
            {
                break;
            }
        }

        Debug.Log("Player 2");
        Debug.Log(Player2Position.position);

        return;
    }
}
