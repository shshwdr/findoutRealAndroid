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

    string characterPath = "newCharacter/";
    string bodyPath = "newCharacter/Bodies";
    string circlePath = "newCharacter/circle";
    string squarePath = "newCharacter/square";
    string outfitPath = "newCharacter/outfit";
    string hairPath = "newCharacter/hair";
    string accessoriesPath = "newCharacter/Accessories";

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

    // Start is called before the first frame update
    void Start()
    {
        accessories = transform.Find("accessory").GetComponentsInChildren<SpriteAnimator>();
        body = transform.Find("body").GetComponent<SpriteAnimator>();
        outfit = transform.Find("outfit").GetComponent<SpriteAnimator>();
        unreals = transform.Find("unreal").GetComponentsInChildren<SpriteAnimator>();
        hair = transform.Find("hair").GetComponent<SpriteAnimator>();
        resetCharacter();
    }
    public void resetItem(string path, SpriteAnimator animator)
    {
        var test = Resources.LoadAll<Texture2D>(path);
        print(test);
        if(test.Length == 0)
        {
            return;
        }
        var cid = test[Random.Range(0, test.Length)];
        animator.gameObject.SetActive(true);
        animator.PlayerSpriteSheets = Resources.LoadAll<Sprite>(path + "/" + cid.name);
        if(animator.PlayerSpriteSheets.Length == 0)
        {
            Debug.Log($"{path} {cid.name}'s length is 0");
        }
    }
    public void resetBase()
    {
        resetItem(bodyPath,body);
    }

    string pickUnrealPath()
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
        if (possiblePath.Count == 0)
        {
            return null;
        }
        return possiblePath[Random.Range(0, possiblePath.Count)];
    }

    public void setWords()
    {
        if (!isReal)
        {
            if (GameManager.Instance.currentRules.Count == 1)
            {
                words.text = robotWords[Random.Range(0, robotWords.Length)];
                return;
            }
            else
            {
                var rand = Random.Range(0f, 1f) > 0.7f;
                if (rand)
                {
                    words.text = robotWords[Random.Range(0, robotWords.Length)];
                    return;
                }
            }
        }
        else
        {

        }
        var allWords = generalWords.Union(humanWords).ToArray();
        words.text = allWords[Random.Range(0, allWords.Length)];
    }
    public void resetUnreal()
    {
        if (isReal || GameManager.Instance.currentRules.Count == 1)
        {
            foreach(var unreal in unreals)
            {
                unreal.gameObject.SetActive(false);
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

        decideIfReal();
        
        foreach(SpriteAnimator anim in GetComponentsInChildren<SpriteAnimator>())
        {

            anim.resetPosition();
        }
        setWords();
        resetBase();
        resetUnreal();
        resetOutfit();
        resetHair();
        resetAccessory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
