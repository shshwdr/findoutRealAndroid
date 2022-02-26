using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulleBook : MonoBehaviour
{
    RuleCell[] ruleCells;
    // Start is called before the first frame update
    void Start()
    {
        ruleCells = GetComponentsInChildren<RuleCell>(true);
        
    }
    public void showView()
    {
        if (ruleCells == null)
        {

            ruleCells = GetComponentsInChildren<RuleCell>(true);
        }
        gameObject.SetActive(true);
        Time.timeScale = 0;
        updateView();
    }
    public void hideView()
    {

        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void updateView()
    {
        int i = 0;
        for (; i <= GameManager.Instance.level; i++)
        {
            ruleCells[i].gameObject.SetActive(true);
            ruleCells[i].init(i);
        }
        for(;i< ruleCells.Length; i++)
        {

            ruleCells[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
