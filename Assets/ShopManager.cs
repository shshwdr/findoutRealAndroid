using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item {
    public string name;
    public string displayName;
    public string desc;
    public List<int> costs;
    public int onlyHasOne;
    public int consumable;
    public bool canBuy { get { return CheatManager.Instance.hasUnlimitResource ||  GameManager.Instance.money >= cost; } }
    public bool wouldSell { get { return onlyHasOne == 0 || ShopManager.Instance.itemInventory[name] < 1; } }
    public int cost { get {   return costs[GameManager.Instance.currentGameStage]; } }
}

public class ShopManager : Singleton<ShopManager>
{
    public int health = 100;

    public bool ate;
    public bool slept;
    public bool assisted;
    public Dictionary<string, Item> itemInfoDict = new Dictionary<string, Item>();
    public Dictionary<string, int> itemInventory = new Dictionary<string, int>();
    private void Start()
    {

        var customerList = CsvUtil.LoadObjects<Item>("Item");
        foreach (var info in customerList)
        {
            itemInfoDict[info.name] = info;
        }
    }

    public void clearShop()
    {
        ate = false;
        slept = false;
        assisted = false;
    }
    public bool shouldAssist()
    {
        if(ShopManager.Instance.itemInventory.ContainsKey("Assist") && !assisted)
        {
            assisted = true;
            return true;
        }
        return false;
    }
    public void updateHealth()
    {
        if (!ate)
        {
            health -= 20;
        }
        if (!slept)
        {
            health -= 10;
        }
        if (itemInventory.ContainsKey("Succulents"))
        {

            health +=5;
        }

        if (health <= 0)
        {
            //game over
            if (CheatManager.shouldLog)
            {
                Debug.Log("game over");
            }
        }
        health = Mathf.Min(100, health);
        EventPool.Trigger("updateHealth");
    }

    public void buy(string name)
    {
        switch (name)
        {
            case "Food":
                ate = true;
                break;
            case "Bed":
                slept = true;
                break;
            default:
                itemInventory[name] +=1;
                break;
        }
        GameManager.Instance.money -= itemInfoDict[name].cost;
        EventPool.Trigger("updateMoney");
    }


    public void use(string name)
    {
        if (itemInventory[name] > 0)
        {
            switch (name)
            {
                case "emergency":
                    health += 15;
                    break;
            }
            itemInventory[name] -= 1;
        }

        EventPool.Trigger("updateHealth");
    }
}
