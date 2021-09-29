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

    private void Start()
    {
        imageSize[0] = 0;
        imageSize[1] = 50;
        imageSize[2] = 100;
    }

    public void UIWeatherUpdate(int index, int type)
    {
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
                imageSize[i] = 150;
            }
            weatherImage[i].transform.localScale = new Vector3(1 - Mathf.Abs(imageSize[i] - 50) / 100, 1 - Mathf.Abs(imageSize[i] - 50) / 100,1);
            weatherImage[i].rectTransform.anchoredPosition = new Vector2(imageSize[i] - 50, 30);
        }
    }
}
