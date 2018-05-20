using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("Error Get Animator in player");
        }
        else
        {
            Debug.Log("Animator Loaded!!");
            animator.SetBool("walk", false);
        }

        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.Log("Error Get playerController");
        }
        else
        {
            Debug.Log("playerController Loaded!!");
        }


    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Walking()
    {
       animator.SetBool("walk", true);
    }

    public void Idle()
    {
       animator.SetBool("walk", false);
    }

    public void Win()
    {
        animator.SetTrigger("win");
    }

    public void Lose()
    {
        animator.SetTrigger("lose");
    }

}
