using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { android, human}
public enum RealRule {
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


    none,
   // rotateWrongDirection,
   // talkWhenTakeOff,

}
public class GameManager : Singleton<GameManager>
{
    public int money;

    public string[] allRules { get { return allRulesTexts; } }

    [HideInInspector]
    string[] allRulesTexts = new string[] {
    "Android won't lie, if he said he is an andoid, then he is.",
    "Some android has a blue circle on his head, use up, right and left arrow to check all three faces",
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
    "None",
    };

    int[] adoidCount = new int[] { 1 };
    List<List<int>> androidPattern = new List<List<int>>();

    public RealRule latestRule;
    
    public SetCharacter character;
    public int level;
    public bool isLevelStarted;
    RealRule[] levelToRule = new RealRule[] {
        RealRule.sayImRobot,
        RealRule.circleOnHead,
    RealRule.androidLie,
    RealRule.canReport,
        RealRule.squareOnBody,
        //RealRule.hasClothes,
    //RealRule.hasAccessory,
    //RealRule.explainTheyHaveTattoo,
        RealRule.metalBodyParts,
    RealRule.tattooLie,
    RealRule.metalPartLie,

    };
    public List<RealRule> currentRules = new List<RealRule>();

    int characterCount = 0;
    public int upgradeCount = 1;
    public bool atMaxLevel()
    {
        return level >= levelToRule.Length;
    }
    public void upgrade()
    {
        isLevelStarted = false;
        level++;
        if(atMaxLevel())
        {
            latestRule = RealRule.none;
            return;
        }
        latestRule = levelToRule[level];
        currentRules.Add(latestRule);
        EventPool.Trigger("levelStart");
    }

    public string getLevelText()
    {
        if(level>= levelToRule.Length)
        {
            return "";
        }
        if((int)levelToRule[level] >= allRules.Length)
        {
            Debug.LogError("this is wrong");
        }
        return allRules[(int)levelToRule[level]];
    }
    void Start()
    {
        currentRules.Add(levelToRule[0]);
        EventPool.Trigger("levelStart");
    }

    public void startLevel()
    {
        isLevelStarted = true;
        resetCharacter();
    }

    public void answer(bool isCorrect,CharacterType type,bool isLying = false)//
    {
        if (isLying)
        {
            if (isCorrect)
            {

                money += 30;
            }
            else
            {
                money -= 30;
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
        }

        EventPool.Trigger("updateMoney");
        GameManager.Instance.nextCharacter();
    }

    void resetCharacter()
    {
        character.resetCharacter();
    }

    
    public void nextCharacter()
    {

        characterCount++;
        if (characterCount > upgradeCount)
        {
            upgrade();
            characterCount = 0;

        }
        resetCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetCharacter();
        }
    }
}
