using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public Text result;
    public Text explain;
    public void init(bool isCorrect,string ex)
    {
        if (isCorrect)
        {
            result.color = Color.green;
            result.text = "Good Job!";
        }
        else
        {
            result.color = Color.red;
            result.text = "Attention!";
        }
        explain.text = ex;
    }

    public void initWarn()
    {

        result.color = Color.red;
        result.text = "Think Twice!";
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
