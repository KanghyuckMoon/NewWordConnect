using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        colliders = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
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
                Debug.Log(nowpassword);
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
