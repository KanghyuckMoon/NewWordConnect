using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : WordGameObject
{

    //³¯¾¾
    [SerializeField]
    public List<int> s_Weather;
    public int s_WeatherPersent = 0;
    public int s_WeatherCode = 0;

    public List<GameObject> weatherObjects;

    private void Update()
    {
        WeatherCheak();
    }

    private void WeatherCheak()
    {
        for(int i = 0; i < s_Weather.Count; i++)
        {
            if (s_WeatherCode == i)
            {
                weatherObjects[i].SetActive(true);
            }
            else
            {
                weatherObjects[i].SetActive(false);
            }
            if ((100 / s_Weather.Count) * (i + 1) >= s_WeatherPersent && (100 / s_Weather.Count) * (i) <= s_WeatherPersent)
            {
                s_WeatherCode = s_Weather[i];
            }
        }

    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Setting()
    {
        gravityScale = 0;
        rigid.gravityScale = 0;
    }



    public override void SizeUp()
    {
        s_WeatherPersent += 10;
        if (s_WeatherPersent <= 0) s_WeatherPersent = 100;
        if (s_WeatherPersent >= 100) s_WeatherPersent = 0;
    }

    public override void SizeDown()
    {
        s_WeatherPersent -= 10;
        if (s_WeatherPersent <= 0) s_WeatherPersent = 100;
        if (s_WeatherPersent >= 100) s_WeatherPersent = 0;
    }

    public override void Jump()
    {
        s_WeatherPersent += 10;
        if (s_WeatherPersent <= 0) s_WeatherPersent = 100;
        if (s_WeatherPersent >= 100) s_WeatherPersent = 0;
    }

    public override void Down()
    {
        s_WeatherPersent -= 10;
        if (s_WeatherPersent <= 0) s_WeatherPersent = 100;
        if (s_WeatherPersent >= 100) s_WeatherPersent = 0;
    }
}
