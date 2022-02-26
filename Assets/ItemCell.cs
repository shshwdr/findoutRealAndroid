using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;

    public Text costLabel;
    public Button button;
    Text detailText;

    public void updateCell(Item it, Text detail)
    {
        item = it;
        detailText = detail;
        costLabel.text = item.cost.ToString();
        var canBuy = item.canBuy;

        costLabel.color = canBuy ? Color.white : Color.red;
        button.onClick.AddListener(delegate { buy(); });
        button.interactable = canBuy;
        button.GetComponentInChildren<Text>().color = canBuy ? Color.white : Color.red;
    }

    public void updateCell(Item it, int count, Text detail)
    {
        item = it;
        detailText = detail;
        costLabel.text = $"x {count}";
        button.GetComponentInChildren<Text>().text = "Use";

        button.onClick.AddListener(delegate { use(); });
    }

    public void use()
    {

        ShopManager.Instance.use(item.name);
    }

    public void buy()
    {
        ShopManager.Instance.buy(item.name);
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
