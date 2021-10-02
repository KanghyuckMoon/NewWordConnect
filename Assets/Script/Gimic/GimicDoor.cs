using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicDoor : MonoBehaviour
{
    private Collider2D colliders;
    private Animator animator;

    private void Start()
    {
        colliders = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void OnoffDoor(bool onoff)
    {
        if(onoff)
        {
            animator.SetBool("Onoff",true);
            colliders.enabled = false;
        }
        else
        {
            animator.SetBool("Onoff", false);
            colliders.enabled = true;
        }
    }
}
