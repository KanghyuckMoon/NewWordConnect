using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimicPassWordDoor : MonoBehaviour
{
    private Collider2D colliders;
    private Animator animator;
    [SerializeField]
    private int passwordColor;
    private string nowpassword;
    private bool onoff;
    [SerializeField]
    private List<GimicPassWord> gimicPassWords;
    private Vector2 originalVector = Vector2.zero;

    private void Start()
    {
        originalVector = transform.position;
        colliders = GetComponent<Collider2D>();
        StartCoroutine(CheakColor());
    }

    private IEnumerator CheakColor()
    {
        int a = 0;
        while (true)
        {
            nowpassword = null;
            for (int i = 0; i < gimicPassWords.Count; i++)
            {
                nowpassword += gimicPassWords[i].ReturnPassWord();
            }
            if (nowpassword != null)
            {
                a = int.Parse(nowpassword);
                if (a == passwordColor)
                {
                    onoff = true;
                    OnoffDoor();
                }
                else
                {
                    onoff = false;
                    OnoffDoor();
                }
            }
            yield return null;
        }
    }

    public void OnoffDoor()
    {
        if(onoff)
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
