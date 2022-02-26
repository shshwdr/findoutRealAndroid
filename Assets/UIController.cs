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
    public Text healthLabel;


    public GameObject reportButton;

    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("levelStart", showCurrentRuleForLevel);
        EventPool.OptIn("updateMoney", updateMoney);
        EventPool.OptIn("updateHealth", updateHealth);
        currentRuleForLevelButton.onClick.AddListener(delegate
        {
            hideCurrentRuleForLevel();
            GameManager.Instance.startLevel();
        });
        updateMoney();
        reportButton.SetActive(false);
    }
    public void updateMoney()
    {
        moneyLabel.text = $"Money: {GameManager.Instance.money}";
    }
    public void updateHealth()
    {
        var health = ShopManager.Instance.health;
        moneyLabel.text = $"Health: {health}";
        if (health <= 0)
        {
            moneyLabel.color = Color.red;
        }
        else
        {
            moneyLabel.color = Color.white;

        }
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
        if (GameManager.Instance.currentRules.Contains(RealRule.canReport))
        {
            reportButton.SetActive(true);
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

    public void report()
    {

        if (!GameManager.Instance.isLevelStarted)
        {
            return;
        }
        var go = Instantiate(Resources.Load<GameObject>("popup"), popupTransform.position, Quaternion.identity);
        bool isCorrect = character.isLying == true;

        go.GetComponent<PopupController>().init(isCorrect, character.explain);
        GameManager.Instance.answer(isCorrect, CharacterType.android,true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
