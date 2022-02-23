using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public SetCharacter character;
    public Transform popupTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void selectReal(bool isReal)
    {
        var go = Instantiate(Resources.Load<GameObject>("popup"), popupTransform.position,Quaternion.identity);
        if (character.isReal == isReal)
        {
            //correct
            go.GetComponentInChildren<Text>().text = "correct";
        }
        else
        {

            go.GetComponentInChildren<Text>().text = "wrong";
        }
        GameManager.Instance.nextCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
