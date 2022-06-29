using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;


public class Coin : MonoBehaviour
{
    public Renderer mesh;
    public VisualEffect effect;
    public string grabEventName = "OnGrab";

    public Rigidbody rb;

    public float rotationSpeed = 1;
    public float floatSpeed = 1;
    public float floatHeight = 1;

    bool once;




    private void Start()
    {
        mesh.transform.eulerAngles += new Vector3(0, Random.Range(-90, 90), 0);
    }
    void Update()
    {
        mesh.transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        mesh.transform.position += new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * Time.deltaTime * floatHeight, 0);
    }


    private async void OnTriggerEnter(Collider other)
    {
        if (once) return;

        if (other.attachedRigidbody == null) return;

        if (other.attachedRigidbody.gameObject.tag != "Player") return;

        LevelManager.current.GrabbedCoin();

        mesh.enabled = false;
        effect.SendEvent(grabEventName);
        effect.Stop(); //Stop default emition
        rb.isKinematic = true;
        once = true;


        await Task.Delay(2000);

        

        if (gameObject != null)
            Destroy(gameObject);
    }
}
