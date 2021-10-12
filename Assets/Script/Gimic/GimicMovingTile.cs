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
    [SerializeField]
    private bool rightleft = false;

    protected override void Start()
    {
        originalPosition = transform.position;
        realSpeed = 1;
        rigid = GetComponent<Rigidbody2D>();
        Settingvalue();
    }

    void FixedUpdate()
    {
        if(rightleft)
        {
            MovingRightLeftPlatform();
        }
        else
        {
        MovingPlatform();
        }
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
    protected void MovingRightLeftPlatform()
    {
        if (originalPosition.x + distance < base.transform.position.x)
        {
            updown = true;
        }
        else if (originalPosition.x > base.transform.position.x)
        {
            updown = false;
        }
        rigid.AddForce(Vector2.right * realSpeed * upDownSpeed * (float)(updown ? -1 : 1), ForceMode2D.Impulse);
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
