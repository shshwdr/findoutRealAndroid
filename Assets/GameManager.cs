using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RealRule {sayImRobot,circleOnHead,squareOnBody,hasClothes,explainTheyHaveTattoo, metalBodyParts,rotateWrongDirection,talkWhenTakeOff,androidLie}
public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public string[] allRules = new string[] { 
    "Android won't lie, if he said he is an andoid, then he is.",
    "Android has a blue circle on his head, use up, right and left arrow to check all three faces",
    "Android has a green square on his body.",
    "Too many testers and they don't have time to take off the clothes and accessories, click to take off them and take a full check.",
    "Some human have tattoos on their body. If they said they have tattoo on certain place, ignore that place if there is circle or square there."


    };
    public SetCharacter character;
    public int level;
    public bool isLevelStarted;
    RealRule[] levelToRule = new RealRule[] {RealRule.sayImRobot,
        RealRule.circleOnHead,
        RealRule.squareOnBody,
        RealRule.hasClothes,
    RealRule.explainTheyHaveTattoo};
    public List<RealRule> currentRules = new List<RealRule>();
    // level 0 has circle on head
    // 1: has missing hand
    // 2: has clothes
    // 3: has square on body
    // 4: people would explain
    // 5: un real would lie
    // 6: 
    // Start is called before the first frame update

    int characterCount = 0;
    public int upgradeCount = 1;
    public void upgrade()
    {
        isLevelStarted = false;
        level++;
        if(level>= levelToRule.Length)
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
