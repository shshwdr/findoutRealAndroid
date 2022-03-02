using Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public ItemCell[] shopItems;

    public ItemCell[] inventoryItems;
    public Text detailText;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void updateShop()
    {
        int i = 0;
        for (; i < ShopManager.Instance.itemInfoDict.Count; i++)
        {
            var value = ShopManager.Instance.itemInfoDict.Values.ToList()[i];
            if (value.wouldSell)
            {
                shopItems[i].gameObject.SetActive(true);
                shopItems[i].updateCell(value, detailText);
            }
            else
            {

                shopItems[i].gameObject.SetActive(false);
            }
        }
        for(;i< shopItems.Length; i++)
        {

            shopItems[i].gameObject.SetActive(false);
        }


        i = 0;
        for (; i < ShopManager.Instance.itemInventory.Count; i++)
        {
            var key = ShopManager.Instance.itemInventory.Keys.ToList()[i];
            var item = ShopManager.Instance.itemInfoDict[key];
            var value = ShopManager.Instance.itemInventory[key];
            if (value>0)
            {
                inventoryItems[i].gameObject.SetActive(true);
                inventoryItems[i].updateCell(item, value, detailText);
            }
            else
            {

                inventoryItems[i].gameObject.SetActive(false);
            }
        }
        for (; i < inventoryItems.Length; i++)
        {

            inventoryItems[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
