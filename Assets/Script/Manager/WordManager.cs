﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

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
    private RectTransform barSubject = null;
    [SerializeField]
    private RectTransform barCondition = null;
    [SerializeField]
    private RectTransform barExecution = null;

    [SerializeField]
    private Canvas canvas;

    //쿨타임
    [SerializeField]
    private Image cooltimeImage = null;
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
    private GameObject backPanel = null;
    private GameObject subjectScrollsPanel = null;
    private GameObject conditionScrollsPanel = null;
    private GameObject executionScrollsPanel = null;
    private int numberPressed = 0;

    //시스템들
    [SerializeField]
    private GameObject WordSystem = null;
    [SerializeField]
    private GameObject TextSystem = null;
    [SerializeField]
    private RectTransform[] textsystemRect = new RectTransform[3];
    //
    [SerializeField]
    private GameObject escSystem;
    [SerializeField]
    private Sprite[] numSprites;

    //주어용 변수
    private PlayerMove s_player;
    private EnemyManager s_enemys;
    private StageManager s_stage;
    private Camera s_mainCamera;
    public TimeManager s_timeManager;

    //스테이지에서 가져오는 변수
    private TemperentManager s_temperentManager;
    private WeatherManager s_weatherManager;
    //게임창
    private DisplayManager s_displayManager;
    //특수

    //조건어용 변수
    [SerializeField]
    private float c_onesecondCoolTime = 0f;

    //실행어용 변수

    //이벤트 변수
    public bool isEvent = false;
    public bool isGameStart = false;
    public bool isInputESC = false;
    private bool isBreak = false;

    //애니메이션 변수
    [SerializeField]
    private RectTransform dieAnimation;
    [SerializeField]
    private RectTransform[] chain;
    private float[] chainsXpos = new float[2];

    //키세팅 변수
    private bool wasd;
    private bool keypad;
    private KeySetting keysetting;
    private SaveUser saveUser;

    //윈 게임
    [SerializeField]
    private GameObject wingame;
    [SerializeField]
    private RectTransform winbackground;
    [SerializeField]
    private RectTransform wintext;

    //쉐이더
    [SerializeField]
    private List<Material> materials;

    [SerializeField]
    private OptionPanel optionPanel;

    private void Awake()
    {
        //주어 찾기
        FindSubjects();
        keysetting = SaveManager.Instance.CurrenKeySetting;
        saveUser = SaveManager.Instance.CurrentSaveUser;
    }

    private void Start()
    {
        SetListUI(); //UI 갯수 세는 함수
        SetSizeListUI(); //UI 크기 맞추는 함수
        AllChangeTexts(); //텍스트 바꾸는 함수
        OnOffScroll(); //스크롤 껐다 키는 함수

        //캐싱
        subjectScrollsPanel = subjectScroll.GetChild(0).gameObject;
        conditionScrollsPanel = conditionScroll.GetChild(0).gameObject;
        executionScrollsPanel = executionScroll.GetChild(0).gameObject;


        subjectUnlock = saveUser.subjectGet;
        conditionUnlock = saveUser.conditionGet;
        executionUnlock = saveUser.executionGet;
        SortList(subjectUnlock);
        SortList(conditionUnlock);
        SortList(executionUnlock);
        ReCreateWordPanel(subjectUnlock, 0);
        ReCreateWordPanel(conditionUnlock, 1);
        ReCreateWordPanel(executionUnlock, 2);

        chainsXpos[0] = chain[0].anchoredPosition.x;
        chainsXpos[1] = chain[1].anchoredPosition.x;
        chain[0].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        chain[1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        cooltimeImage.color = new Color(1, 1, 1, 0);
        CleanSelectChainBreak();
    }

    private void FindSubjects()
    {
        s_player = FindObjectOfType<PlayerMove>();
        s_enemys = FindObjectOfType<EnemyManager>(); 
        s_stage = FindObjectOfType<StageManager>(); 
        s_mainCamera = FindObjectOfType<Camera>(); 
        s_temperentManager = FindObjectOfType<TemperentManager>(); 
        s_weatherManager = FindObjectOfType<WeatherManager>(); 

        s_timeManager = GetComponent<TimeManager>(); 
        s_displayManager = GetComponent<DisplayManager>(); 
        canvas.worldCamera = s_mainCamera;

        s_temperentManager.SetPlayer(s_player);
        s_timeManager.SetPlayer(s_player);
        s_weatherManager.SetPlayer(s_player);
        s_displayManager.SetPlayer(s_player);
    }

    public void SetKeySetting()
    {
        wasd = SaveManager.Instance.CurrenKeySetting.Wasd;
        keypad = SaveManager.Instance.CurrenKeySetting.Numpad;
    }

    private void Update()
    {
        if (isEvent) return;
        if (!isGameStart) return;
        if (isBreak) return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isInputESC = !isInputESC;
            s_player.isStop = isInputESC;
            escSystem.SetActive(isInputESC);
            optionPanel.isEnabled = isInputESC;
        }
        if (isInputESC) return;
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
        cooltimeImage.DOColor(new Color(1, 1, 1, 1), 1f);
        if (cooltime <= 1)
        {
            cooltime += Time.deltaTime * cooltimeSpeed;
            cooltimeImage.fillAmount = cooltime;
        }
        else
        {
            coolOn = false;
            if (cooltimeImage.color.a > 0)
                cooltimeImage.DOColor(new Color(1, 1, 1, 0), 1f);
            wordSetOn = true;
        }
    }
    private void CoolTimefalse()
    {
        coolOn = false;
        wordSetOn = false;
        cooltimeImage.DOColor(new Color(1, 1, 1, 0), 1);
    }

    public void AllChangeTexts()
    {
        switch (nowWord)
        {
            case 0:
                for (int i = 0; i < panellistSubject.Count; i++)
                {
                    if (subjectUnlock[i + 1] == -1) continue;
                    subjectScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        subjectlist[subjectUnlock[i + 1]];
                }
                break;
            case 1:
                for (int i = 0; i < panellistCondition.Count; i++)
                {
                    if (conditionUnlock[i + 1] == -1) continue;
                    conditionScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        conditionlist[conditionUnlock[i + 1]];
                }
                break;
            case 2:
                for (int i = 0; i < panellistExecution.Count; i++)
                {
                    if (executionUnlock[i + 1] == -1) continue;
                    executionScroll.GetChild(i).transform.GetChild(1).GetComponent<Text>().text =
                        executionlist[executionUnlock[i + 1]];
                }
                break;
        }
    }

    public void CreatePanel()
    {
        GameObject newPanel = null;
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
        SetListUI();
        SetSizeListUI();
        AllChangeTexts();
    } // 완료

    public void AddSubjectWord(int index)
    {
        GameObject newPanel = null;
        newPanel = CreatePanel_Pull(0);
        newPanel.SetActive(true);
        panellistSubject.Add(newPanel);
        for(int i = 0; i < subjectUnlock.Count; i++)
        {
            if(subjectUnlock[i] == -1)
            {
                subjectUnlock[i] = index;
                SortList(subjectUnlock);
                SetSizeListUI();
                return;
            }
        }
        newPanel.transform.GetChild(1).GetComponent<Text>().text = subjectlist[subjectUnlock[index]];
        SortList(subjectUnlock);
        SetSizeListUI();
    }
    public void ReCreateWordPanel(List<int> list, int type)
    {
        GameObject newPanel = null;
        for (int i = 2; i < list.Count; i++)
        {
            if(list[i] == -1)
            {
                continue;   
            }
            else
            {
                newPanel = CreatePanel_Pull(type);
                newPanel.SetActive(true);
                switch (type)
                {
                    case 0:
                        panellistSubject.Add(newPanel);
                        break;
                    case 1:
                        panellistCondition.Add(newPanel);
                        break;
                    case 2:
                        panellistExecution.Add(newPanel);
                        break;
                }
            }
        }
        AllChangeTexts();
    }


    public void AddConditionWord(int index)
    {
        GameObject newPanel = null;
        newPanel = CreatePanel_Pull(1);
        newPanel.SetActive(true);
        panellistCondition.Add(newPanel);
        for (int i = 0; i < conditionUnlock.Count; i++)
        {
            if (conditionUnlock[i] == -1)
            {
                conditionUnlock[i] = index;
                SortList(conditionUnlock);
                SetSizeListUI();
                return;
            }
        }
        newPanel.transform.GetChild(1).GetComponent<Text>().text = conditionlist[conditionUnlock[index]];
        SortList(conditionUnlock);
        SetSizeListUI();
    }

    public void AddExecutionWord(int index)
    {
        GameObject newPanel = null;
        newPanel = CreatePanel_Pull(2);
        newPanel.SetActive(true);
        panellistExecution.Add(newPanel);
        for (int i = 0; i < executionUnlock.Count; i++)
        {
            if (executionUnlock[i] == -1)
            {
                executionUnlock[i] = index;
                SortList(executionUnlock);
                SetSizeListUI();
                return;
            }
        }
        newPanel.transform.GetChild(1).GetComponent<Text>().text = executionlist[executionUnlock[index]];
        SortList(executionUnlock);
        SetSizeListUI();
    }

    public void SortList(List<int> list)
    {
        int temp = 0;
        for(int i = 1; i < list.Count; i++)
        {
            if (list[i] == -1) continue;
            for(int j = 0; j < i; j++)
            {
                if(list[j] == -1)
                {
                    temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
                if(list[i] < list[j])
                {
                    temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
        AllChangeTexts();
        SaveManager.Instance.SaveToJson();
    }

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
    }

    public void BackPanel()
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
    }

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
        SetSizeListText();
        SetNumListImage();
    }

    private void SetNumListImage()
    {
        for (int i = 0; i < panellistSubject.Count; i++)
        {
            tempSetSizeImage = panellistSubject[i].transform.GetChild(0).GetComponent<Image>();
            tempSetSizeImage.sprite = numSprites[i];
            tempSetSizeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(ReturnImageSizeDelta(panellistSubject.Count), ReturnImageSizeDelta(panellistSubject.Count));
        }
        for (int i = 0; i < panellistCondition.Count; i++)
        {
            tempSetSizeImage = panellistCondition[i].transform.GetChild(0).GetComponent<Image>();
            tempSetSizeImage.sprite = numSprites[i];
            tempSetSizeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(ReturnImageSizeDelta(panellistCondition .Count), ReturnImageSizeDelta(panellistSubject.Count));
        }
        for (int i = 0; i < panellistExecution.Count; i++)
        {
            tempSetSizeImage = panellistExecution[i].transform.GetChild(0).GetComponent<Image>();
            tempSetSizeImage.sprite = numSprites[i];
            tempSetSizeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(ReturnImageSizeDelta(panellistExecution.Count), ReturnImageSizeDelta(panellistSubject.Count));
            Debug.Log(tempSetSizeImage);
        }
    }
    private void SetSizeListText() //패널의 텍스트 사이즈 조절
    {
        tempSetSizeText = null;
        for (int i = 0; i < panellistSubject.Count; i++)
        {
            tempSetSizeText = panellistSubject[i].transform.GetChild(1).GetComponent<Text>();
            if (panellistSubject.Count == 1)
            {
                tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistSubject.Count) - ReturnImageSizeDelta(panellistSubject.Count), 40);
                tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
                break;
            }
            tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistSubject.Count) - ReturnImageSizeDelta(panellistSubject.Count), 40);
            tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
        }
        for (int i = 0; i < panellistCondition.Count; i++)
        {
            tempSetSizeText = panellistCondition[i].transform.GetChild(1).GetComponent<Text>();
            if (panellistCondition.Count == 1)
            {
                tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistCondition.Count) - ReturnImageSizeDelta(panellistCondition.Count), 40);
                tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
                break;
            }
            tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistCondition.Count) - ReturnImageSizeDelta(panellistCondition.Count), 40);
            tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
        }
        for (int i = 0; i < panellistExecution.Count; i++)
        {
            tempSetSizeText = panellistExecution[i].transform.GetChild(1).GetComponent<Text>();
            if (panellistExecution.Count == 1)
            {
                tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistExecution.Count) - ReturnImageSizeDelta(panellistExecution.Count), 40);
                tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
                break;
            }
            tempSetSizeText.rectTransform.sizeDelta = new Vector2(ReturnSizeDelta(panellistExecution.Count) - ReturnImageSizeDelta(panellistExecution.Count), 40);
            tempSetSizeText.rectTransform.anchoredPosition = new Vector2(ReturnImageSizeDelta(panellistSubject.Count) * 0.5f, -15);
        }
    }

    private float ReturnSizeDelta(int count)
    {
        switch (count)
        {
            case 1:
                return 620;
            case 2:
                return 310;
            case 3:
                return 206;
            case 4:
                return 155;
            case 5:
                return 124;
            case 6:
                return 103;
            case 7:
                return 88;
            case 8:
                return 77;
            case 9:
                return 68;
        }
        return 0;
    }

    private float ReturnImageSizeDelta(int count)
    {
        switch (count)
        {
            case 1:
                return 40;
            case 2:
                return 40;
            case 3:
                return 35;
            case 4:
                return 30;
            case 5:
                return 28;
            case 6:
                return 25;
            case 7:
                return 23;
            case 8:
                return 21;
            case 9:
                return 19;
        }
        return 0;
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
        subjectText.color = new Color(0, 0, 0, 1);
        conditionText.color = new Color(0, 0, 0, 1);
        executionText.color = new Color(0, 0, 0, 1);
        barSubject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barCondition.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barExecution.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        wordSelect.Clear();
        OnOffScroll();
    }
    private void CleanSelectChainBreak() // 틀린 조합
    {
        isBreak = true;
        subjectWord = 0;
        conditionWord = 0;
        executionWord = 0;
        nowWord = 0;
        subjectText.text = null;
        conditionText.text = null;
        executionText.text = null;
        subjectText.color = new Color(0, 0, 0, 1);
        conditionText.color = new Color(0, 0, 0, 1);
        executionText.color = new Color(0, 0, 0, 1);
        barSubject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barCondition.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barExecution.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barSubject.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
        barCondition.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
        chain[0].DOAnchorPosX(chainsXpos[0], 0.2f);
        chain[0].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.2f);
        chain[1].DOAnchorPosX(chainsXpos[1], 0.2f);
        chain[1].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.2f);
        wordSelect.Clear();
        Invoke("CleanBreakEnd", 0.5f);
        OnOffScroll();
    }
    private void CleanBreakEnd()
    {
        barSubject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        barCondition.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        BackAnimationSubject();
        BackAnimationCondition();
        BackAnimationExecution();
        panelBar.SetBool("PanelOn", true);
        isBreak = false;
    }

    private void CheakWordCombination() // 틀린 조합 찾기
    {
        if (subjectWord == 1 && conditionWord == 7) CleanSelectChainBreak(); //용사가 카메라 안에 들 때

        else if (subjectWord == 3 && conditionWord == 4) CleanSelectChainBreak(); //스테이지가 블록을 밟을 때

        else if (subjectWord == 4 && conditionWord == 3) CleanSelectChainBreak(); //카메라가 충돌할 때
        else if (subjectWord == 4 && conditionWord == 4) CleanSelectChainBreak(); //카메라가 블록을 밟을 때
        else if (subjectWord == 4 && conditionWord == 7) CleanSelectChainBreak(); //카메라가 카메라 안에 들을 때
        else if (subjectWord == 4 && conditionWord == 8) CleanSelectChainBreak(); //카메라가 소리를 낼 때
        else if (subjectWord == 4 && executionWord == 8) CleanSelectChainBreak(); //카메라가 충돌하지 않는다

        else if (subjectWord == 5 && conditionWord == 3) CleanSelectChainBreak(); //날씨가 충돌할 때
        else if (subjectWord == 5 && conditionWord == 4) CleanSelectChainBreak(); //날씨가 블록을 밟을 때
        else if (subjectWord == 5 && conditionWord == 7) CleanSelectChainBreak(); //날씨가 카메라 안에 들 때
        else if (subjectWord == 5 && conditionWord == 8) CleanSelectChainBreak(); //날씨가 소리를 낼 때
        else if (subjectWord == 5 && executionWord == 8) CleanSelectChainBreak(); //날씨가 충돌하지 않는다

        else if (subjectWord == 6 && conditionWord == 3) CleanSelectChainBreak(); //온도가 충돌할 때
        else if (subjectWord == 6 && conditionWord == 4) CleanSelectChainBreak(); //온도가 블록을 밟을 때
        else if (subjectWord == 6 && conditionWord == 7) CleanSelectChainBreak(); //온도가 카메라 안에 들 때
        else if (subjectWord == 6 && conditionWord == 8) CleanSelectChainBreak(); //온도가 소리를 낼 때
        else if (subjectWord == 6 && executionWord == 8) CleanSelectChainBreak(); //온도가 충돌하지 않는다


        else if (subjectWord == 7 && conditionWord == 3) CleanSelectChainBreak(); //게임창이 충돌할 때
        else if (subjectWord == 7 && conditionWord == 4) CleanSelectChainBreak(); //게임창이 블록을 밟을 때
        else if (subjectWord == 7 && conditionWord == 6) CleanSelectChainBreak(); //게임창이 떨어질 때
        else if (subjectWord == 7 && conditionWord == 7) CleanSelectChainBreak(); //게임창이 카메라 안에 들 때
        else if (subjectWord == 7 && conditionWord == 8) CleanSelectChainBreak(); //게임창이 소리를 낼 때
        else if (subjectWord == 7 && executionWord == 2) CleanSelectChainBreak(); //게임창이 1초동안 빨라진다
        else if (subjectWord == 7 && executionWord == 3) CleanSelectChainBreak(); //게임창이 1초동안 정지한다
        else if (subjectWord == 7 && executionWord == 4) CleanSelectChainBreak(); //게임창이 1초동안 느려진다
        else if (subjectWord == 7 && executionWord == 8) CleanSelectChainBreak(); //게임창이 충돌하지 않는다

        else if (subjectWord == 8 && conditionWord == 2) CleanSelectChainBreak(); //시간이 가만히 있을 때
        else if (subjectWord == 8 && conditionWord == 3) CleanSelectChainBreak(); //시간이 충돌할 때
        else if (subjectWord == 8 && conditionWord == 4) CleanSelectChainBreak(); //시간이 블록을 밟을 때
        else if (subjectWord == 8 && conditionWord == 6) CleanSelectChainBreak(); //시간이 떨어질 때
        else if (subjectWord == 8 && conditionWord == 7) CleanSelectChainBreak(); //시간이 카메라 안에 들 때
        else if (subjectWord == 8 && conditionWord == 8) CleanSelectChainBreak(); //시간이 소리를 낼 때
        else if (subjectWord == 8 && executionWord == 8) CleanSelectChainBreak(); //시간이 충돌하지 않는다
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
                        CheakWordCombination();
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
        CheakWordCombination();
    }

    private void InputDefault()
    {
        nowWord = 3;
        OnOffScroll();
        AllChangeTexts();
    }

    private void AnimationSubject()
    {
        //barSubject.GetComponent<Animator>().SetBool("PanelOn",true);
        barSubject.localScale = new Vector3(2, 2, 1);
        barSubject.GetComponent<Image>().DOKill();
        barSubject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barSubject.DOScale(new Vector2(1, 1), 0.05f).OnComplete
            (() => barSubject.DOScale(new Vector3(0.9f,0.9f,1),0.02f).OnComplete
            (() => barSubject.DOScale(new Vector3(1,1,1), 0.08f)));
    }
    private void AnimationCondition()
    {
        barCondition.localScale = new Vector3(2, 2, 1);
        barCondition.GetComponent<Image>().DOKill();
        barCondition.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barCondition.DOScale(new Vector2(1, 1), 0.05f).OnComplete
            (() => barCondition.DOScale(new Vector3(0.9f, 0.9f, 1), 0.02f).OnComplete
            (() => barCondition.DOScale(new Vector3(1, 1, 1), 0.08f)));

        chain[0].DOAnchorPosX(chainsXpos[0] + 90, 0.2f);
        chain[0].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.2f);
        //사슬 움직임
    }
    private void AnimationExecution()
    {
        barExecution.localScale = new Vector3(2, 2, 1);
        barExecution.GetComponent<Image>().DOKill();
        barExecution.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        barExecution.DOScale(new Vector2(1, 1), 0.05f).OnComplete
            (() => barExecution.DOScale(new Vector3(0.9f, 0.9f, 1), 0.02f).OnComplete
            (() => barExecution.DOScale(new Vector3(1, 1, 1), 0.08f)));

        chain[1].DOAnchorPosX(chainsXpos[1] + 90, 0.2f);
        chain[1].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.2f);
        //사슬 움직임
    }

    private void BackAnimationSubject()
    {
        barSubject.GetComponent<Image>().DOColor(new Color(1,1,1,0), 0.2f);
    }
    private void BackAnimationCondition()
    {
        barCondition.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.2f);
        chain[0].DOAnchorPosX(chainsXpos[0], 0.2f);
        chain[0].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.2f);
        //사슬 움직임
    }
    private void BackAnimationExecution()
    {
        barExecution.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.2f);
        chain[1].DOAnchorPosX(chainsXpos[1], 0.2f);
        chain[1].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.2f);
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
                wordSelect.Add(s_stage.GetComponent<WordGameObject>());
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
                Condition_OutSound();
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
                    wordSelect[i].PlayFunction("SetMoveZero");
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
                    wordSelect[i].PlayFunction("W_VisibleEffectOntrue");
                    ExecutionWordObject(i);
                }
            }
        }
    }

    private void Condition_OutSound() // 8번 소리를 낼 때
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if (wordSelect[i].isSound)
            {
                if(wordSelect[i].isSoundEffect)
                {
                    wordSelect[i].isSoundEffect = false;
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
            wordSelect[i].PlayFunction("Jump");
        }
    }
    private void Execution_Jump(int i)
    {
        wordSelect[i].PlayFunction("Jump");
    }
    private void Execution_SpeedUp() // 2번 스피드
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].PlayFunction("SpeedUp");
        }
    }
    private void Execution_SpeedUp(int i) // 2번 스피드
    {

        wordSelect[i].PlayFunction("SpeedUp");

    }
    private void Execution_TimeStop() // 3번 시간 정지
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].PlayFunction("SpeedStop");
        }
    }
    private void Execution_TimeStop(int i) // 
    {
        wordSelect[i].PlayFunction("SpeedStop");
    }
    private void Execution_SpeedDown() // 4번 스피드 떨어짐
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].PlayFunction("SpeedDown");
        }
    }
    private void Execution_SpeedDown(int i) // 4번 스피드 떨어짐
    {
        wordSelect[i].PlayFunction("SpeedDown");
    }
    private void Execution_Down() // 5번 내려간다
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            if(conditionWord == 6)
            {
                wordSelect[i].PlayFunction("SuperDown");
            }
            else
            {
                wordSelect[i].PlayFunction("Down");
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
            wordSelect[i].PlayFunction("SuperDown");
        }
        else
        {
            wordSelect[i].PlayFunction("Down");
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
            wordSelect[i].PlayFunction("SizeUp");
        }
    }
    private void Execution_SizeUp(int i)
    {
        wordSelect[i].PlayFunction("SizeUp");
    }
    private void Execution_SizeDown()
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].PlayFunction("SizeDown");
        }
    }
    private void Execution_SizeDown(int i)
    {
        wordSelect[i].PlayFunction("SizeDown");
    }

    private void Execution_ColliderOff()
    {
        for (int i = 0; i < wordSelect.Count; i++)
        {
            wordSelect[i].PlayFunction("ColliderOff");
        }
    } // 8번 1초동안 충돌하지 않는다
    private void Execution_ColliderOff(int i)
    {
        wordSelect[i].PlayFunction("ColliderOff");
    }

    public void EventOn()
    {
        CleanSelect();
        isEvent = true;
        WordSystem.SetActive(false);
        TextSystem.SetActive(true);
        textsystemRect[0].anchoredPosition = new Vector2(-700,240);
        textsystemRect[1].anchoredPosition = new Vector2(700,-190);
        textsystemRect[2].anchoredPosition = new Vector2(0,200);
        textsystemRect[0].DOAnchorPosX(0, 0.3f);
        textsystemRect[1].DOAnchorPosX(0, 0.3f).OnComplete(() => textsystemRect[2].DOAnchorPosY(0,0.15f));
    }
    public void EventOff()
    {
        isEvent = false;
        WordSystem.SetActive(true);
        TextSystem.SetActive(false);
    }

    public void PlayToDieResetAnimation()
    {
        dieAnimation.anchoredPosition = new Vector2(-640,0);
        dieAnimation.DOAnchorPosX(640, 2.5f);
    }


    public void WinGame()
    {
        wingame.SetActive(true);
        WordSystem.SetActive(false);
        wintext.GetComponent<Text>().text = "스테이지 클리어";
        wintext.anchoredPosition = new Vector2(-100, wintext.anchoredPosition.y);
        winbackground.anchoredPosition = new Vector2(-100, winbackground.anchoredPosition.y);
        wintext.DOAnchorPosX(0, 1);
        winbackground.DOAnchorPosX(0, 0.5f);
    }

    //그래픽 관련 함수

    public Material ReturnMaterials(int index)
    {
        return materials[index];
    }
}
