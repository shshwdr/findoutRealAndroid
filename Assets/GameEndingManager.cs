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
                label.text = "You died from not getting enough food and rest. \nHuman wins the war, you were posthumously named a martyr.";
            }
            else
            {
                label.text = "You died from not getting enough food and rest. \nAndroid wins the war, you were posthumously named a traitor.";
            }
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
