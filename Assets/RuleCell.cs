using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleCell : MonoBehaviour
{
    public Text label;

    public void init(int level)
    {
        label.text = GameManager.Instance.getLevelText(level);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
