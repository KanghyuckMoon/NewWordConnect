using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : WordGameObject
{
    [SerializeField]
    private float times = 0;

    private void Update()
    {
        times = Time.timeScale;
    }

    public override void Setting()
    {
        speed = user.speed;
        maxSpeed = user.maxspeed;
        friction = user.friction;
        airfriction = user.aitfriction;
        downGravityOn = user.downGravityOn;
        gravityScale = 0;
        jump = user.jump;

        rigid.drag = friction;
        rigid.gravityScale = 0;
    }

    public override void Jump()
    {
        Time.timeScale = 10;
        Invoke("Jumpoff", 0.1f);
    }
    private void Jumpoff()
    {
        Time.timeScale = 1;
    }
    public override void Down()
    {

    }
    public override void SpeedUp()
    {
        Time.timeScale = 1.5f;
        Invoke("SpeedReset", 1);
    }
    public override void SpeedDown()
    {
        Time.timeScale = 0.5f;
        Invoke("SpeedReset", 1);
    }
    public override void SpeedStop()
    {
        Time.timeScale = 0;
        DOVirtual.DelayedCall(1, SpeedReset,true);
    }
    public override void SpeedReset()
    {
        Time.timeScale = 1f;
    }

    public override void SizeUp()
    {
        if(sizeIndex == 0)
        {
            sizeIndex = 1;
            Time.timeScale = 1.2f;
        }
        else if(sizeIndex == -1)
        {
            sizeIndex = 0;
            Time.timeScale = 1;
        }
        
    }
    public override void SizeDown()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = -1;
            Time.timeScale = 0.8f;
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            Time.timeScale = 1;
        }

    }
}
