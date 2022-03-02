using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndingManager : Singleton<GameEndingManager>
{

    public GameObject gameOverObject;


    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverObject.SetActive(true);
        var label = gameOverObject.GetComponentInChildren<Text>();
        if (ShopManager.Instance.health <= 0)
        {
            if (GameManager.Instance.currentGameStage >= 3)
            {
                label.text = "You died from not getting enough food and rest. \nHuman won the war, you were posthumously named a martyr.";
            }
            else
            {
                label.text = "You died from not getting enough food and rest. \nAndroid won the war, you were posthumously named a traitor.";
            }
        }
        if (GameManager.Instance.atMaxLevel())
        {
            if(GameManager.Instance.makeRobotToHuman + GameManager.Instance.makeRobotToHuman <= 2)
            {

                label.text = "Human won the war, you did perfect in the war.\nPeople admire you then think you are too perfect to be true.\nThey did a check on you and find out you are an android. You are Dismantled.";
            }
            else
            {

                if (GameManager.Instance.makeRobotToHuman > GameManager.Instance.makeRobotToHuman)
                {

                    label.text = "You are caught and killed because you let too many android leave as human. \nAndroid won the war and you were posthumously named a martyr.";
                }
                else
                {

                    label.text = "Human won the war. \nYou treat too many human as android and they were angry after the war.\nYou die alone and lonely.";
                }
            }
        }
        else
        {

            label.text = "The war is endless..";
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
