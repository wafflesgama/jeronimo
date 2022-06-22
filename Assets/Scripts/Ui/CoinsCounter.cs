using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCounter : MonoBehaviour
{
    public Text textElement;
    // Start is called before the first frame update
    void Start() 
    {
        textElement.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = LevelManager.CoinCont.ToString();
    }
}
