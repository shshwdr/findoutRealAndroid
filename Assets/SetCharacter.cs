using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetCharacter : MonoBehaviour
{
    public Transform startTrans;
    public Transform middleTrans;
    public Transform endTrans;
    public Transform endUpTrans;
    public float moveTime = 0.3f;
    public GameObject rulesSelection;
    SpriteAnimator body;
    SpriteAnimator[] unreals;
    SpriteAnimator hair;
    SpriteAnimator outfit;
    SpriteAnimator[] accessories;
    public bool isReal;
    public bool isLying;
    public bool isMute;
    public bool wouldComplainClothes;
    public bool accounceHuman;
    public bool behaviorRobot;

    List<string> allTattooPosition = new List<string>();
    List<string> allMissingPartPosition = new List<string>();

    public Text words;
    public Text clothesLabel;
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
    string[][] warDialogues = new string[][]
    {
        new string[]{
            "The android at my home behave weirdly.",
            "Adroids are friends of human.",
            "Why are you looking for androids?",
            "What is the goverment doing to arrest androids?",
            "What is the goverment doing to arrest androids?",
        },
        new string[]{
            "Recently more androids behave weirdly.",
            "Will androids rebel?",
            "Why can't androids just do what we asked them to do?",
            "I always feel androids are bad.",
            "I 've never trusted androids.",
            "Shut down all androids and make human great again!",
        },new string[]{
            "The war.. starts..",
            "I've never expected the war to start.",
            "I made jokes about fight with androids but now I'm so afraid.",
            "I think human will win.. but with what price?",
            "I live at home without electric for days. It's horrible.",
            "Do we win?",
            "Androids killed my family. I hate them.",
        },
    };

    string[] robotWords = new string[]
    {
        "I'm a robot.",
        "I'm an android.",
        "I'm not a human.",
        "I'm a machine.",
        "I was made by human.",
        "I'm not real.",

    };

    string[] humanWords = new string[]
    {
        "I'm not a robot.",
        "I'm not an android.",
        "I'm a human.",
        "I'm real.",
        "I was born by human.",

    };

    string[] generalWords = new string[]
    {
        "Good day.",
        "Please don't kill me.",
        "My name is Connor.",
        "How's your day?",
        "Why did you arrest me?",
        "You look tired.",
    };

    string[] clothesWords = new string[]
    {
        "No! Please don't do this!",
        "This is sexual harassment!",
        "Don't take my clothes!",
        "Don't take my clothes!",
        "I am also dignified!",
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

    SpriteAnimator[] allAnimators;
    // Start is called before the first frame update
    void Start()
    {
        allAnimators= GetComponentsInChildren<SpriteAnimator>(true);
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
        animator.forcePosition = "";
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

    public void takeClothesOff()
    {
        if (wouldComplainClothes)
        {
            updateClothesWords();
        }
    }


    public void updateClothesWords()
    {
        clothesLabel.text = Utils.randomFromList(clothesWords);
        if (CheatManager.shouldLog)
        {
            Debug.Log($"say clothes words {clothesLabel.text}");
        }
        StartCoroutine(changeclothesWordsBack());
    }

    IEnumerator changeclothesWordsBack()
    {
        yield return new WaitForSeconds(1);
        clothesLabel.text = "";
    }
    public void setWords()
    {
        words.gameObject.SetActive(true);
        var generalw = generalWords.Union(warDialogues[GameManager.Instance.currentGameStage]).ToArray();

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
                    updateWords(Utils.randomFromList(generalw));
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
                                accounceHuman = true;
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
                                    updateWords(Utils.randomFromList(generalw));
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
                            updateWords(Utils.randomFromList(generalw));
                        }
                    }
                }
            }
        }
        else
        {
            var allWords = generalw.Union(humanWords).ToArray();
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
        rulesSelection.SetActive(false);
        explain = "";
        accounceHuman = false;
        isMute = false;
        wouldComplainClothes = true;
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
            GameManager.Instance.clearLatestRule();
            shouldResetLatestRule = false;
        }

        StartCoroutine(forceUpdateAnimate());
    }

    IEnumerator forceUpdateAnimate()
    {
        transform.position = startTrans.position;
        yield return new WaitForSeconds(0.01f);
        foreach (SpriteAnimator anim in allAnimators)
        {
            anim.resetPosition("right");
        }
        transform.DOMoveX(middleTrans.position.x, moveTime);
        yield return new WaitForSeconds(moveTime);
        foreach (SpriteAnimator anim2 in allAnimators)
        {
            if (anim2 == null)
            {
                rulesSelection = rulesSelection;
            }
            anim2.resetPosition("down");
        }
        rulesSelection.SetActive(true);
    }

    public void characterLeave(bool isReal)
    {

        rulesSelection.SetActive(false);
        StartCoroutine(forceUpdateAnimateLeave(isReal));

    }

    IEnumerator forceUpdateAnimateLeave(bool isReal)
    {
        foreach (SpriteAnimator anim in allAnimators)
        {
            if (anim == null)
            {
                rulesSelection = rulesSelection;
            }

            if (isReal)
            {
                anim.resetPosition("left");
                transform.DOMoveX(startTrans.position.x, moveTime);

            }
            else
            {

                anim.resetPosition("up");
                transform.DOMoveY(endUpTrans.position.y, moveTime);
            }
        }
        yield return new WaitForSeconds(moveTime);

        GameManager.Instance.nextCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
