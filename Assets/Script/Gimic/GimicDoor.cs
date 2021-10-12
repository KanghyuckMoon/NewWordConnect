using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimicDoor : MonoBehaviour
{
    private Collider2D colliders;
    private Animator animator;
    private Vector2 originalVector = Vector2.zero;

    private void Start()
    {
        originalVector = transform.position;
        colliders = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void OnoffDoor(bool onoff)
    {
        if (onoff)
        {
            transform.DOLocalMoveY(originalVector.y - 3, 1f);
            colliders.enabled = false;
        }
        else
        {
            transform.DOLocalMoveY(originalVector.y, 1f);
            colliders.enabled = true;
        }
    }
}
