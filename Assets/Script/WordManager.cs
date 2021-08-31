using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private List<int> subjectUnlock = new List<int>(); // 주어
    [SerializeField]
    private List<int> conditionUnlock = new List<int>(); // 조건어
    [SerializeField]
    private List<int> executionUnlock = new List<int>(); // 실행어

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
                Debug.Log(numberPressed);

                if (numberPressed == 0)
                {
                    CleanSelect();
                    return;
                }
                if (nowWord >= 3) return;

                switch(nowWord)
                {
                    case 0:
                        subjectWord = i;
                        nowWord = 1;
                        break;
                    case 1:
                        conditionWord = i;
                        nowWord = 2;
                        break;
                    case 2:
                        executionWord = i;
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
                            return;
                        case 2:
                            nowWord = 1;
                            conditionWord = 0;
                            return;
                        case 3:
                            nowWord = 2;
                            executionWord = 0;
                            return;
                        default:
                            return;
                    }
                }
            }
        }
    }
}
