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

    public void buy(string name)
    {
        switch (name)
        {
            case "Food":
                health += 20;
                break;
            case "Bed":
                health += 15;
                break;
            default:
                itemInventory[name] +=1;
                break;
        }
        GameManager.Instance.money -= itemInfoDict[name].cost;
        health = Mathf.Min(100, health);
        EventPool.Trigger("updateHealth");
        EventPool.Trigger("updateMoney");
    }


    public void use(string name)
    {
        if (itemInventory[name] > 0)
        {
            switch (name)
            {
                case "emergency":
                    health += 20;
                    break;
            }
            itemInventory[name] -= 1;
        }

        EventPool.Trigger("updateHealth");
    }
}
