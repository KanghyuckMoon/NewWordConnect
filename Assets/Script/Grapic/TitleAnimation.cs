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
    private RectTransform[] mainwords;
    [SerializeField]
    private RectTransform[] startwords;
    [SerializeField]
    private RectTransform[] optionwords;
    private Image images;
    private int select = 0;
    private int nowbarselect = 0;
    private int optionselect = 0;
    private int lastselect = 0;
    private bool loadingOn = false;
    [SerializeField]
    private Text spacetext = null;

    [SerializeField]
    private RectTransform Mainbar = null;
    [SerializeField]
    private RectTransform Startbar = null;
    [SerializeField]
    private RectTransform Optionbar = null;

    [SerializeField]
    private RectTransform optionarrow = null;
    [SerializeField]
    private RectTransform[] optionRectTexts = null;
    [SerializeField]
    private Text[] optionTexts = null;

    private KeySetting keysetting;


    private void Awake()
    {
        images = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(AnimationTitle());
        keysetting = SaveManager.Instance.CurrenKeySetting;
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
        for(int i = 0; i < 3; i++)
        {
            mainwords[i].DOAnchorPosY(-120, 0.3f);
        }
        yield return new WaitForSeconds(0.2f);
        loadingOn = true;
    }

    private void Update()
    {
        if (loadingOn)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[0]) || Input.GetKeyDown(KeyCode.Backspace))
                {
                    select = 0;
                    spacetext.color = new Color(1, 1, 1, 0);
                    spacetext.DOKill();
                    MoveWord(0);
                    if (nowbarselect != 0)
                        MoveScreen(select);

                }
                else if (Input.GetKeyDown(keyCodes[i] - (keysetting.Numpad ? 0 : 208)))
                {
                    if(i < 4)
                    {
                        select = i;
                        MoveWord(select);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if(nowbarselect == 0)
                    {
                        MoveScreen(select);
                        select = 0;
                        optionselect = 0;
                    }
                    else if(nowbarselect == 2 && select != 0)
                    {
                        StartGame(select);
                    }
                }
            }

            if(nowbarselect == 1)
            {
                if(Input.GetKeyDown(KeyCode.W))
                {
                    OptionSelect(-1);
                    MoveOption();
                }
                else if(Input.GetKeyDown(KeyCode.S))
                {
                    OptionSelect(1);
                    MoveOption();
                }
            }
        }
    }

    private void MoveWord(int index)
    {
        for(int i = 0; i < 3; i++)
        {
            switch(nowbarselect)
            {
                case 0:
                    if (i + 1 == index)
                    {
                        mainwords[i].DOAnchorPosY(-70, 0.5f);
                        spacetext.color = new Color(1, 1, 1, 0);
                        spacetext.DOKill();
                        spacetext.DOColor(new Color(1, 1, 1, 1), 1f);
                        switch (i)
                        {
                            case 0:
                                spacetext.text = "-스페이스를 눌러 게임을 설정합니다-";
                                break;
                            case 1:
                                spacetext.text = "-스페이스를 눌러 게임을 시작합니다-";
                                break;
                            case 2:
                                spacetext.text = "-스페이스를 눌러 게임을 종료합니다-";
                                break;
                        }
                    }
                    else
                    {
                        mainwords[i].DOAnchorPosY(-120, 0.5f);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    if (i + 1 == index)
                    {
                        startwords[i].DOAnchorPosX(60, 0.5f);
                    }
                    else
                    {
                        startwords[i].DOAnchorPosX(95.58f, 0.5f);
                    }
                    break;
            }
            
        }
    }

    private void MoveScreen(int index)
    {
        if (nowbarselect == 0 || index == 0)
        {
            nowbarselect = select;
        }
        else if (nowbarselect == index)
        {
            SetLoadingEnd();
            return;
        }
        switch (index)
        {
            case 0:
                Mainbar.DOAnchorPosX(0, 1).OnComplete(() => SetLoadingEnd()); ;
                Startbar.DOAnchorPosX(700, 1);
                Optionbar.DOAnchorPosX(-700, 1);
                break;
            case 1: // 옵션
                Mainbar.DOAnchorPosX(700, 1).OnComplete(() => SetLoadingEnd());
                Startbar.DOAnchorPosX(1400, 1);
                Optionbar.DOAnchorPosX(0, 1);
                break;
            case 2: // 스타트
                Mainbar.DOAnchorPosX(-700, 1).OnComplete(() => SetLoadingEnd());
                Startbar.DOAnchorPosX(0, 1);
                Optionbar.DOAnchorPosX(-1400, 1);
                break;
        }
    }

    private void MoveOption()
    {
        optionarrow.DOAnchorPosY(optionRectTexts[optionselect].anchoredPosition.y,0.2f);
    }

    private void OptionSelect(int i)
    {
        if (optionselect + i < 0) return;
        if (optionselect + i > optionRectTexts.Length - 1) return;
        optionselect += i;
    }

    private void StartGame(int index)
    {
        loadingOn = false;
        SaveManager.Instance.SetSaveUserData(index);
        Startbar.DOAnchorPosX(-1500, 2).OnComplete(() => GoToStageSelect());
    }

    private void SetLoadingEnd()
    {
        loadingOn = true;
    }

    private void GoToStageSelect()
    {
        if(SaveManager.Instance.CurrentSaveUser.writingData)
        {
            SceneManager.LoadScene("StageSelect");
        }
        else
        {
            SceneManager.LoadScene("StoryIntro");
        }
    }

}