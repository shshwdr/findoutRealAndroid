using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
     SpriteRenderer PlayerSprite;
    Animator animator;
    public Sprite[] PlayerSpriteSheets;

    public string forcePosition;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if (forcePosition.Length>0)
        {
            //Debug.LogError($"force position still has value {forcePosition}");
            if (CheatManager.shouldLog)
            {
                Debug.Log($"force move to {forcePosition}");
            }
            animator.SetTrigger(forcePosition);
            forcePosition = "";
        }
        else
        {
            var horizontal = (int)Input.GetAxisRaw("Horizontal");
            if (horizontal != 0)
            {
                if (horizontal == 1)
                {

                    animator.SetTrigger("right");
                }
                else
                {

                    animator.SetTrigger("left");
                }
            }
            else
            {

                var verticle = (int)Input.GetAxisRaw("Vertical");
                if (verticle != 0)
                {
                    if (verticle == -1)
                    {
                        animator.SetTrigger("down");
                    }
                    else
                    {

                        animator.SetTrigger("up");
                    }
                }
            }
        }
        
    }

    public void resetPosition(string dir)
    {
        //if(animator == null)
        //{

        //    animator = GetComponent<Animator>();
        //}
        //animator.SetTrigger("right");
        //if (forcePosition.Length > 0)
        //{
        //    forcePosition = "";
        //}
        //else
        {
            forcePosition = dir;
        }
    }
    public void SetSprite(int id)
    {
        if(id>= PlayerSpriteSheets.Length)
        {
            PlayerSprite.sprite = null;
            Debug.LogWarning(id+" "+ PlayerSpriteSheets.Length);
            return;
        }
        if(PlayerSpriteSheets == null || PlayerSprite == null)
        {

            //Debug.LogWarning("something is null");
            return;
        }
        PlayerSprite.sprite = PlayerSpriteSheets[id];
    }

    private void OnMouseDown()
    {
        Debug.Log($"on mouse down {name}");
        gameObject.SetActive(false);
        if(name == "outfit" && GetComponentInParent<SetCharacter>())
        {
            GetComponentInParent<SetCharacter>().takeClothesOff();
        }
    }
}
