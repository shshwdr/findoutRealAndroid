using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;
    bool consumed;
    public Text nameLabel;
    public Text costLabel;
    public Button button;
    Text detailText;
    bool isShop;

    public void updateCell(Item it, Text detail)
    {
        isShop = true;
        item = it;

        nameLabel.text = it.displayName;
        detailText = detail;
        costLabel.text = item.cost.ToString();
        consumed = false;
        updateColor();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { buy(); });
        button.GetComponentInChildren<Text>().text = "Buy";
    }

    void updateColor()
    {
        if (isShop)
        {
            var canBuy = item.canBuy;

            costLabel.color = canBuy ? Color.black : Color.red;
            // button.interactable = canBuy;
            button.GetComponentInChildren<Text>().color = canBuy ? Color.black : Color.red;
            if (consumed)
            {

                button.GetComponentInChildren<Text>().text = "Consumed";
            }
        }
        else
        {

            costLabel.text = $"x {ShopManager.Instance.itemInventory[item.name]}";
        }
    }

    public void updateCell(Item it, int count, Text detail)
    {
        isShop = false;
        item = it;
        nameLabel.text = it.displayName;
        detailText = detail;
        updateColor();
        button.GetComponentInChildren<Text>().text = "Use";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { use(); });
    }

    public void use()
    {
        ShopManager.Instance.use(item.name);
        updateColor();
    }

    public void buy()
    {
        if (consumed)
        {

            DialogueManager.ShowAlert("You've bought it");
        }
        else
        {
            if (item.canBuy && item.wouldSell)
            {

                ShopManager.Instance.buy(item.name);
                if (item.consumable == 1)
                {
                    consumed = true;
                }
            }
            else
            {
                if (!item.canBuy)
                {

                    DialogueManager.ShowAlert("Not Enough Money");
                }
                else if (!item.wouldSell)
                {

                    DialogueManager.ShowAlert("You already have it");
                }
            }
        }
        updateColor();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        detailText.text = item.desc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailText.text = "";
    }
}
