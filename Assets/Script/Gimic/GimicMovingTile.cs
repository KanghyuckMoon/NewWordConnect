using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField]
    private bool rightleft = false;

    protected override void Start()
    {
        originalPosition = transform.position;
        realSpeed = 1;
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
        transform.Translate(Vector2.up * realSpeed * upDownSpeed * (float)(updown ? -1 : 1));
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
        transform.Translate(Vector2.right * realSpeed * upDownSpeed * (float)(updown ? -1 : 1));
    }


    public override void SetGimicSpeed(float speed)
    {
        realSpeed = speed;
    }

}
