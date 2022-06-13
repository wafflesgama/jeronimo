using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawn : MonoBehaviour
{
    public GameObject lootBase;
    public int count = 5;
    public float randomPos;
    public float dispersionForce = 5;
    public float dispersionRadius = 5;

    List<Rigidbody> loot;

    void Start()
    {
        loot = new List<Rigidbody>();


        for (int i = 0; i < count+1; i++)
        {
            var obj = Instantiate(lootBase, lootBase.transform.position + new Vector3(Random.Range(-randomPos, randomPos), 0, Random.Range(-randomPos, randomPos)), lootBase.transform.rotation);
            loot.Add(obj.GetComponent<Rigidbody>());
        }

        lootBase.SetActive(false);

        foreach (var item in loot)
        {
            item.AddExplosionForce(dispersionForce, transform.position, dispersionRadius);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
