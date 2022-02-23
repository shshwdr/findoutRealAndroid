using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
     SpriteRenderer PlayerSprite;
    Animator animator;
    public Sprite[] PlayerSpriteSheets;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        var horizontal =(int) Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            if(horizontal == 1)
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

    public void resetPosition()
    {

        animator.SetTrigger("down");
    }
    public void SetSprite(int id)
    {
        if(id>= PlayerSpriteSheets.Length)
        {
            PlayerSprite.sprite = null;
            Debug.LogWarning(id+" "+ PlayerSpriteSheets.Length);
            return;
        }
        PlayerSprite.sprite = PlayerSpriteSheets[id];
    }

    private void OnMouseDown()
    {
        Debug.Log($"on mouse down {name}");
        gameObject.SetActive(false);
    }
}
