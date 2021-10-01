using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherBar : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private Image[] weatherImage;
    private float[] imageSize = new float[3];
    private int[] imageindex = new int[3];
    private WeatherManager weatherManager;

    private void Awake()
    {
        weatherManager = FindObjectOfType<WeatherManager>();
        imageSize[0] = 0;
        imageSize[1] = 50;
        imageSize[2] = 100;
        SetShader(1);
    }

    public void SetShader(int index)
    {
        for(int i = 0; i < 3; i++)
        {
            if(i == index)
            {
                weatherImage[i].material.EnableKeyword("SHINE_ON");
            }
            else
            {
                weatherImage[i].material.DisableKeyword("SHINE_ON");
            }
        }
    }

    public void UIWeatherUpdate(int index, int type)
    {
        if(type > weatherManager.ReturnToWeatherSize())
        {
            type -= weatherManager.ReturnToWeatherSize() + 1;
        }
        imageindex[index] = type;
        weatherImage[index].sprite = sprites[type];
    }

    public void WeatherUp()
    {
        //다들 왼쪽으로 이동
        for(int i = 0; i < 3; i++)
        {
            imageSize[i] -= 10;
            if (imageSize[i] < 0)
            {
                UIWeatherUpdate(i, imageindex[i] + 3);
                imageSize[i] = 140;
            }
            weatherImage[i].transform.localScale = new Vector3(1 - Mathf.Abs(imageSize[i] - 50) / 100, 1 - Mathf.Abs(imageSize[i] - 50) / 100,1);
            weatherImage[i].rectTransform.anchoredPosition = new Vector2(imageSize[i] - 50, 10);
        }
    }
}
