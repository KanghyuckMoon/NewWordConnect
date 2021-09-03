using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
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

    readonly private List<string> subjectlist = new List<string>() { " ", "용사가", "모든 적이", "스테이지가", "카메라가", "날씨가", "온도가", "게임창이", "소리가", "특수" };
    readonly private List<string> conditionlist = new List<string>() { " ", "1초 마다", "가만히 있을 때", "충돌할 때", "블록을 밟을 때", "입력할 때", "떨어질 때", "카메라에 보일 때", "소리를 낼 때", "특수" };
    readonly private List<string> executionlist = new List<string>() { " ", "뛰어 오른다", "1초 동안 빨라진다", "1초 동안 정지한다", "1초 동안 느려진다", "떨어진다", "커진다", "작아진다", "충돌하지 않는다", "특수" };

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
    private int selectCount = 0;

    //UI
    [SerializeField]
    private Animator panelBar = null;
    [SerializeField]
    private Text subjectText = null;
    [SerializeField]
    private Text conditionText = null;
    [SerializeField]
    private Text executionText = null;

    [SerializeField]
    private Transform subjectScroll = null;
    [SerializeField]
    private Transform conditionScroll = null;
    [SerializeField]
    private Transform executionScroll = null;

    private List<GameObject> panellistSubject = new List<GameObject>();
    private List<GameObject> panellistCondition = new List<GameObject>();
    private List<GameObject> panellistExecution = new List<GameObject>();

    //캐싱용 멤버변수
    private Text tempSetSizeText = null;
    private Image tempSetSizeImage = null;
    private Button resetOnClick = null;
    private GameObject backPanel = null;
    private GameObject newPanel = null;
    private GameObject subjectScrollsPanel = null;
    private GameObject conditionScrollsPanel = null;
    private GameObject executionScrollsPanel = null;
    private int numberPressed = 0;

    //주어용 변수
    
    //조건어용 변수

    //실행어용 변수

    private void Start()
    {
        SetListUI(); //UI 갯수 세는 함수
        SetSizeListUI(); //UI 크기 맞추는 함수
        AllChangeTexts(); //텍스트 바꾸는 함수
        OnOffScroll(); //스크롤 껐다 키는 함수
        ResetOnClick(); // 버튼 기능들 리셋시키고 다시 부여

        //캐싱
        subjectScrollsPanel = subjectScroll.GetChild(0).gameObject;
        conditionScrollsPanel = conditionScroll.GetChild(0).gameObject;
        executionScrollsPanel = executionScroll.GetChild(0).gameObject;
    }

    private void Update()
    {
        InputWordKey();

        //테스트용
        SelectCount();
    }
    public void AllChangeTexts()
    {
        switch (nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    subjectScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        subjectlist[subjectUnlock[i + 1]];
                }
                break;
            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    conditionScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        conditionlist[conditionUnlock[i + 1]];
                }
                break;
            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    executionScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        executionlist[executionUnlock[i + 1]];
                }
                break;
        }

    }

    public void CreatePanel()
    {
        newPanel = null;
        switch (nowWord)
        {
            case 0:
                if (panellistSubject.Count >= 9) return;
                newPanel = CreatePanel_Pull(0);
                newPanel.SetActive(true);
                panellistSubject.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = subjectlist[subjectUnlock[panellistSubject.Count]];
                break;
            case 1:
                if (panellistCondition.Count >= 9) return;
                newPanel = CreatePanel_Pull(1);
                newPanel.SetActive(true);
                panellistCondition.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = conditionlist[conditionUnlock[panellistCondition.Count]];
                break;
            case 2:
                if (panellistExecution.Count >= 9) return;
                newPanel = CreatePanel_Pull(2);
                newPanel.SetActive(true);
                panellistCondition.Add(newPanel);
                newPanel.transform.GetChild(1).GetComponent<Text>().text = executionlist[executionUnlock[panellistExecution.Count]];
                break;
        }
        ResetOnClick();
        SetListUI();
        SetSizeListUI();
        AllChangeTexts();
    } // 완료

    private GameObject CreatePanel_Pull(int type)
    {
        switch(type)
        {
            case 0:
                if(subjectScroll.childCount == 0)
                {
                    return subjectScrollsPanel;
                }
                else if(subjectScroll.childCount > panellistSubject.Count)
                {
                    return subjectScroll.GetChild(panellistSubject.Count).gameObject;
                }
                return Instantiate(subjectScrollsPanel, subjectScrollsPanel.transform.parent);

            case 1:
                if (conditionScroll.childCount == 0)
                {
                    return conditionScrollsPanel;
                }
                else if (conditionScroll.childCount > panellistCondition.Count)
                {
                    return conditionScroll.GetChild(panellistCondition.Count).gameObject;
                }
                return Instantiate(conditionScrollsPanel, conditionScrollsPanel.transform.parent);

            case 2:
                if (executionScroll.childCount == 0)
                {
                    return executionScrollsPanel;
                }
                else if (executionScroll.childCount > panellistExecution.Count)
                {
                    return executionScroll.GetChild(panellistExecution.Count).gameObject;
                }
                return Instantiate(executionScrollsPanel, executionScrollsPanel.transform.parent);
        }
        return null;
    } //하위 함수


    public void ResetOnClick()
    {
        resetOnClick = null;
        for (int i = 0; i < subjectScroll.childCount; i++)
        {
            int temp = i;
            resetOnClick = subjectScroll.GetChild(temp).GetComponent<Button>();
            resetOnClick.onClick.RemoveAllListeners();
            resetOnClick.onClick.AddListener(() => { ClickOnWordSelect(temp + 1); });
        }
        for (int i = 0; i < conditionScroll.childCount; i++)
        {
            int temp = i;
            resetOnClick = conditionScroll.GetChild(temp).GetComponent<Button>();
            resetOnClick.onClick.RemoveAllListeners();
            resetOnClick.onClick.AddListener(() => { ClickOnWordSelect(temp + 1); });
        }
        for (int i = 0; i < executionScroll.childCount; i++)
        {
            int temp = i;
            resetOnClick = executionScroll.GetChild(temp).GetComponent<Button>();
            resetOnClick.onClick.RemoveAllListeners();
            resetOnClick.onClick.AddListener(() => { ClickOnWordSelect(temp + 1); });
        }
    } // 완료
    public void BackPanel() // 최적화 필요함 풀링 필요함
    {
        backPanel = null;
        switch (nowWord)
        {
            case 0:
                if (panellistSubject.Count == 0) return;

                backPanel = subjectScroll.GetChild(panellistSubject.Count - 1).gameObject;
                backPanel.SetActive(false);
                panellistSubject.RemoveAt(panellistSubject.Count - 1);
                break;
            case 1:
                if (panellistCondition.Count == 0) return;
                backPanel = conditionScroll.GetChild(panellistCondition.Count - 1).gameObject;
                backPanel.SetActive(false);
                panellistCondition.RemoveAt(panellistCondition.Count - 1);
                break;
            case 2:
                if (panellistExecution.Count == 0) return;

                backPanel = executionScroll.GetChild(panellistExecution.Count - 1).gameObject;
                backPanel.SetActive(false);
                panellistExecution.RemoveAt(panellistExecution.Count - 1);
                break;

        }
        SetListUI();
        SetSizeListUI();
    }
    public void ClearPanel()
    {
        int count = 0;
        switch (nowWord)
        {
            case 0:
                count = panellistSubject.Count;
                if (count == 1) return;
                for (int i = 1; i < count; i++)
                {
                    subjectScroll.GetChild(i).gameObject.SetActive(false);
                    panellistSubject.RemoveAt(panellistSubject.Count - 1);
                }
                break;
            case 1:
                count = panellistCondition.Count;
                if (count == 1) return;
                for (int i = 1; i < count; i++)
                {
                    conditionScroll.GetChild(i).gameObject.SetActive(false);
                    panellistCondition.RemoveAt(panellistCondition.Count - 1);
                }
                break;
            case 2:
                count = panellistExecution.Count;
                if (count == 1) return;
                for (int i = 1; i < count; i++)
                {
                    executionScroll.GetChild(i).gameObject.SetActive(false);
                    panellistExecution.RemoveAt(panellistExecution.Count - 1);
                }
                break;
        }

        SetSizeListUI();
    } // 최적화 필요함 풀링 필요함
    private void SetListUI()
    {
        panellistSubject.Clear();
        for (int i = 0; i < subjectScroll.childCount; i++)
        {
            if(subjectScroll.GetChild(i).gameObject.activeSelf)
            {
                panellistSubject.Add(subjectScroll.GetChild(i).gameObject);
            }
        };

        panellistCondition.Clear();
        for (int i = 0; i < conditionScroll.childCount; i++)
        {
            if(conditionScroll.GetChild(i).gameObject.activeSelf)
            {
                panellistCondition.Add(conditionScroll.GetChild(i).gameObject);
            }
        };

        panellistExecution.Clear();
        for (int i = 0; i < executionScroll.childCount; i++)
        {
            if(executionScroll.GetChild(i).gameObject.activeSelf)
            {
                panellistExecution.Add(executionScroll.GetChild(i).gameObject);
            }
        };
    }
    private void SetSizeListUI()
    {
        SetSizeListImage();
        SetSizeListText();
    }

    

    private void SetSizeListImage() // 패널의 이미지 조절 완료
    {
        tempSetSizeImage = null;
        switch (nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    tempSetSizeImage = panellistSubject[i].transform.GetChild(0).GetComponent<Image>();
                    if (panellistSubject.Count == 1)
                    {
                        tempSetSizeImage.rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistSubject.Count == 0) return;
                    tempSetSizeImage.rectTransform.sizeDelta = SetSizeListImage_OutVector(panellistSubject.Count);
                }
                break;

            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    tempSetSizeImage = panellistCondition[i].transform.GetChild(0).GetComponent<Image>();
                    if (panellistCondition.Count == 1)
                    {
                        tempSetSizeImage.rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistCondition.Count == 0) return;
                    tempSetSizeImage.rectTransform.sizeDelta = SetSizeListImage_OutVector(panellistCondition.Count);
                }
                break;

            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    tempSetSizeImage = panellistExecution[i].transform.GetChild(0).GetComponent<Image>();
                    if (panellistExecution.Count == 1)
                    {
                        tempSetSizeImage.rectTransform.sizeDelta = new Vector2(100, 100);
                        return;
                    }
                    if (panellistExecution.Count == 0) return;
                    tempSetSizeImage.rectTransform.sizeDelta = SetSizeListImage_OutVector(panellistExecution.Count);
                }
                break;
        }

    }
    private Vector2 SetSizeListImage_OutVector(int count)
    {
        return new Vector2(100 / (count * 0.5f), 100 / (count * 0.5f));
    } //하위 함수

    private void SetSizeListText() //패널의 텍스트 사이즈 조절 최적화 필요
    {
        tempSetSizeText = null;
        switch (nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    tempSetSizeText = panellistSubject[i].transform.GetChild(1).GetComponent<Text>();
                    if (panellistSubject.Count == 1)
                    {
                        tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,0);
                        panellistSubject[i].transform.GetChild(1).GetComponent<Text>().fontSize = 60;
                        return;
                    }
                    tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,0);
                    tempSetSizeText.fontSize = SetSizeListText_OutFontSize(panellistSubject.Count);
                }
                break;

            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    tempSetSizeText = panellistCondition[i].transform.GetChild(1).GetComponent<Text>();
                    if (panellistCondition.Count == 1)
                    {
                        tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,1);
                        tempSetSizeText.fontSize = 60;
                        return;
                    }
                    tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,1);
                    tempSetSizeText.fontSize = SetSizeListText_OutFontSize(panellistCondition.Count);
                }
                break;

            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    tempSetSizeText = panellistExecution[i].transform.GetChild(1).GetComponent<Text>();
                    if (panellistExecution.Count == 1)
                    {
                        tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,2);
                        tempSetSizeText.fontSize = 60;
                        return;
                    }
                    tempSetSizeText.rectTransform.sizeDelta = SetSizeListText_OutVector(i,2);
                    tempSetSizeText.fontSize = SetSizeListText_OutFontSize(panellistExecution.Count);
                }
                break;
        }

    }
    private Vector2 SetSizeListText_OutVector(int i, int type)
    {
        switch(type)
        {
            case 0:
                return new Vector2(640 / panellistSubject.Count, panellistSubject[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);

            case 1:
                return new Vector2(640 / panellistCondition.Count, panellistCondition[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);

            case 2:
                return new Vector2(640 / panellistExecution.Count, panellistExecution[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rect.height);
            default:
                return new Vector2(0,0);
        }
        
    } // 하위 함수
    private int SetSizeListText_OutFontSize(int count) // 하위 함수 폰트 수치
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

    private void CleanSelect() // 초기화
    {
        subjectWord = 0;
        conditionWord = 0;
        executionWord = 0;
        nowWord = 0;
        subjectText.text = null;
        conditionText.text = null;
        executionText.text = null;
        wordSelect.Clear();
        OnOffScroll();
    }

    private void InputWordKey() // 키입력 최적화 필요
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) || Input.GetKeyDown(keyCodes[i] - 208))
            {
                numberPressed = i;
                switch (nowWord)
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
                    default:
                        return;
                }
                if (numberPressed == 0)
                {
                    CleanSelect();
                    return;
                }
                switch (nowWord)
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
                        panelBar.SetBool("PanelOn",false);
                        break;
                    default:
                        nowWord = 3;
                        OnOffScroll();
                        AllChangeTexts();
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetMouseButtonDown(1))
            {
                if (nowWord <= 0)
                {
                    nowWord = 0;
                    return;
                }
                else
                {
                    switch (nowWord)
                    {
                        case 1:
                            nowWord = 0;
                            subjectWord = 0;
                            subjectText.text = null;
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                        case 2:
                            nowWord = 1;
                            conditionWord = 0;
                            conditionText.text = null;
                            OnOffScroll();
                            AllChangeTexts();
                            return;
                        case 3:
                            nowWord = 2;
                            executionWord = 0;
                            executionText.text = null;
                            OnOffScroll();
                            AllChangeTexts();
                            panelBar.SetBool("PanelOn", true);
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
                panelBar.SetBool("PanelOn", false);
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
        switch (nowWord)
        {
            case 0:
                subjectScroll.gameObject.SetActive(true);
                conditionScroll.gameObject.SetActive(false);
                executionScroll.gameObject.SetActive(false);
                break;
            case 1:
                subjectScroll.gameObject.SetActive(false);
                conditionScroll.gameObject.SetActive(true);
                executionScroll.gameObject.SetActive(false);
                break;
            case 2:
                subjectScroll.gameObject.SetActive(false);
                conditionScroll.gameObject.SetActive(false);
                executionScroll.gameObject.SetActive(true);
                break;
            case 3:
                subjectScroll.gameObject.SetActive(false);
                conditionScroll.gameObject.SetActive(false);
                executionScroll.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private int OutToNowWord(int i, int type)
    {
        switch (type)
        {
            case 0://주어
                subjectText.text = subjectlist[subjectUnlock[i]];
                return subjectUnlock[i];
            case 1://조건어
                conditionText.text = conditionlist[conditionUnlock[i]];
                return conditionUnlock[i];
            case 2://실행어
                executionText.text = executionlist[executionUnlock[i]];
                return executionUnlock[i];
        }
        return 0;
    }




    //테스트용 함수
    private void SelectCount() //선택된 오브젝트 수
    {
        selectCount = wordSelect.Count;
    }
}
