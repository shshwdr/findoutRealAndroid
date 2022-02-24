using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public SetCharacter character;
    public Transform popupTransform;

    public GameObject currentRuleForLevel;
    public Button currentRuleForLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("levelStart", showCurrentRuleForLevel);
        currentRuleForLevelButton.onClick.AddListener(delegate
        {
            hideCurrentRuleForLevel();
            GameManager.Instance.startLevel();
        });
    }

    public void showCurrentRuleForLevel()
    {
        currentRuleForLevel.SetActive(true);
        currentRuleForLevel.GetComponentInChildren<OneRuleController>().init(GameManager.Instance.level);
    }

    void hideCurrentRuleForLevel()
    {

        currentRuleForLevel.SetActive(false);
    }

    public void selectReal(bool isReal)
    {
        if (!GameManager.Instance.isLevelStarted)
        {
            return;
        }
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
