using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
public enum CharacterType { android, human }
public enum RealRule
{
    sayImRobot,
    circleOnHead,
    squareOnBody,
    metalBodyParts,
    whiteEye,


    hasClothes,
    hasAccessory,

    explainTheyHaveTattoo,
    explainTheyHaveMissingPart,

    androidLie,
    canReport,
    tattooLie,
    metalPartLie,


    talkWhenTakeOff,
    none,

}
public class GameManager : Singleton<GameManager>
{

    public int maxTime;
    public float currentTime;

    public int money;
    int gameStage;
    int maxStage = 3;
    public int currentGameStage { get { return Mathf.Min(gameStage, maxStage); } }
    int[] gameStages = new int[] { 3, 7, 11 };
    public string[] allRules { get { return allRulesTexts; } }

    [HideInInspector]
    string[] allRulesTexts = new string[] {
    "Android won't lie, if he said he is an andoid, then he is.",
    "Some android has a blue circle on his face.",
    "Some android has a green square on his body.",
    "Some android have a metal body parts",
    "Some android have white eyes",

    "Too many testers and they don't have time to take off the clothes, click to take off them and take a full check.",
    "Too many testers and they don't have time to take off the accessories, click to take off them and take a full check.",

    "Some human have tattoos of circle on head or square on body. If he explained that he has the tattoo, then he's fine.",
    "Some human have lost part of his body and has a metal body part. If he explained that, then he's fine.",

    "Android would lie, if it says it is a human but it is not, it is an android",
    "You can report if an android is lying and get more money",
    "Android would lie about tattoo",
    "Android would lie about metal body parts",
    "Android would not complain when take off clothes",
    "None",
    };

    public int[] androidCount = new int[] { 1 };
    List<List<int>> androidPattern = new List<List<int>>();
    List<int> currentAndroidPattern;
    GameObject shopController;
    [HideInInspector]
    public RealRule latestRule = RealRule.none;

    public SetCharacter character;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public bool isLevelStarted;

    public RealRule currentRule { get { return levelToRule[level]; } }

    RealRule[] levelToRule = new RealRule[] {
        RealRule.sayImRobot,//0
        RealRule.circleOnHead,//1
        RealRule.squareOnBody,//2
        //war almost
        RealRule.hasClothes,//3
        RealRule.explainTheyHaveTattoo,//4
        RealRule.androidLie,//5
        //war start
        RealRule.canReport,//6
        RealRule.talkWhenTakeOff,//7
        RealRule.metalBodyParts,//8
        RealRule.tattooLie,//9
        //war bad
        RealRule.hasAccessory,//10
        RealRule.explainTheyHaveMissingPart,//11
        RealRule.metalPartLie,//12
        
        RealRule.none,//13

    };
    [HideInInspector]
    public List<RealRule> currentRules = new List<RealRule>();

    [HideInInspector]
    public int characterCount = 0;
    public int upgradeCount = 1;

    public int makeHumanToRobot;
    public int makeRobotToHuman;
    public bool atMaxLevel()
    {
        return level >= levelToRule.Length;
    }
    public void upgrade()
    {
        //ShopManager.Instance.health -= 30;
        isLevelStarted = false;
        Time.timeScale = 0;
        level++;
        if (atMaxLevel())
        {

            GameEndingManager.Instance.GameOver();
            //latestRule = RealRule.none;
            return;
        }
        latestRule = levelToRule[level];
        currentRules.Add(latestRule);

        if (latestRule == RealRule.canReport || latestRule == RealRule.hasAccessory || latestRule == RealRule.hasClothes)
        {
            latestRule = RealRule.none;
        }
        else
        {
            if (CheatManager.shouldLog)
            {
                Debug.Log($"latest rule change to {latestRule}");
            }
        }
        if (gameStage < gameStages.Length)
        {
            if(level >= gameStages[gameStage])
            {
                gameStage++;
                if (CheatManager.shouldLog)
                {
                    Debug.Log($"update stage to {gameStage}");
                }
                //EventPool.Trigger("updateStage");
            }
        }
        ShopManager.Instance.clearShop();
        shopController.SetActive(true);
        shopController.GetComponent<ShopController>().updateShop();
    }

    public void closeShop()
    {
        ShopManager.Instance.updateHealth();
        if (ShopManager.Instance.health > 0)
        {

            //show dialog
            pauseGameBetweenLevel();
        }

    }

    void pauseGameBetweenLevel()
    {
        Time.timeScale = 0;
        //PixelCrushers.DialogueSystem.DialogueManager.has
        if (PixelCrushers.DialogueSystem.DialogueManager.hasInstance)
        {

            PixelCrushers.DialogueSystem.DialogueManager.StartConversation($"M{level+1}");
        }
        else
        {
            finishDialogue();
        }
    }

    void finishDialogue()
    {
        EventPool.Trigger("levelStart");
    }
    public void clearLatestRule()
    {

        if (CheatManager.shouldLog)
        {
            Debug.Log($"clear  {latestRule}");
        }
        latestRule = RealRule.none;
    }
    public string getLevelText()
    {
        return getLevelText(level);
    }
    public string getLevelText(int l)
    {
        if (level >= levelToRule.Length)
        {
            return "";
        }
        if ((int)levelToRule[level] >= allRules.Length)
        {
            Debug.LogError("this is wrong");
        }
        return allRules[(int)levelToRule[level]];
    }
    void Start()
    {



        currentRules.Add(levelToRule[0]);
        // EventPool.Trigger("levelStart");
        EventPool.OptIn("dialogEnd",finishDialogue);
        shopController = GameObject.FindObjectOfType<ShopController>(true).gameObject;


        //generate androidPattern
        foreach (var i in androidCount)
        {
            List<int> onePattern = new List<int>();
            int j = 0;
            for (; j < i; j++)
            {

                onePattern.Add(1);// 1 = android
            }
            for (; j < upgradeCount; j++)
            {
                onePattern.Add(0);
            }
            androidPattern.Add(onePattern);
        }

        Time.timeScale = 0;
        generateCurrentLevelPattern();
        StartCoroutine(startGame());
        StartCoroutine(well());
    }

    IEnumerator well()
    {
        yield return null;
        //Resources.LoadAsync<Sprite>("");
    }

    IEnumerator startGame()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        pauseGameBetweenLevel();
    }

    void generateCurrentLevelPattern()
    {
        var potentialPattern = Utils.randomFromList(androidPattern);
        potentialPattern.Shuffle();
        currentAndroidPattern = potentialPattern;
    }

    public int nextType()
    {
        return currentAndroidPattern[characterCount];
    }

    public void startLevel()
    {
        isLevelStarted = true;

        currentTime = maxTime;
        Time.timeScale = 1;
        resetCharacter();
    }

    public void answer(bool isCorrect, CharacterType type, bool isLying = false)//
    {
        if (isLying)
        {
            if (isCorrect)
            {

                money += 30;
            }
            else
            {
                money -= 15;
                makeHumanToRobot++;

                DialogueManager.ShowAlert("Lost 15 coins for mistake");
            }
        }
        else
        {

            if (isCorrect)
            {
                if (type == CharacterType.android)
                {
                    money += 15;
                }
                else
                {
                    money += 10;
                }
            }
            else
            {
                if (type == CharacterType.android)
                {
                    makeHumanToRobot++;
                }
                else
                {
                    makeRobotToHuman++;
                }

                money -=5 ;
                DialogueManager.ShowAlert("Lost 5 coins for mistake");
            }
        }

        EventPool.Trigger("updateMoney");
    }

    void resetCharacter()
    {
        character.resetCharacter();
    }


    public void nextCharacter()
    {

        characterCount++;
        if (characterCount >= upgradeCount)
        {
            upgrade();
            characterCount = 0;
            generateCurrentLevelPattern();

        }
        else
        {
            resetCharacter();
            EventPool.Trigger("nextCharacter");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetCharacter();
        }
        currentTime -= Time.deltaTime;
        if (isLevelStarted && currentTime <= 0 )
        {
            upgrade();
        }
    }
}
