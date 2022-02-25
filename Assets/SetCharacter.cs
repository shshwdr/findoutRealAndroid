using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetCharacter : MonoBehaviour
{
    SpriteAnimator body;
    SpriteAnimator[] unreals;
    SpriteAnimator hair;
    SpriteAnimator outfit;
    SpriteAnimator[] accessories;
    public bool isReal;
    public bool isLying;

    List<string> allTattooPosition = new List<string>();
    List<string> allMissingPartPosition = new List<string>();

    public Text words;
    public string explain;
    const string explainImRobot = "It said he's an android!";
    const string explainCircleOnFace = "It has a circle on {0}!";
    const string explainSquareOnBody = "It has a square on {0}!";
    const string explainMissBodyPart = "It has a metal {0}!";


    const string explainLie = "It lies! ";
    const string explainLieTatto = "It does not have tattoo on {0}!";
    const string explainLieMetalPart = "It does not have metal {0}!";


    const string explainNormalHuman = "He looks a perfect human!";
    const string explainExplainedTattoo = "He explained tattoo on {0}!";
    const string explainExplainedMissBodyPart = "He explained the metal {0}!";


    string characterPath = "newCharacter/";
    const string bodyPath = "newCharacter/Bodies";
    const string circlePath = "newCharacter/circle";
    const string squarePath = "newCharacter/square";
    const string metalPartPath = "newCharacter/metalPart";
    const string outfitPath = "newCharacter/outfit";
    const string hairPath = "newCharacter/hair";
    const string accessoriesPath = "newCharacter/Accessories";
    const string glovesPath = "newCharacter/gloves";
    const string unremoveableAccessoryPath = "newCharacter/unremoveableAccessory";

    string[] robotWords = new string[]
    {
        "I'm a robot.",
        "I'm an android.",
        "I'm not a human.",

    };

    string[] humanWords = new string[]
    {
        "I'm not a robot.",
        "I'm not an android.",
        "I'm a human.",

    };

    string[] generalWords = new string[]
    {
        "Good day.",
        "Please don't kill me.",
        "My name is Connor.",

    };

    string[] explainTattooWords = new string[] { 
    "I thought it was cool so I tattooed on my {0}",
    "I have a tattoo on my {0}",
    "On my {0}? It's just a tattoo!",

    };

    string[] explainMissingParts = new string[] {
    "I lost my {0} in a factory accident.",
    "I don't have {0} since I was born.",
    "I lost my {0} when fight with an evil android.",

    };


    bool shouldResetLatestRule = false;
   

    // Start is called before the first frame update
    void Start()
    {
        accessories = transform.Find("accessory").GetComponentsInChildren<SpriteAnimator>(true);
        body = transform.Find("body").GetComponent<SpriteAnimator>();
        outfit = transform.Find("outfit").GetComponent<SpriteAnimator>();
        unreals = transform.Find("unreal").GetComponentsInChildren<SpriteAnimator>(true);
        hair = transform.Find("hair").GetComponent<SpriteAnimator>();

        addNameFromPath(circlePath, true);
        addNameFromPath(squarePath, true);
        addNameFromPath(metalPartPath, false);



        //resetCharacter();
    }

    public void addNameFromPath(string path, bool isTattoo)
    {
        var test = Resources.LoadAll<Texture2D>(path);
        //print(test);
        if (test.Length == 0)
        {
            Debug.LogWarning($"item not exist in {path}");
            return;
        }
        var cid = test[Random.Range(0, test.Length)];
        var actualName = cid.name;
        actualName = path + "/" + actualName;
        if (isTattoo)
        {
            allTattooPosition.Add(actualName);
        }
        else
        {
            allMissingPartPosition.Add(actualName);
        }
    }
    public string resetItem(string path, SpriteAnimator animator)
    {
        var test = Resources.LoadAll<Texture2D>(path);
        //print(test);
        if(test.Length == 0)
        {
            Debug.LogWarning($"item not exist in {path}");
            return "";
        }
        var cid = test[Random.Range(0, test.Length)];
        animator.gameObject.SetActive(true);
        var actualName = cid.name;
        animator.PlayerSpriteSheets = Resources.LoadAll<Sprite>(path + "/" + actualName);
        if(animator.PlayerSpriteSheets.Length == 0)
        {
            Debug.Log($"{path} {actualName}'s length is 0");
        }

        int slashPos = actualName.LastIndexOf(',');
        if (slashPos != -1)
        {

            actualName = actualName.Substring(slashPos);
        }

        return actualName;
    }
    public void resetBase()
    {
        resetItem(bodyPath,body);
    }

    public void updateWordsOfExplain(bool isTattoo, string path)
    {
        string generalWord   = "";
        if (isTattoo)
        {
            generalWord = Utils.randomFromList(explainTattooWords);
        }
        else
        {
            generalWord = Utils.randomFromList(explainMissingParts);
        }
        //int slashPos = path.LastIndexOf('/');
        //path = path.Substring(slashPos);
        string w = string.Format(generalWord, path);
        updateWords(w);
    }

    string pickUnrealPath(bool onlyTattoo = false)
    {
        string path = null;
        if (GameManager.Instance.latestRule == RealRule.circleOnHead || GameManager.Instance.latestRule == RealRule.explainTheyHaveTattoo)
        {
            path = circlePath;
            shouldResetLatestRule = true;
        }
        else if (GameManager.Instance.latestRule == RealRule.squareOnBody)
        {
            path = squarePath;
            shouldResetLatestRule = true;
        }
        else if (GameManager.Instance.latestRule == RealRule.metalBodyParts || GameManager.Instance.latestRule == RealRule.explainTheyHaveMissingPart)
        {
            path = metalPartPath;
            shouldResetLatestRule = true;
        }
        else
        {
            List<string> possiblePath = new List<string>();
            if (GameManager.Instance.currentRules.Contains(RealRule.circleOnHead))
            {
                possiblePath.Add(circlePath);
            }
            if (GameManager.Instance.currentRules.Contains(RealRule.squareOnBody))
            {
                possiblePath.Add(squarePath);
            }
            if (!onlyTattoo && GameManager.Instance.currentRules.Contains(RealRule.metalBodyParts))
            {
                possiblePath.Add(metalPartPath);
            }
            if (possiblePath.Count == 0)
            {
                return null;
            }
            path = possiblePath[Random.Range(0, possiblePath.Count)];
        }

        if (CheatManager.shouldLog)
        {
            Debug.Log($"add unreal decoration {path}");
        }
        return path;
    }
    public void updateWords(string word)
    {
        words.text = word;
        if (CheatManager.shouldLog)
        {
            Debug.Log($"say words {word}");
        }
    }
    public void setWords()
    {
        if (!isReal)
        {
            if (GameManager.Instance.currentRules.Count == 1)
            {
                updateWords(Utils.randomFromList(robotWords));
                explain = explainImRobot;
                return;
            }
            else
            {
                if(GameManager.Instance.latestRule == RealRule.circleOnHead ||
                   GameManager.Instance.latestRule == RealRule.squareOnBody ||
                  GameManager.Instance.latestRule == RealRule.metalBodyParts)
                {
                    updateWords(Utils.randomFromList(generalWords));
                }
                else
                {
                    if (GameManager.Instance.currentRules.Contains(RealRule.androidLie))
                    {
                        if (GameManager.Instance.latestRule == RealRule.androidLie)
                        {
                            updateWords(Utils.randomFromList(humanWords));
                            isLying = true;
                            shouldResetLatestRule = true;
                        }
                        else
                        {
                            var rand = Random.Range(0f, 1f) > 0.85f;
                            if (rand)
                            {
                                updateWords(Utils.randomFromList(humanWords));
                                isLying = true;
                                return;
                            }
                            else
                            {
                                rand = Random.Range(0f, 1f) > 0.85f;
                                if (rand)
                                {
                                    updateWords(Utils.randomFromList(robotWords));
                                    explain = explainImRobot;
                                    return;
                                }
                                else
                                {
                                    updateWords(Utils.randomFromList(generalWords));
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        var rand = Random.Range(0f, 1f) > 0.7f;
                        if (rand)
                        {
                            updateWords(Utils.randomFromList(robotWords));
                            explain = explainImRobot;
                            return;
                        }
                        else
                        {

                            updateWords(Utils.randomFromList(generalWords));
                        }
                    }
                }
                
            }
        }
        else
        {
            var allWords = generalWords.Union(humanWords).ToArray();
            updateWords(Utils.randomFromList(allWords));
        }
    }
    public void resetUnreal()
    {
        if (isReal || GameManager.Instance.currentRules.Count == 1)
        {
            foreach(var unreal in unreals)
            {
                unreal.gameObject.SetActive(false);
            }
            if (GameManager.Instance.currentRules.Contains(RealRule.explainTheyHaveTattoo))
            {
                if (Random.Range(0, 2) > 0 || GameManager.Instance.latestRule == RealRule.explainTheyHaveTattoo || GameManager.Instance.latestRule == RealRule.explainTheyHaveMissingPart)
                {
                    var pickedPath = pickUnrealPath();
                    //if (unrealTypes.Contains(pickedPath))
                    //{
                    //    break;
                    //}
                    //unrealTypes.Add(pickedPath);
                    unreals[0].gameObject.SetActive(true);
                    string path = pickUnrealPath(GameManager.Instance.currentRules.Contains(RealRule.explainTheyHaveMissingPart));
                    var itemName = resetItem(path, unreals[0]);


                    switch (pickedPath)
                    {
                        case circlePath:
                        case squarePath:
                            explain = string.Format( explainExplainedTattoo,itemName);
                            updateWordsOfExplain(true, itemName);
                            break;
                        case metalPartPath:
                            explain = string.Format(explainExplainedMissBodyPart, itemName);
                            updateWordsOfExplain(false, itemName);
                            break;

                    }

                    if (CheatManager.shouldLog)
                    {
                        Debug.Log("real people with a tattoo");
                    }
                }
            }
        }
        else//not real
        {
            List<string> unrealTypes = new List<string>();
            List<string> unrealFinalNames = new List<string>();
            var pickedPath = pickUnrealPath();
            if (pickedPath == null)
            {
                return;
            }
            unrealTypes.Add(pickedPath);
            var finalName = resetItem(pickedPath, unreals[0]);
            int pointPos = finalName.LastIndexOf(',');
            if (pointPos != -1)
            {
                finalName = finalName.Substring(pointPos);
            }
            unrealFinalNames.Add(finalName);
            if (explain == "")
            {
                switch (pickedPath) {
                    case circlePath:
                        explain = string.Format( explainCircleOnFace,finalName);
                        break;
                    case squarePath:
                        explain = string.Format(explainSquareOnBody, finalName);
                        break;
                    case metalPartPath:
                        explain = string.Format(explainMissBodyPart, finalName);
                        break;
                
                }
                
            }
            for(int i = 1;i<unreals.Length;i++) 
            {
                var unreal = unreals[i];
                if (Random.Range(0, 3) > 1)
                {
                    pickedPath = pickUnrealPath();
                    if (unrealTypes.Contains(pickedPath))
                    {
                        break;
                    }
                    unrealTypes.Add(pickedPath);
                    unreal.gameObject.SetActive(true);
                    finalName = resetItem(pickUnrealPath(), unreal);
                    
                pointPos = finalName.LastIndexOf(',');
                if (pointPos != -1)
                {

                        finalName = finalName.Substring(pointPos);
                }
                    unrealFinalNames.Add(finalName);
                }
                else
                {
                    break;
                }
            }

            if (GameManager.Instance.currentRules.Contains(RealRule.tattooLie) && !isLying && Random.Range(0, 2) < 1 &&
                GameManager.Instance.latestRule != RealRule.metalBodyParts)
            {
                var pos = Utils.randomFromList(allTattooPosition);
                int slashPos = pos.LastIndexOf('/');
                var finalPos = pos.Substring(slashPos);
                pointPos = finalPos.LastIndexOf(',');
                if (pointPos != -1)
                {

                    finalPos = pos.Substring(pointPos);
                }

                if(GameManager.Instance.latestRule == RealRule.tattooLie)
                {
                    shouldResetLatestRule = true;
                    int loopTest = 0;
                    while (unrealFinalNames.Contains(finalPos))
                    {
                        loopTest++;
                        if(loopTest > 100)
                        {
                            Debug.LogError("loop too many");
                            break;
                        }
                        pos = Utils.randomFromList(allTattooPosition);
                        slashPos = pos.LastIndexOf('/');
                        finalPos = pos.Substring(slashPos);
                        pointPos = finalPos.LastIndexOf(',');
                        if (pointPos != -1)
                        {

                            finalPos = pos.Substring(pointPos);
                        }
                    }
                }

                if (!unrealFinalNames.Contains(finalPos))
                {
                    if (CheatManager.shouldLog)
                    {
                        Debug.Log($"lie about {pos}");
                    }
                    //lie about tattoo
                    isLying = true;

                    updateWordsOfExplain(false, finalPos);
                    explain = explainLieTatto;
                }
            }

            if (GameManager.Instance.currentRules.Contains(RealRule.metalPartLie) && !isLying && Random.Range(0, 2) < 1)
            {
                var pos = Utils.randomFromList(allMissingPartPosition);
                int slashPos = pos.LastIndexOf('/');
                var finalPos = pos.Substring(slashPos);
                pointPos = finalPos.LastIndexOf(',');
                if (pointPos != -1)
                {

                    finalPos = pos.Substring(pointPos);
                }

                if (GameManager.Instance.latestRule == RealRule.metalPartLie)
                {
                    shouldResetLatestRule = true;
                    int loopTest = 0;
                    while (unrealFinalNames.Contains(finalPos))
                    {
                        loopTest++;
                        if (loopTest > 100)
                        {
                            Debug.LogError("loop too many");
                            break;
                        }
                        pos = Utils.randomFromList(allMissingPartPosition);
                        slashPos = pos.LastIndexOf('/');
                        finalPos = pos.Substring(slashPos);
                        pointPos = finalPos.LastIndexOf(',');
                        if (pointPos != -1)
                        {

                            finalPos = pos.Substring(pointPos);
                        }
                    }
                }

                if (!unrealFinalNames.Contains(finalPos))
                {
                    if (CheatManager.shouldLog)
                    {
                        Debug.Log($"lie about {pos}");
                    }
                    //lie about tattoo
                    isLying = true;

                    updateWordsOfExplain(false, finalPos);
                    explain = explainLieTatto;
                }
            }
        }
    }

    public void resetOutfit()
    {

        if (!GameManager.Instance.currentRules.Contains(RealRule.hasClothes))
        {
            outfit.gameObject.SetActive(false);
        }
        else
        {
            outfit.gameObject.SetActive(true);
            resetItem(outfitPath, outfit);
        }
    }

    public void resetHair()
    {
        resetItem(hairPath,hair);
    }
    public void resetAccessory()
    {
        foreach(var accessory in accessories)
        {
            accessory.gameObject.SetActive(false);
        }

        if (GameManager.Instance.currentRules.Contains(RealRule.hasAccessory))
        {
            if (Utils.randomBool())
            {
                resetItem(accessoriesPath, accessories[0]);
            }
            if (Utils.randomBool())
            {
                resetItem(glovesPath, accessories[1]);
            }

            if (Utils.randomBool())
            {
                resetItem(unremoveableAccessoryPath, accessories[2]);
            }
        }
        //if (GameManager.Instance.level < 3)
        //{

            //    accessory.gameObject.SetActive(false);
            //}
            //else
            //{
            //    accessory.gameObject.SetActive(true);
            //}
            //resetItem(accessoriesPath,accessory);
    }

    public void decideIfReal()
    {
        isReal = GameManager.Instance.nextType() == 0;
    }

   
    public void resetCharacter()
    {
        explain = "";
        
        if (CheatManager.shouldLog)
        {
            Debug.Log("start reset");
        }
        decideIfReal();
        isLying = false;
        if (CheatManager.shouldLog)
        {
            Debug.Log($" it is real: {isReal}");
        }
        foreach (SpriteAnimator anim in GetComponentsInChildren<SpriteAnimator>())
        {

            anim.resetPosition();
        }
        setWords();
        resetBase();
        resetUnreal();
        resetOutfit();
        resetHair();
        resetAccessory();

        if(explain == "")
        {
            if (isReal)
            {
                explain = explainNormalHuman;
            }
            else
            {
                Debug.LogWarning("why no explain?");
            }
        }
        if (isLying)
        {
            explain = explainLie + explain;
        }

        if (shouldResetLatestRule)
        {
            GameManager.Instance.latestRule = RealRule.none;
            shouldResetLatestRule = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
