using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWing : EnemyBased
{
    [SerializeField]
    private float distance = 1f;
    private bool updown;
    private Vector2 originalPosition = Vector2.zero;
    [SerializeField]
    private bool gravityOn = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void EnemyMove()
    {
        if (originalPosition.y + distance < base.transform.position.y)
        {
            updown = true;
        }
        else if (originalPosition.y > base.transform.position.y)
        {
            updown = false;
        }
        rigid.AddForce(Vector2.up * enemymoveSpeed * (float)(updown ? -1 : 1), ForceMode2D.Impulse);
    }

    public override void Settingvalue()
    {
        base.Settingvalue();
        if(!gravityOn)
        {
        rigid.gravityScale = 0;

        }
    }
}
