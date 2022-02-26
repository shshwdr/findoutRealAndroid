using PixelCrushers;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUtils : Singleton<DialogueUtils>
{
    public bool isInDialogue;
    public List<GameObject> hideItems;
    public int saveSlotNumber = 1;
    GameObject controls;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        controls = GameObject.Find("controls");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void startConversation()
    {
        isInDialogue = true;
    }
    public void started()
    {

    }
    public void endConversation()
    {
        isInDialogue = false;
        EventPool.Trigger("dialogEnd");
    }
}
