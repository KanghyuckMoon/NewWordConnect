using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperentManager : WordGameObject
{
    //온도
    [SerializeField]
    public float s_Temperature = 50;

    public int tempdan = 0;

    private float notMoveTemperature = 0;
    private bool locktemp = false;

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

    public void OverTempSet()
    {
        if (s_Temperature <= 0) s_Temperature = 0;
        if (s_Temperature >= 100) s_Temperature = 100;
    }

    public override void Jump()
    {
        if (locktemp) return; 
        OverTempSet();
    }

    public override void Down()
    {
        if (locktemp) return;
        s_Temperature += 10;
        OverTempSet();
    }

    public override void SizeUp()
    {
        if (locktemp) return;
        s_Temperature -= 10;
        OverTempSet();
    }

    public override void SizeDown()
    {
        if (locktemp) return;
        s_Temperature += 10;
        OverTempSet();
    }

    public void GetWeather(int index)
    {
        switch(index)
        {
            case 0:
                locktemp = false;
                break;
            case 1:
                locktemp = false;
                break;
            case 2:
                locktemp = false;
                break;
            case 3: //비
                locktemp = false;
                s_Temperature -= 20;
                OverTempSet();
                break;
            case 5: //눈
                locktemp = true;
                s_Temperature = 100;
                break;
            case 6: //더위
                s_Temperature += 30;
                OverTempSet();
                break;

            default:
                locktemp = false;
                break;
        }
    }
}
