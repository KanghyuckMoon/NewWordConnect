using System.Collections;
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

    [SerializeField]
    private List<string> subjectlist = new List<string>() { " ","용사가", "모든 적이", "스테이지가", "카메라가", "날씨가", "온도가", "게임창이", "소리가", "특수"};
    [SerializeField]
    private List<string> conditionlist = new List<string>() { " ","1초 마다", "가만히 있을 때", "충돌할 때", "블록을 밟을 때", "입력할 때", "떨어질 때", "카메라에 보일 때", "소리를 낼 때", "특수" };
    [SerializeField]
    private List<string> executionlist = new List<string>() { " ","뛰어 오른다", "1초 동안 빨라진다", "1초 동안 정지한다", "1초 동안 느려진다", "떨어진다", "커진다", "작아진다", "충돌하지 않는다", "특수" };

    [SerializeField]
    private List<int> subjectUnlock = new List<int>();//주어
    [SerializeField]
    private List<int> conditionUnlock = new List<int>();// 조건어
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


    [SerializeField]
    private Text subjectText = null;
    [SerializeField]
    private Text conditionText = null;
    [SerializeField]
    private Text executionText = null;

    //UI
    [SerializeField]
    private GameObject subjectScroll;
    [SerializeField]
    private GameObject conditionScroll;
    [SerializeField]    
    private GameObject executionScroll;
    [SerializeField]
    private List<GameObject> panellistSubject;
    [SerializeField]
    private List<GameObject> panellistCondition;
    [SerializeField]
    private List<GameObject> panellistExecution;

    private void Start()
    {
        SetListUI(); //UI 갯수 세는 함수
        SetSizeListUI(); //UI 크기 맞추는 함수
        AllChangeTexts(); //텍스트 바꾸는 함수
        OnOffScroll(); //스크롤 껐다 키는 함수
        ResetOnClick(); // 버튼 기능들 리셋시키고 다시 부여
    }

    private void Update()
    {
        InputWordKey();

        //테스트용
        SelectCount();
    }
    public void AllChangeTexts()
    {
        switch(nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    subjectScroll.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        subjectlist[subjectUnlock[i + 1]];
                }
                break;
            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    conditionScroll.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        conditionlist[conditionUnlock[i + 1]];
                }
                break;
            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    executionScroll.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        executionlist[executionUnlock[i + 1]];
                }
                break;
            case 3:
                break;
            default:
                break;
        }
        
    }

    public void CreatePanel()
    {
        GameObject newPanel = null;
        switch (nowWord)
        {
            case 0:
                if (subjectScroll.transform.childCount >= 9) return;
                newPanel = Instantiate(subjectScroll.transform.GetChild(0).gameObject, subjectScroll.transform.GetChild(0).transform.parent);
                panellistSubject.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = subjectlist[subjectUnlock[panellistSubject.Count]];
                break;
            case 1:
                if (conditionScroll.transform.childCount >= 9) return;
                newPanel = Instantiate(conditionScroll.transform.GetChild(0).gameObject, conditionScroll.transform.GetChild(0).transform.parent);
                panellistCondition.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = conditionlist[conditionUnlock[panellistCondition.Count]];
                break;
            case 2:
                if (executionScroll.transform.childCount >= 9) return;
                newPanel = Instantiate(executionScroll.transform.GetChild(0).gameObject, executionScroll.transform.GetChild(0).transform.parent);
                panellistCondition.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = executionlist[executionUnlock[panellistExecution.Count]]; 
                break;
            case 3:
                break;
            default:
                break;
        }
        ResetOnClick();
        SetListUI();
        SetSizeListUI();
        AllChangeTexts();
    }
    public void ResetOnClick()
    {
        for(int i = 0; i < subjectScroll.transform.childCount;i++)
        {
            int temp = i;
            subjectScroll.transform.GetChild(temp).GetComponent<Button>().onClick.RemoveAllListeners();
            subjectScroll.transform.GetChild(temp).GetComponent<Button>().onClick.AddListener(delegate { ClickOnWordSelect(temp + 1); });
        }
        for (int i = 0; i < conditionScroll.transform.childCount; i++)
        {
            int temp = i;
            conditionScroll.transform.GetChild(temp).GetComponent<Button>().onClick.RemoveAllListeners();
            conditionScroll.transform.GetChild(temp).GetComponent<Button>().onClick.AddListener(delegate { ClickOnWordSelect(temp + 1); });
        }
        for (int i = 0; i < executionScroll.transform.childCount; i++)
        {
            int temp = i;
            executionScroll.transform.GetChild(temp).GetComponent<Button>().onClick.RemoveAllListeners();
            executionScroll.transform.GetChild(temp).GetComponent<Button>().onClick.AddListener(delegate { ClickOnWordSelect(temp + 1); });
        }
    }
    public void BackPanel()
    {
        GameObject backPanel = null;
        switch (nowWord)
        {
            case 0:
                if (subjectScroll.transform.childCount == 1) return;
                
                backPanel = subjectScroll.transform.GetChild(subjectScroll.transform.childCount - 1).gameObject;
                Destroy(backPanel);
                break;
            case 1:
                if (conditionScroll.transform.childCount == 1) return;
                backPanel = conditionScroll.transform.GetChild(conditionScroll.transform.childCount - 1).gameObject;
                Destroy(backPanel);
                break;
            case 2:
                if (executionScroll.transform.childCount == 1) return;

                backPanel = executionScroll.transform.GetChild(executionScroll.transform.childCount - 1).gameObject;
                Destroy(backPanel);
                break;

        }
        SetListUI();
        SetSizeListUI();
        switch(nowWord)
        {
            case 0:
                panellistSubject.RemoveAt(panellistSubject.Count - 1);
                break;

            case 1:
                panellistCondition.RemoveAt(panellistCondition.Count - 1);
                break;

            case 2:
                panellistExecution.RemoveAt(panellistExecution.Count - 1);
                break;
        }
    }
    public void ClearPanel()
    {
        int count = 0;
        switch (nowWord)
        {
            case 0:
                count = subjectScroll.transform.childCount;
                for (int i = 1; i < count; i++)
                {
                    Destroy(subjectScroll.transform.GetChild(i).gameObject);
                    panellistSubject.RemoveAt(panellistSubject.Count - 1);
                }
                break;
            case 1:
                count = conditionScroll.transform.childCount;
                for (int i = 1; i < count; i++)
                {
                    Destroy(conditionScroll.transform.GetChild(i).gameObject);
                    panellistCondition.RemoveAt(panellistCondition.Count - 1);
                }
                break;
            case 2:
                count = executionScroll.transform.childCount;
                for (int i = 1; i < count; i++)
                {
                    Destroy(executionScroll.transform.GetChild(i).gameObject);
                    panellistExecution.RemoveAt(panellistExecution.Count - 1);
                }
                break;
            default:
                break;
        }
        
        SetSizeListUI();
    }
    private void SetListUI()
    {
                panellistSubject.Clear();
                for (int i = 0; i < subjectScroll.transform.childCount; i++)
                {
                    panellistSubject.Add(subjectScroll.transform.GetChild(i).gameObject);
                };
                panellistCondition.Clear();
                for (int i = 0; i < conditionScroll.transform.childCount; i++)
                {
                    panellistCondition.Add(conditionScroll.transform.GetChild(i).gameObject);
                };
                panellistExecution.Clear();
                for (int i = 0; i < executionScroll.transform.childCount; i++)
                {
                    panellistExecution.Add(executionScroll.transform.GetChild(i).gameObject);
                };
    }
    private void SetSizeListUI()
    {
        SetSizeListImage();
        SetSizeListText();
    }

    private void SetSizeListImage() // 패널의 이미지 조절
    {
        switch(nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    if (panellistSubject.Count == 1)
                    {
                        panellistSubject[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistSubject.Count == 0) return;
                    panellistSubject[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta =
                        new Vector2(100 / (panellistSubject.Count * 0.5f),
                        100 / (panellistSubject.Count * 0.5f));
                }
                break;

            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    if (panellistCondition.Count == 1)
                    {
                        panellistCondition[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistCondition.Count == 0) return;
                    panellistCondition[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta =
                        new Vector2(100 / (panellistCondition.Count * 0.5f),
                        100 / (panellistCondition.Count * 0.5f));
                }
                break;

            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    if (panellistExecution.Count == 1)
                    {
                        panellistExecution[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistExecution.Count == 0) return;
                    panellistExecution[i].transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta =
                        new Vector2(100 / (panellistExecution.Count * 0.5f),
                        100 / (panellistExecution.Count * 0.5f));
                }
                break;
            case 3:
                break;
            default:
                break;
        }
        
    }
    private void SetSizeListText() //패널의 텍스트 사이즈 조절
    {
        switch(nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    if (panellistSubject.Count == 1)
                    {
                        panellistSubject[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                               new Vector2(640 / panellistSubject.Count,
                               panellistSubject[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                        panellistSubject[i].transform.GetChild(1).GetComponent<Text>().fontSize = 60;
                        return;
                    }
                    panellistSubject[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                           new Vector2(640 / panellistSubject.Count,
                           panellistSubject[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                    panellistSubject[i].transform.GetChild(1).GetComponent<Text>().fontSize = OuttoFontSize(panellistSubject.Count);
                }
                break;

            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    if (panellistCondition.Count == 1)
                    {
                        panellistCondition[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                               new Vector2(640 / panellistCondition.Count,
                               panellistCondition[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                        panellistCondition[i].transform.GetChild(1).GetComponent<Text>().fontSize = 60;
                        return;
                    }
                    panellistCondition[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                           new Vector2(640 / panellistCondition.Count,
                           panellistCondition[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                    panellistCondition[i].transform.GetChild(1).GetComponent<Text>().fontSize = OuttoFontSize(panellistCondition.Count);
                }
                break;

            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    if (panellistExecution.Count == 1)
                    {
                        panellistExecution[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                               new Vector2(640 / panellistExecution.Count,
                               panellistExecution[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                        panellistExecution[i].transform.GetChild(1).GetComponent<Text>().fontSize = 60;
                        return;
                    }
                    panellistExecution[i].transform.GetChild(1).GetComponent<Text>().rectTransform.sizeDelta =
                           new Vector2(640 / panellistExecution.Count,
                           panellistExecution[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
                    panellistExecution[i].transform.GetChild(1).GetComponent<Text>().fontSize = OuttoFontSize(panellistExecution.Count);
                }
                break;

            case 3:
                break;

            default:
                break;
        }
        
    }
    private int OuttoFontSize(int count) // 폰트 수치
    {
        switch (count)
        {
            case 1:
                return 60;
            case 2:
                return 50;
            case 3:
                return 40;
            case 4:
                return 35;
            case 5:
                return 30;
            case 6:
                return 26;
            case 7:
                return 24;
            case 8:
                return 22;
            case 9:
                return 20;
            default:
                return 20;
        }
    }

    //테스트용 함수
    private void SelectCount() //선택된 오브젝트 수
    {
        selectCount = wordSelect.Count;
    }


    //직접 사용할 함수
    private void CleanSelect() // 초기화
    {
        subjectWord = 0;
        conditionWord = 0;
        executionWord = 0;
        nowWord = 0;
        subjectText.text = " ";
        conditionText.text = " ";
        executionText.text = " ";
        wordSelect.Clear();
        OnOffScroll();
    }

    private void InputWordKey() // 키입력
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) || Input.GetKeyDown(keyCodes[i] - 208))
            {
                int numberPressed = i;
                switch(nowWord)
                {
                    case 0:
                        if (panellistSubject.Count < numberPressed) return;
                        break;
                    case 1:
                        if (panellistCondition.Count < numberPressed) return;
                        break;
                    case 2:
                        if (panellistExecution.Count < numberPressed) return;
                        break;
                    case 3:
                        return;
                    default:
                        break;
                }
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
                        OnOffScroll();
                        AllChangeTexts();
                        break;
                    case 1:
                        conditionWord = OutToNowWord(numberPressed, 1);
                        nowWord = 2;
                        OnOffScroll();
                        AllChangeTexts();
                        break;
                    case 2:
                        executionWord = OutToNowWord(numberPressed, 2);
                        nowWord = 3;
                        OnOffScroll();
                        AllChangeTexts();
                        break;
                    default:
                        nowWord = 3;
                        OnOffScroll();
                        AllChangeTexts();
                        break;
                }
            }
            else if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetMouseButtonDown(1))
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
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                        case 2:
                            nowWord = 1;
                            conditionWord = 0;
                            conditionText.text = " ";
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                        case 3:
                            nowWord = 2;
                            executionWord = 0;
                            executionText.text = " ";
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                        default:
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                    }
                }
            }
        }
    }

    public void ClickOnWordSelect(int num)
    {
        switch (nowWord)
        {
            case 0:
                subjectWord = OutToNowWord(num, 0);
                Debug.Log(subjectWord);
                nowWord = 1;
                OnOffScroll();
                AllChangeTexts();
                break;
            case 1:
                conditionWord = OutToNowWord(num, 1);
                nowWord = 2;
                OnOffScroll();
                AllChangeTexts();
                break;
            case 2:
                executionWord = OutToNowWord(num, 2);
                nowWord = 3;
                OnOffScroll();
                AllChangeTexts();
                break;
            default:
                nowWord = 3;
                OnOffScroll();
                AllChangeTexts();
                break;
        }
    }

    private void OnOffScroll()
    {
        switch(nowWord)
        {
            case 0:
                subjectScroll.SetActive(true);
                conditionScroll.SetActive(false);
                executionScroll.SetActive(false);
                break;
            case 1:
                subjectScroll.SetActive(false);
                conditionScroll.SetActive(true);
                executionScroll.SetActive(false);
                break;
            case 2:
                subjectScroll.SetActive(false);
                conditionScroll.SetActive(false);
                executionScroll.SetActive(true);
                break;
            case 3:
                subjectScroll.SetActive(false);
                conditionScroll.SetActive(false);
                executionScroll.SetActive(false);
                break;
            default:
                break;
        }
    }

    private int OutToNowWord(int i,int type)
    {
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
}
