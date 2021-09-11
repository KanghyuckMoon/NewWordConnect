﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

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

    readonly private List<string> subjectlist = new List<string>() { " ", "용사가", "모든 적이", "스테이지가", "카메라가", "날씨가", "온도가", "게임창이", "시간이", "특수" };
    readonly private List<string> conditionlist = new List<string>() { " ", "1초 마다", "가만히 있을 때", "충돌할 때", "블록을 밟을 때", "입력할 때", "떨어질 때", "카메라에 보일 때", "소리를 낼 때", "특수" };
    readonly private List<string> executionlist = new List<string>() { " ", "뛰어 오른다", "1초 동안 빨라진다", "1초 동안 정지한다", "1초 동안 느려진다", "떨어진다", "커진다", "작아진다", "1초 동안 충돌하지 않는다", "특수" };

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
    private List<WordGameObject> wordSelect = new List<WordGameObject>(); // 선택되는 오브젝트
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

    [SerializeField]
    private GameObject barSubject = null;
    [SerializeField]
    private GameObject barCondition = null;
    [SerializeField]
    private GameObject barExecution = null;

    [SerializeField]
    private Canvas canvas;

    //쿨타임
    [SerializeField]
    private Image cooltimeImage = null;
    private Animator cooltimeAnimator = null;
    [SerializeField]
    private float cooltime = 0f;
    private bool coolOn = false;
    private bool wordSetOn = false;
    [SerializeField]
    private float cooltimeSpeed = 1f;

    //온도
    [SerializeField]
    private Image temperImage = null;

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
    private WordGameObject s_player;
    private EnemyManager s_enemys;
    private StageManager s_stage;
    private Camera s_mainCamera;
    public TimeManager s_timeManager;

    //스테이지에서 가져오는 변수
    private TemperentManager s_temperentManager;
    private WeatherManager s_weatherManager;
    //소리
    private float s_Sound;
    //게임창
    private DisplayManager s_displayManager;
    //특수

    //조건어용 변수
    [SerializeField]
    private float c_onesecondCoolTime = 0f;
    private float c_StandTime = 0f;

    //실행어용 변수

    private void Awake()
    {
        //주어 찾기
        FindSubjects();
    }

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
        cooltimeAnimator = cooltimeImage.GetComponent<Animator>();
    }

    private void FindSubjects()
    {
        s_player = FindObjectOfType<PlayerMove>(); //안됨
        s_enemys = FindObjectOfType<EnemyManager>(); // 안됨
        s_stage = FindObjectOfType<StageManager>(); // 안됨
        s_mainCamera = FindObjectOfType<Camera>(); // 안됨
        s_temperentManager = FindObjectOfType<TemperentManager>(); // 안ㄴ됨
        s_weatherManager = FindObjectOfType<WeatherManager>(); // 안됨ㄴ

        s_timeManager = GetComponent<TimeManager>(); // 됨
        s_displayManager = GetComponent<DisplayManager>(); // 됨
        canvas.worldCamera = s_mainCamera;

        s_temperentManager.SetPlayer(s_player);
        s_timeManager.SetPlayer(s_player);
        s_weatherManager.SetPlayer(s_player);
        s_displayManager.SetPlayer(s_player);
    }

    private void Update()
    {
        InputWordKey();
        Cooldown();
        Temperature();
        TimeRewind();
        if (wordSetOn)
        {
            ConditionWordObject();
        }
    }

    private void Cooldown()
    {
        if (!coolOn) return; 
        if (nowWord != 3) return;
        cooltimeAnimator.SetBool("CoolTimeOn", true);
        if (cooltime <= 1)
        {
            cooltime += Time.deltaTime * cooltimeSpeed;
            cooltimeImage.fillAmount = cooltime;
        }
        else
        {
            coolOn = false;
            if (cooltimeImage.color.a > 0)
            cooltimeAnimator.SetBool("CoolTimeOn", false);
            wordSetOn = true;
        }
    }
    private void CoolTimefalse()
    {
        coolOn = false;
        wordSetOn = false;
        cooltimeAnimator.SetBool("CoolTimeOn", false);
        cooltimeAnimator.Play("CoolTimeOff");
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
    public void BackPanel() // 
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
    } // 
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

    private void SetSizeListText() //패널의 텍스트 사이즈 조절
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

    private void InputWordKey() // 키입력
    {
        if (Time.timeScale == 0) return;
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
                        InputSubject(numberPressed);
                        break;
                    case 1:
                        InputCondition(numberPressed);
                        break;
                    case 2:
                        InputExecution(numberPressed);
                        AllReset();
                        SelectWordObject();
                        break;
                    default:
                        InputDefault();
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
                            BackAnimationSubject();
                            return;
                        case 2:
                            nowWord = 1;
                            conditionWord = 0;
                            conditionText.text = null;
                            OnOffScroll();
                            AllChangeTexts();
                            BackAnimationCondition();
                            return;
                        case 3:
                            nowWord = 2;
                            executionWord = 0;
                            executionText.text = null;
                            OnOffScroll();
                            AllChangeTexts();
                            panelBar.SetBool("PanelOn", true);
                            BackAnimationExecution();
                            CoolTimefalse();
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
                InputSubject(num);
                break;
            case 1:
                InputCondition(num);
                break;
            case 2:
                InputExecution(num);
                break;
            default:
                InputDefault();
                break;
        }
    }

    private void InputSubject(int num)
    {
        subjectWord = OutToNowWord(num, 0);
        nowWord = 1;
        OnOffScroll();
        AllChangeTexts();
        AnimationSubject();
    }

    private void InputCondition(int num)
    {
        conditionWord = OutToNowWord(num, 1);
        nowWord = 2;
        OnOffScroll();
        AllChangeTexts();
        AnimationCondition();
    }

    private void InputExecution(int num)
    {
        executionWord = OutToNowWord(num, 2);
        nowWord = 3;
        OnOffScroll();
        AllChangeTexts();
        panelBar.SetBool("PanelOn", false);
        AnimationExecution();
        SelectWordObject();
        AllReset();
        cooltime = 0f;
        coolOn = true;
    }

    private void InputDefault()
    {
        nowWord = 3;
        OnOffScroll();
        AllChangeTexts();
    }

    private void AnimationSubject()
    {
        barSubject.GetComponent<Animator>().SetBool("PanelOn",true);
    }
    private void AnimationCondition()
    {
        barCondition.GetComponent<Animator>().SetBool("PanelOn", true);
        barSubject.transform.GetChild(1).GetComponent<Animator>().SetBool("ChainOn", true);
        //사슬 움직임
    }
    private void AnimationExecution()
    {
        barExecution.GetComponent<Animator>().SetBool("PanelOn", true);
        barCondition.transform.GetChild(1).GetComponent<Animator>().SetBool("ChainOn", true);
        //사슬 움직임
    }

    private void BackAnimationSubject()
    {
        barSubject.GetComponent<Animator>().SetBool("PanelOn", false);
    }
    private void BackAnimationCondition()
    {
        barCondition.GetComponent<Animator>().SetBool("PanelOn", false);
        barSubject.transform.GetChild(1).GetComponent<Animator>().SetBool("ChainOn", false);
        //사슬 움직임
    }
    private void BackAnimationExecution()
    {
        barExecution.GetComponent<Animator>().SetBool("PanelOn", false);
        barCondition.transform.GetChild(1).GetComponent<Animator>().SetBool("ChainOn", false);
        //사슬 움직임
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

    //리셋함수
    private void AllReset()
    {
        c_onesecondCoolTime = 0;
        s_timeManager.ResetPositions();
    }

    private void Temperature()
    {
        temperImage.rectTransform.eulerAngles = new Vector3(0,0, (s_temperentManager.s_Temperature * 2.4f) - 120);
        if (s_temperentManager.s_Temperature / 100 <= 0)
        {
            s_temperentManager.tempdan = -1;
        }
        else if(s_temperentManager.s_Temperature / 100 >= 1)
        {
            s_temperentManager.tempdan = 1;
        }
        else 
        {
            s_temperentManager.tempdan = 0;
        }
    }

    private void TimeRewind()
    {
        if(s_timeManager.isStartRecord)
        {
            if (s_timeManager.isRewinding)
            {
                s_player.SpeedStopnotinvoke();
            }
            else
            {
                s_player.TimeReset();
            }
        }
        
    }

    //단어의 힘 --------------------------------------------------------------

    //선택함수
    private void SelectWordObject()
    {
        wordSelect.Clear();
        switch(subjectWord)
        {
            case 0: // 없음 개발 완료
                break;
            case 1: // 용사가 개발 완료
                wordSelect.Add(s_player);
                break;
            case 2:  // 모든적이 개발완료
                for(int i = 0; i < s_enemys.transform.childCount;i++)
                {
                    wordSelect.Add(s_enemys.transform.GetChild(i).GetComponent<WordGameObject>());
                }
                break;
            case 3: // 스테이지가 개발완료
                for (int i = 0; i < s_stage.transform.childCount; i++)
                {
                    if(s_stage.transform.GetChild(i).GetComponent<WordGameObject>() != null)
                    {
                        wordSelect.Add(s_stage.transform.GetChild(i).GetComponent<WordGameObject>());
                    }
                }
                break;
            case 4: // 카메라가 개발완료
                wordSelect.Add(s_mainCamera.GetComponent<WordGameObject>());
                break;
            case 5: // 날씨가 개발완료
                wordSelect.Add(s_weatherManager);
                break;
            case 6: // 온도가 개발완료
                wordSelect.Add(s_temperentManager);
                break;
            case 7: // 게임창이
                wordSelect.Add(s_displayManager);
                break;
            case 8: // 시간이 개발완료
                wordSelect.Add(s_timeManager);
                break;
            case 9: // 여기서부터 특수
                break;
        }
    }
    //조건 함수
    private void ConditionWordObject()
    {
        //충돌체크 변수 끄기
        switch(conditionWord)
        {
            case 0: // 없음
                break;
            case 1: // 1초마다 개발완료
                Condition_OneSencond();
                break;
            case 2: // 가만히 있을 때 개발완료
                Condition_Stand();
                break;
            case 3: // 충돌할 때 개발완료
                Condition_Collider();
                break;
            case 4: // 블록을 밟을 때 개발완료
                Condition_Block();
                break;
            case 5: // 입력할 때 개발안료
                Condition_Input();
                break;
            case 6: // 떨어질 때 개발완료
                Condition_Fall();
                break;
            case 7: // 카메라안에 들어 올때 개발완료
                Condition_InCamera();
                break;
            case 8: // 소리를 낼 때 
                break;
            case 9: // 여기서부터 특수
                break;
        }
    }
    private void Condition_OneSencond() // 1번 1초마다
    {
        if(c_onesecondCoolTime < 1)
        {
            c_onesecondCoolTime += Time.deltaTime;
            return;
        }
        c_onesecondCoolTime = 0;
        ExecutionWordObject();
    } 

    private void Condition_Stand() // 2번 가만히 있을 때
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if (!wordSelect[i].W_MoveOn)
            {
                if(wordSelect[i].W_MoveOnEffect)
                {
                    ExecutionWordObject(i);
                }
            }
        }
    }

    private void Condition_Collider() // 3번 충돌할 때
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if (wordSelect[i].W_Collider)
            {
                if (!wordSelect[i].W_ColliderEffect)
                {
                    ExecutionWordObject(i);
                }
            }
        }
    }

    private void Condition_Block() // 4번 블록을 밟을 때
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if (wordSelect[i].W_Tile > 1)
            {
                if(wordSelect[i].W_BlockOn)
                {
                    wordSelect[i].SetMoveZero();
                    ExecutionWordObject(i);
                }
            }
        }
    }

    private void Condition_Input() // 5번 입력할 때
    {
        if(Input.anyKeyDown)
        {
            ExecutionWordObject();
        }
    }

    private void Condition_Fall() // 6번 떨어질 때
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if(wordSelect[i].ReturnVelocityY() < -3f)
            {
                ExecutionWordObject(i);
            }
        }
    }

    private void Condition_InCamera() // 7번 카메라 안에 들어올때
    {
        for(int i = 0; i < wordSelect.Count; i++)
        {
            if(wordSelect[i].W_Visible)
            {
                if(!wordSelect[i].W_VisibleEffect)
                {
                    wordSelect[i].W_VisibleEffectOntrue();
                    ExecutionWordObject(i);
                }
            }
        }
    }
    //실행 함수
    private void ExecutionWordObject()
    {
        switch (executionWord)
        {
            case 0: // 없음
                break;
            case 1: // 뛰어오른다 개발완료
                Execution_Jump();
                break;
            case 2: // 1초동안 빨라진다 개발완료
                Execution_SpeedUp();
                c_onesecondCoolTime = -1f;
                break;
            case 3: // 1초동안 정지한다 개발완료
                Execution_TimeStop();
                c_onesecondCoolTime = -1;
                break;
            case 4: // 1초동안 느려진다 개발완료
                Execution_SpeedDown();
                c_onesecondCoolTime = -1f;
                break;
            case 5: // 떨어어진다 개발완료
                Execution_Down();
                break;
            case 6: // 커진다 개발완료
                Execution_SizeUp();
                break;
            case 7: // 작아진다 개발완료
                Execution_SizeDown();
                break;
            case 8: // 1초동안 충돌하지 않는다 개발완료
                Execution_ColliderOff();
                c_onesecondCoolTime = -1f;
                break;
            case 9: // 여기서부터 특수
                break;
        }
    }
    private void ExecutionWordObject(int i)
    {
        switch (executionWord)
        {
            case 0: // 없음
                break;
            case 1: // 뛰어오른다 개발완료
                wordSelect[i].SetCollider();
                Execution_Jump(i);
                break;
            case 2:// 1초 동안 빨라진다 개발완료
                Execution_SpeedUp(i);
                c_onesecondCoolTime = -1;
                break;
            case 3: // 1초 동안 정지한다 개발완료
                Execution_TimeStop(i);
                c_onesecondCoolTime = -1;
                break;
            case 4: // 1초 동안 느려진다 개발완료
                Execution_SpeedDown(i);
                c_onesecondCoolTime = -1;
                break;
            case 5: // 내려간다 개발완료
                Execution_Down(i);
                break;
            case 6: // 커진다 개발완료
                Execution_SizeUp(i);
                break;
            case 7: // 작아진다 개발완료
                Execution_SizeDown(i);
                break;
            case 8: //1초 동안 충돌하지 않는다 개발완료
                Execution_ColliderOff(i);
                c_onesecondCoolTime = -1;
                break;
            case 9: // 여기서부터 특수
                break;
        }
    }


    private void Execution_Jump() // 1번 뛰어오른다
    {
        for(int i = 0; i<wordSelect.Count;i++)
        {
            wordSelect[i].Jump();
        }
    }
    private void Execution_Jump(int i)
    {
            wordSelect[i].Jump();
    }
    private void Execution_SpeedUp() // 2번 스피드
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].SpeedUp();
        }
    }
    private void Execution_SpeedUp(int i) // 2번 스피드
    {
        
            wordSelect[i].SpeedUp();
        
    }
    private void Execution_TimeStop() // 3번 시간 정지
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].SpeedStop();
        }
    }
    private void Execution_TimeStop(int i) // 
    {
       wordSelect[i].SpeedStop();
    }
    private void Execution_SpeedDown() // 4번 스피드 떨어짐
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].SpeedDown();
        }
    }
    private void Execution_SpeedDown(int i) // 4번 스피드 떨어짐
    {
            wordSelect[i].SpeedDown();
    }
    private void Execution_Down() // 5번 내려간다
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if(conditionWord == 6)
            {
                wordSelect[i].SuperDown();
            }
            else
            {
                wordSelect[i].Down();
            }
            if(subjectWord == 8)
            {
                c_onesecondCoolTime = -2;
            }
        }
    }
    private void Execution_Down(int i)
    {
        if (conditionWord == 6)
        {
            wordSelect[i].SuperDown();
        }
        else
        {
            wordSelect[i].Down();
        }
        if (subjectWord == 8)
        {
            c_onesecondCoolTime = -2;
        }
    }

    private void Execution_SizeUp()
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].SizeUp();
        }
    }
    private void Execution_SizeUp(int i)
    {
        wordSelect[i].SizeUp();
    }
    private void Execution_SizeDown()
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].SizeDown();
        }
    }
    private void Execution_SizeDown(int i)
    {
        wordSelect[i].SizeUp();
    }

    private void Execution_ColliderOff()
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].ColliderOff();
        }
    } // 8번 1초동안 충돌하지 않는다
    private void Execution_ColliderOff(int i)
    {
        wordSelect[i].ColliderOff();
    }
}