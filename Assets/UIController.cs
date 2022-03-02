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


    public Text timeLabel;
    public Text testerLabel;

    GameManager gameManager;



    public GameObject reportButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        EventPool.OptIn("levelStart", showCurrentRuleForLevel);
        EventPool.OptIn("updateMoney", updateMoney);
        EventPool.OptIn("updateHealth", updateHealth);
        EventPool.OptIn("nextCharacter",updateCharacter);
        currentRuleForLevelButton.onClick.AddListener(delegate
        {
            hideCurrentRuleForLevel();
            GameManager.Instance.startLevel();
        });
        updateMoney();
        updateCharacter();
        updateHealth();
        reportButton.SetActive(false);
    }
    public void updateMoney()
    {
        moneyLabel.text = $"Money: {GameManager.Instance.money}";
    }

    public void updateCharacter()
    {
        testerLabel.text = $"Left: {GameManager.Instance.upgradeCount - gameManager.characterCount-1}";
    }
    public void updateHealth()
    {
        var health = ShopManager.Instance.health;
        healthLabel.text = $"Health: {health}";
        if (health <= 0)
        {
            healthLabel.color = Color.red;
        }
        else
        {
            healthLabel.color = Color.black;

        }
    }
    public void showCurrentRuleForLevel()
    {
        if (gameManager.currentRule == RealRule.none)
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
        bool isCorrect = character.isReal == isReal;

        if (!isCorrect && ShopManager.Instance.shouldAssist())
        {
            var go2 = Instantiate(Resources.Load<GameObject>("popup"), popupTransform.position, Quaternion.identity);
            go2.GetComponent<PopupController>().initWarn();
            return;
        }


        var go = Instantiate(Resources.Load<GameObject>("popup"), popupTransform.position, Quaternion.identity);

        go.GetComponent<PopupController>().init(isCorrect, character.explain);
        GameManager.Instance.answer(isCorrect, isReal ? CharacterType.human : CharacterType.android);
        character.characterLeave(isReal);
    }

    public void report()
    {

        if (!GameManager.Instance.isLevelStarted)
        {
            return;
        }
        var go = Instantiate(Resources.Load<GameObject>("popup"), popupTransform.position, Quaternion.identity);
        bool isCorrect = character.isLying == true;

        go.GetComponent<PopupController>().initLie(isCorrect, character.explain);
        GameManager.Instance.answer(isCorrect, CharacterType.android,true);
        character.characterLeave(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeLabel.text = "Time: "+((int)(gameManager.currentTime)).ToString();
    }
}
