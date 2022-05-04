using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject player;
    public GameObject hand;
    public Quaternion direction;

    public bool inside=false;
    public bool used = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hand != null){
            this.transform.position=hand.transform.position;
            
            direction = player.transform.Find("Idle (2)").gameObject.transform.rotation;
            this.transform.rotation =player.transform.Find("Idle (2)").gameObject.transform.rotation;
        }

        if(inside){

            this.transform.Rotate(0.0f,90.0f,0.0f);

        }else if(hand !=null){
            this.transform.Rotate(0.0f,0.0f,90.0f);

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           player = collision.gameObject;

           hand = player.transform.Find("Idle (2)/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand").gameObject;
        }

        if (collision.gameObject.tag == "KeyZone")
        {
            inside=true;
        }

    }

     void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "KeyZone")
        {
            inside=false;
            //this.transform.Rotate(0.0f,90.0f,0.0f);
        }

    }
}
