using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ObjectiveList : MonoBehaviour
{
    string textValue;
    public Text textElement;
    public UnityEngine.UI.Image image ;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        textValue = "OBJECTIVE LIST";
        image.enabled = false ;
        textElement.text = "";
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.J))
        {   
            i++;
            AsyncFunc();
        }
    }

    async void AsyncFunc()
    {

            image.enabled = true ;
            textElement.text = i.ToString();

            await Task.Delay(2000);
            textElement.text = "";
            image.enabled = false ;
    }
}
