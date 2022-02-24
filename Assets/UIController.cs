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
    public Text moneyLabel;

    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("levelStart", showCurrentRuleForLevel);
        EventPool.OptIn("updateMoney", updateMoney);
        currentRuleForLevelButton.onClick.AddListener(delegate
        {
            hideCurrentRuleForLevel();
            GameManager.Instance.startLevel();
        });
        updateMoney();
    }
    public void updateMoney()
    {
        moneyLabel.text = $"Money: {GameManager.Instance.money}"; 
    }
    public void showCurrentRuleForLevel()
    {
        if (GameManager.Instance.atMaxLevel())
        {

            GameManager.Instance.startLevel();
        }
        else
        {
            currentRuleForLevel.SetActive(true);
            currentRuleForLevel.GetComponentInChildren<OneRuleController>().init(GameManager.Instance.level);
        }
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
        bool isCorrect = character.isReal == isReal;

        go.GetComponent<PopupController>().init(isCorrect, character.explain);
        GameManager.Instance.answer(isCorrect, isReal ? CharacterType.human : CharacterType.android);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
