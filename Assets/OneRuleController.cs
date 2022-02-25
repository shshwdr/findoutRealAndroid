using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneRuleController : MonoBehaviour
{
    public Text ruleLabel;
    public void init(int level)
    {
        ruleLabel.text = GameManager.Instance.getLevelText();
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
