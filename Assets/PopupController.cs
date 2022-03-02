using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupController : MonoBehaviour
{
    public Text result;
    public Text explain;

    public void initLie(bool isCorrect, string ex)
    {
        if (isCorrect)
        {
            result.color = Color.green;
            result.text = "Good Job!";
            explain.text = ex;
        }
        else
        {

            result.color = Color.red;
            result.text = "Attention!";
            explain.text = "it does not lie!";
        }
    }
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
        explain.text = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).DOJump(transform.GetChild(0).position, 100, 1, 2).SetUpdate(true);
        transform.GetChild(1).DOJump(transform.GetChild(1).position, 100, 1, 2).SetUpdate(true);
        //Destroy(gameObject, 2);
        StartCoroutine(dest());
    }

    IEnumerator dest()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
