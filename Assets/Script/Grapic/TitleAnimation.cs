using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleAnimation : MonoBehaviour
{
    readonly private KeyCode[] keyCodes = {
    KeyCode.Keypad0,
    KeyCode.Keypad1,
    KeyCode.Keypad2,
    KeyCode.Keypad3,
    KeyCode.Keypad4,
    KeyCode.Keypad5,
    KeyCode.Keypad6,
    KeyCode.Keypad7,
    KeyCode.Keypad8,
    KeyCode.Keypad9,
    };

    [SerializeField]
    private RectTransform[] words;
    private Image images;
    private int select = 0;
    private bool loadingOn = false;


    private void Awake()
    {
        images = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(AnimationTitle());
    }

    private IEnumerator AnimationTitle()
    {
        float value = 10;
        while (value < 100)
        {
            value+=2;
            images.material.SetFloat("_PixelateSize", value);
            yield return new WaitForSeconds(0.01f);
        }
        images.material.DisableKeyword("PIXELATE_ON");
        yield return new WaitForSeconds(0.1f);
        images.material.DisableKeyword("GLITCH_ON");
        loadingOn = true;
    }

    private void Update()
    {
        if (loadingOn)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]) || Input.GetKeyDown(keyCodes[i] - 208))
                {
                    if(i < 4)
                    {
                        select = i;
                        MoveWord(select);
                    }
                }
            }
        }
    }

    private void MoveWord(int index)
    {
        for(int i = 0; i < 3; i++)
        {
            if(i + 1 == index)
            {
                words[i].DOAnchorPosY(-70,0.5f);
            }
            else
            {
                words[i].DOAnchorPosY(-120, 0.5f);
            }
        }
    }


}