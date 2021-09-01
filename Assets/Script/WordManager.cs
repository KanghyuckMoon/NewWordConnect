﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    private KeyCode[] keyCodes = {
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

    private List<string> subjectlist = new List<string>() { " ","용사가", "모든 적이", "스테이지가", "카메라가", "날씨가", "온도가", "게임창이", "소리가", "특수"};
    private List<string> conditionlist = new List<string>() { " ","1초 마다", "가만히 있을 때", "충돌할 때", "블록을 밟을 때", "입력할 때", "떨어질 때", "카메라에 보일 때", "소리를 낼 때", "특수" };
    private List<string> executionlist = new List<string>() { " ","뛰어 오른다", "1초 동안 빨라진다", "1초 동안 정지한다", "1초 동안 느려진다", "떨어진다", "커진다", "작아진다", "충돌하지 않는다", "특수" };



    [SerializeField]
    private List<int> subjectUnlock = new List<int>(){ 
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9
    };
    [SerializeField]
    private List<int> conditionUnlock = new List<int>()
         {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9
    }; // 조건어
    [SerializeField]
    private List<int> executionUnlock = new List<int>()
         {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9
    }; // 실행어

    [SerializeField]
    private int subjectWord = 0;

    [SerializeField]
    private int conditionWord = 0;

    [SerializeField]
    private int executionWord = 0;

    [SerializeField]
    private int nowWord = 0;

    [SerializeField]
    private List<GameObject> wordSelect = new List<GameObject>(); // 선택되는 오브젝트
    [SerializeField]
    private int selectCount;

    [SerializeField]
    private UIManager uIManager;

    [SerializeField]
    private Text subjectText = null;
    [SerializeField]
    private Text conditionText = null;
    [SerializeField]
    private Text executionText = null;

    private void Update()
    {
        InputWordKey();

        //테스트용
        SelectCount();
    }



    //테스트용 함수
    private void SelectCount()
    {
        selectCount = wordSelect.Count;
    }


    //직접 사용할 함수
    private void CleanSelect()
    {
        subjectWord = 0;
        conditionWord = 0;
        executionWord = 0;
        nowWord = 0;
        wordSelect.Clear();
    }

    private void InputWordKey()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) || Input.GetKeyDown(keyCodes[i] - 208))
            {
                int numberPressed = i;
                //Debug.Log(numberPressed);
                if (uIManager.panelCount < numberPressed) return;
                if (numberPressed == 0)
                {
                    CleanSelect();
                    return;
                }
                if (nowWord >= 3) return;

                switch(nowWord)
                {
                    case 0:
                        subjectWord = OutToNowWord(numberPressed, 0);
                        nowWord = 1;
                        break;
                    case 1:
                        conditionWord = OutToNowWord(numberPressed, 1);
                        nowWord = 2;
                        break;
                    case 2:
                        executionWord = OutToNowWord(numberPressed, 2);
                        nowWord = 3;
                        break;
                    default:
                        nowWord = 3;
                        break;
                }
            }
            else if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.KeypadPeriod))
            {
                if(nowWord <= 0)
                {
                    nowWord = 0;
                    return;
                }
                else
                {
                    switch(nowWord)
                    {
                        case 1:
                            nowWord = 0;
                            subjectWord = 0;
                            subjectText.text = " ";
                            return;
                        case 2:
                            nowWord = 1;
                            conditionWord = 0;
                            conditionText.text = " ";
                            return;
                        case 3:
                            nowWord = 2;
                            executionWord = 0;
                            executionText.text = " ";
                            return;
                        default:
                            return;
                    }
                }
            }
        }
    }

    public void GetkeyNum(int num)
    {
        switch(num)
        {
            case 1:
                One();
                break;
            case 2:
                Two();
                break;
            case 3:
                Three();
                break;
            default:
                Debug.Log("asdsadsa");
                break;
        }
    }

    private int OutToNowWord(int i,int type)
    {
        Debug.Log(subjectUnlock.Count);
        Debug.Log(subjectlist.Count);
        switch (type)
        {
            case 0://주어
                subjectText.text = string.Format("{0}", subjectlist[subjectUnlock[i]]);
                return subjectUnlock[i];
            case 1://조건어
                conditionText.text = string.Format("{0}", conditionlist[conditionUnlock[i]]);
                return conditionUnlock[i];
            case 2://실행어
                executionText.text = string.Format("{0}", executionlist[executionUnlock[i]]);
                return executionUnlock[i];
        }
        return 0;
    }
    public void One()
    {
        Debug.Log("11");
    }
    public void Two()
    {
        Debug.Log("22");
    }
    public void Three()
    {
        Debug.Log("33");
    }
}
