using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : WordGameObject
{

    //날씨
    [Header("[날씨 순서] 날씨 오브젝트들 갯수보다 높은 숫자를 쓰지마시오")]
    [SerializeField]
    private List<int> s_Weather;
    private float s_WeatherPersent = 0;
    private float notmoveWeatherPersent = 0;
    private int s_WeatherCode = 0;

    [Header("[날씨 오브젝트들]")]
    public List<GameObject> weatherObjects;

    [SerializeField]
    private float autoWeatherPlus = 0;
    private float realautoWeatherPlus = 0;
    private int weatherselect = 1;
    private WeatherBar weatherBar;
    private TemperentManager temperentManager;


    private void FixedUpdate()
    {
        WeatherCheak();
    }

    public int ReturnToWeatherSize()
    {
        return s_Weather[s_Weather.Count - 1];
    }
    public int ReturnToWeatherCount()
    {
        return s_Weather.Count;
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
            if (((50 * s_Weather.Count - 1) / s_Weather.Count) * (i + 1) >= s_WeatherPersent && ((50 * s_Weather.Count - 1) / s_Weather.Count) * (i) <= s_WeatherPersent)
            {
                if (s_WeatherCode != s_Weather[i])
                {
                    weatherselect++;
                    if (weatherselect > 2) weatherselect = 0;
                    weatherBar.SetShader(weatherselect);
                    temperentManager.GetWeather(s_WeatherCode);
                }
                s_WeatherCode = s_Weather[i];
            }
        }
        s_WeatherPersent += realautoWeatherPlus;
        if (s_WeatherPersent <= 0) s_WeatherPersent = 50 * s_Weather.Count;
        if (s_WeatherPersent >= 50 * s_Weather.Count) s_WeatherPersent = 0;
        
    }

    protected override void Start()
    {
        //base.Start();
        realautoWeatherPlus = autoWeatherPlus;
        StartCoroutine(OnMoveDetect());
        weatherBar = FindObjectOfType<WeatherBar>();
        temperentManager = FindObjectOfType<TemperentManager>();
        weatherBar.UIWeatherUpdate(0, s_Weather[s_Weather.Count - 1]);
        weatherBar.UIWeatherUpdate(1, s_Weather[0]);
        weatherBar.UIWeatherUpdate(2, s_Weather[1]);
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
            if (notmoveWeatherPersent != s_WeatherPersent)
            {
                notmoveWeatherPersent = s_WeatherPersent;
                w_MoveOn = true;
                w_Movetime = 0f;
            }

            yield return waitForSeconds;
        }
    }


    public override void SizeUp()
    {
        s_WeatherPersent += 10;
        weatherBar.WeatherUp();
    }

    public override void SizeDown()
    {
        s_WeatherPersent -= 10;
    }

    public override void SpeedUp()
    {
        realautoWeatherPlus = autoWeatherPlus * 1.5f;
        Invoke("ResetSpeed",1);
    }
    public override void SpeedDown()
    {
        realautoWeatherPlus = autoWeatherPlus * 0.5f;
        Invoke("ResetSpeed", 1);
    }
    public override void SpeedStop()
    {
        realautoWeatherPlus = 0;
        Invoke("ResetSpeed", 1);
    }

    public void ResetSpeed()
    {
        realautoWeatherPlus = autoWeatherPlus;
    }

    public override void Jump()
    {
        s_WeatherPersent += 10;
        weatherBar.WeatherUp();
    }

    public override void Down()
    {
        s_WeatherPersent -= 10;
    }
}
