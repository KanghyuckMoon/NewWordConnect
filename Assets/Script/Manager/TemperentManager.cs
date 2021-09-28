using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperentManager : WordGameObject
{
    //¿Âµµ
    [SerializeField]
    public float s_Temperature = 50;

    public int tempdan = 0;

    private float notMoveTemperature = 0;

    protected override void Start()
    {
        StartCoroutine(OnMoveDetect());
    }

    protected override IEnumerator OnMoveDetect()
    {
        while (true)
        {
            if (w_Movetime < 0.02f)
            {
                w_Movetime += Time.deltaTime;

            }
            else
            {
                w_MoveOn = false;
                w_MoveOnEffect = true;
            }
            if (notMoveTemperature != s_Temperature)
            {
                notMoveTemperature = s_Temperature;
                w_MoveOn = true;
                w_Movetime = 0f;
            }

            yield return waitForSeconds;
        }
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
