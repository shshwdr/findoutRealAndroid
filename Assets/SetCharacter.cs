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

    public Text words;
    public string explain;
    const string explainImRobot = "It said he's an android!";
    const string explainCircleOnFace = "It has a circle on face!";
    const string explainSquareOnBody = "It has a square on body!";
    const string explainMissBodyPart = "It has a metal body part!";


    const string explainNormalHuman = "He looks a perfect human!";
    const string explainExplainedTattoo = "He explained tattoo on body!";
    const string explainExplainedMissBodyPart = "He explained the missing body part!";


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

    
  


    // Start is called before the first frame update
    void Start()
    {
        accessories = transform.Find("accessory").GetComponentsInChildren<SpriteAnimator>(true);
        body = transform.Find("body").GetComponent<SpriteAnimator>();
        outfit = transform.Find("outfit").GetComponent<SpriteAnimator>();
        unreals = transform.Find("unreal").GetComponentsInChildren<SpriteAnimator>(true);
        hair = transform.Find("hair").GetComponent<SpriteAnimator>();
        //resetCharacter();
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
        animator.PlayerSpriteSheets = Resources.LoadAll<Sprite>(path + "/" + cid.name);
        if(animator.PlayerSpriteSheets.Length == 0)
        {
            Debug.Log($"{path} {cid.name}'s length is 0");
        }
        return cid.name;
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
        //int slashPos = path.LastIndexOf('/');
        //path = path.Substring(slashPos);
        string w = string.Format(generalWord, path);
        updateWords(w);
    }

    string pickUnrealPath(bool onlyTattoo = false)
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
        var path =  possiblePath[Random.Range(0, possiblePath.Count)];

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
                if (Random.Range(0, 2) > 0)
                {
                    var pickedPath = pickUnrealPath();
                    //if (unrealTypes.Contains(pickedPath))
                    //{
                    //    break;
                    //}
                    //unrealTypes.Add(pickedPath);
                    unreals[0].gameObject.SetActive(true);
                    string path = pickUnrealPath(true);
                    var itemName = resetItem(path, unreals[0]);


                    switch (pickedPath)
                    {
                        case circlePath:
                        case squarePath:
                            explain = explainExplainedTattoo;
                            break;
                        case metalPartPath:
                            explain = explainExplainedMissBodyPart;
                            break;

                    }

                    updateWordsOfExplain(true,itemName);
                    if (CheatManager.shouldLog)
                    {
                        Debug.Log("real people with a tattoo");
                    }
                }
            }
        }
        else
        {
            List<string> unrealTypes = new List<string>();
            var pickedPath = pickUnrealPath();
            if (pickedPath == null)
            {
                return;
            }
            unrealTypes.Add(pickedPath);
            resetItem(pickedPath, unreals[0]);
            if(explain == "")
            {
                switch (pickedPath) {
                    case circlePath:
                        explain = explainCircleOnFace;
                        break;
                    case squarePath:
                        explain = explainSquareOnBody;
                        break;
                    case metalPartPath:
                        explain = explainMissBodyPart;
                        break;
                
                }
                
            }
            for(int i = 1;i<unreals.Length;i++) 
            {
                var unreal = unreals[i];
                if (Random.Range(0, 2) > 0)
                {
                    pickedPath = pickUnrealPath();
                    if (unrealTypes.Contains(pickedPath))
                    {
                        break;
                    }
                    unrealTypes.Add(pickedPath);
                    unreal.gameObject.SetActive(true);
                    resetItem(pickUnrealPath(), unreal);
                }
                else
                {
                    break;
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
        isReal = Random.Range(0, 2) > 0;
    }
    public void resetCharacter()
    {
        explain = "";
        if (CheatManager.shouldLog)
        {
            Debug.Log("start reset");
        }
        decideIfReal();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
