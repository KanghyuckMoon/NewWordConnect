using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperentManager : WordGameObject
{
    //¿Âµµ
    [SerializeField]
    public int s_Temperature = 50;

    public int tempdan = 0;

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
        s_Temperature -= 10;
        if (s_Temperature <= 0) s_Temperature = 0;
        if (s_Temperature >= 100) s_Temperature = 100;
    }

    public override void Down()
    {
        s_Temperature += 10;
        if (s_Temperature <= 0) s_Temperature = 0;
        if (s_Temperature >= 100) s_Temperature = 100;
    }

    public override void SizeUp()
    {
        s_Temperature -= 10;
        if (s_Temperature <= 0) s_Temperature = 0;
        if (s_Temperature >= 100) s_Temperature = 100;
    }

    public override void SizeDown()
    {
        s_Temperature += 10;
        if (s_Temperature <= 0) s_Temperature = 0;
        if (s_Temperature >= 100) s_Temperature = 100;
    }
}
