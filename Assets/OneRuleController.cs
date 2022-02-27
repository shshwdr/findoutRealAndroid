using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneRuleController : MonoBehaviour
{
    public Text ruleLabel;
    public Image image;
    public void init(int level)
    {
        ruleLabel.text = GameManager.Instance.getLevelText();
        var sprite = Resources.Load<Sprite>($"tutorial/tutorial{level}");
        if (sprite)
        {

            image.sprite = Resources.Load<Sprite>($"tutorial/tutorial{level}");
        }
        image.sprite = Resources.Load<Sprite>($"tutorial/tutorial{level}");
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
