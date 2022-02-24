using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { android, human}
public enum RealRule {sayImRobot,
    circleOnHead,
    squareOnBody,
    hasClothes,
    explainTheyHaveTattoo, 
    metalBodyParts,
    rotateWrongDirection,
    talkWhenTakeOff,
    androidLie,
    whiteEye,
    hasAccessory,

}
public class GameManager : Singleton<GameManager>
{
    public int money;
    [HideInInspector]
    public string[] allRules = new string[] { 
    "Android won't lie, if he said he is an andoid, then he is.",
    "Android has a blue circle on his head, use up, right and left arrow to check all three faces",
    "Android has a green square on his body.",
    "Too many testers and they don't have time to take off the clothes and accessories, click to take off them and take a full check.",
    "Some android have a metal body parts",
    "Some human have tattoos on their body. If they said they have tattoo on certain place, ignore that place if there is circle or square there.",
    "Android would lie",


    };
    public SetCharacter character;
    public int level;
    public bool isLevelStarted;
    RealRule[] levelToRule = new RealRule[] {
        //RealRule.sayImRobot,
        RealRule.circleOnHead,
        //RealRule.squareOnBody,
        RealRule.hasClothes,
    RealRule.hasAccessory,
    RealRule.explainTheyHaveTattoo,
     //   RealRule.metalBodyParts,
    RealRule.androidLie
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
            return;
        }
        currentRules.Add(levelToRule[level]);
        EventPool.Trigger("levelStart");
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

    public void answer(bool isCorrect,CharacterType type)//
    {
        if (isCorrect)
        {
            if(type == CharacterType.android)
            {
                money += 15;
            }
            else
            {
                money += 10;
            }
            EventPool.Trigger("updateMoney");
        }

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
