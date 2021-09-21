using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicMovingTile : GimicBase
{
    [SerializeField]
    private float distance = 1f;
    [SerializeField]
    private float upDownSpeed;
    [SerializeField]
    private float mass = 1;
    private bool updown;
    private Vector2 originalPosition = Vector2.zero;
    [SerializeField]
    private bool gravityOn = false;
    private Rigidbody2D rigid;

    protected override void Start()
    {
        realSpeed = 1;
        rigid = GetComponent<Rigidbody2D>();
        Settingvalue();
    }

    void FixedUpdate()
    {
        MovingPlatform();
    }

    protected void MovingPlatform()
    {
        if (originalPosition.y + distance < base.transform.position.y)
        {
            updown = true;
        }
        else if (originalPosition.y > base.transform.position.y)
        {
            updown = false;
        }
        rigid.AddForce(Vector2.up * realSpeed * upDownSpeed * (float)(updown ? -1 : 1), ForceMode2D.Impulse);
    }

    public override void SetGimicSpeed(float speed)
    {
        realSpeed = speed;
    }

    public void Settingvalue()
    {
        rigid.drag = 7.5f;
        rigid.mass = mass;
        if(gravityOn)
        {
        rigid.gravityScale = 4.300000190734863f;
        }
        else
        {
            rigid.gravityScale = 0;
        }
    }
}