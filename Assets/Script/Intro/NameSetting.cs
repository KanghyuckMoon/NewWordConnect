using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NameSetting : MonoBehaviour
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
    private ImageLoding imageLoding = null;
    private bool nameOn = false;
    private bool playing = false;
    private char[,] hangle =
    {
        {'ㄱ','ㄴ','ㄷ','ㄹ','ㅁ','ㅏ','ㅑ','ㅓ','ㅕ','ㅗ'},
        {'ㅂ','ㅅ','ㅇ','ㅈ','ㅊ','ㅛ','ㅜ','ㅠ','ㅐ','ㅒ'},
        {'ㅋ','ㅌ','ㅍ','ㅎ',' ','끝','ㅔ','ㅖ','ㅡ','ㅣ'}
    };
    private char[] cho = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private char[] jung = { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ',
                            'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private char[] jong = { ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ',
                            'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    private int x, y;
    private int indexplayername = 0;
    private int[] indexplayernamestat = new int[5];
    private int[,] indexunicode = new int[5,3];
    private char[] playername = new char[5];


    [SerializeField]
    private Transform hangleobj = null;
    [SerializeField]
    private RectTransform box;
    [SerializeField]
    private RectTransform selectbox;
    private RectTransform[,] recthangle = new RectTransform[3,10];
    private KeySetting keysetting;

    [SerializeField]
    private Text[] NameText = new Text[5];

    [SerializeField]
    private RectTransform yesnobox;
    [SerializeField]
    private RectTransform yes;
    [SerializeField]
    private RectTransform no;
    private bool yesnoon;
    private int yesnoselect;

    [SerializeField]
    private GameObject NameObjects;

    [SerializeField]
    private Image backgroundImage;
    private WaitForSeconds wait;

    private void Awake()
    {
        wait = new WaitForSeconds(0.1f);
        int k = 0;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                recthangle[i,j] = hangleobj.GetChild(k).GetComponent<RectTransform>();
                k++;
            }
        }
    }

    private void Start()
    {
        keysetting = SaveManager.Instance.CurrenKeySetting;
        MoveBox();
        MoveSelectBox();
        playername[0] = ' ';
        playername[1] = ' ';
        playername[2] = ' ';
        playername[3] = ' ';
        playername[4] = ' ';
    }

    private void Update()
    {

        if (nameOn)
        {
            NameInput();
        }
        else
        {
            if (imageLoding.GetEnd() == true)
            {
                if (playing) return;
                playing = true;
                SetNameObj();
            }
        }
    }

    private IEnumerator FadeOut()
    {
        backgroundImage.color = new Color(0, 0, 0, 1);
        for(float i = 0; i <= 1; i+=0.1f)
        {
            backgroundImage.color = new Color(i, i, i, 1);
            yield return wait;
        }
        backgroundImage.color = new Color(1, 1, 1, 1);
        yield return wait;
        yield return wait;
        SceneManager.LoadScene("StageSelect");
    }

    private void SetNameObj()
    {
        nameOn = true;
        NameObjects.SetActive(true);
    }

    private void NameInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (yesnoon) return;
            if (y == 0) return;
            y--;
            MoveBox();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            if (yesnoon) return;
            if (y == 2) return;
            y++;
            MoveBox();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (yesnoon)
            {
                yesnoselect -= 1;
                if (yesnoselect < 0) yesnoselect = 1;
                else if (yesnoselect > 1) yesnoselect = 0;
                MoveBox();
                return;
            }
            if (x == 0) return;
            x--;
            MoveBox();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (yesnoon)
            {
                yesnoselect -= 1;
                if (yesnoselect < 0) yesnoselect = 1;
                else if (yesnoselect > 1) yesnoselect = 0;
                MoveBox();
                return;
            }
            if (x == 9) return;
            x++;
            MoveBox();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SetName();
        }
        else
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i + 1 >= 9 ? 9 : i + 1] - (keysetting.Numpad ? 0 : 208)))
                {
                    if (i < 5)
                    {
                        indexplayername = i;
                        MoveSelectBox();
                    }
                }
            }
        }
        
    }

    private void MoveBox()
    {
        if(yesnoon)
        {
            if(yesnoselect == 0)
            {
            box.DOAnchorPos(yes.anchoredPosition, 0.2f);
            }
            else
            {
                box.DOAnchorPos(no.anchoredPosition, 0.2f);
            }
        }
        else
        {
        box.DOAnchorPos(recthangle[y,x].anchoredPosition,0.2f);
        }

    }

    private void MoveSelectBox()
    {
        if(yesnoon)
        {
            selectbox.DOAnchorPos(yesnobox.anchoredPosition, 0.2f);
        }
        else
        {
        selectbox.DOAnchorPos(NameText[indexplayername].GetComponent<RectTransform>().anchoredPosition, 0.2f);
        }
    }

    private void SetName()
    {
        if(yesnoon)
        {
            if(yesnoselect == 0)
            {
                yesnoon = false;
                yesnobox.gameObject.SetActive(false);
                yes.gameObject.SetActive(false);
                no.gameObject.SetActive(false);
                NameObjects.SetActive(false);
                StartCoroutine(FadeOut());
            }
            else
            {
                yesnoon = false;
                yesnobox.gameObject.SetActive(false);
                yes.gameObject.SetActive(false);
                no.gameObject.SetActive(false);
                MoveBox();
                MoveSelectBox();
            }
        }
        else
        {
            InputZero();
            switch (indexplayernamestat[indexplayername])
            {
                case 6:
                    SetName6();
                    break;

                case 5:
                    SetName5();
                    break;

                case 4:
                    SetName4();
                    break;

                case 3:
                    SetName3();
                    break;

                case 2:
                    SetName2();
                    break;

                case 1:
                    SetName1();
                    break;

                case 0:
                    SetName0();
                    break;
            }
            NameText[0].text = string.Format("{0}", playername[0]);
            NameText[1].text = string.Format("{0}", playername[1]);
            NameText[2].text = string.Format("{0}", playername[2]);
            NameText[3].text = string.Format("{0}", playername[3]);
            NameText[4].text = string.Format("{0}", playername[4]);
        }
        
    }

    private int FindListInChar(char value, char[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == value)
            {
                return i;
            }
        }
        return -1;
    }

    private void SetCharName()
    {
        playername[indexplayername] = (char)
            ((indexunicode[indexplayername, 0] * 21 * 28) + 
            (indexunicode[indexplayername, 1] * 28) + 
            (indexunicode[indexplayername, 2]) + 0xAC00);
    }     

    private void SetName0() // 아무 것도 없을 때
    {
        if (indexplayernamestat[indexplayername] == 0)
        {
            if (FindListInChar(hangle[y, x],cho) == -1) return;
            playername[indexplayername] = hangle[y, x];
            indexunicode[indexplayername, 0] = FindListInChar(hangle[y, x], cho);
            switch (hangle[y, x])
            {
                case 'ㄱ':
                case 'ㄷ':
                case 'ㅂ':
                case 'ㅅ':
                case 'ㅈ':
                    indexplayernamestat[indexplayername] = 1;
                    break;
                default:
                    indexplayernamestat[indexplayername] = 2;
                    break;
            }
        }
    }

    private void SetName1() // ㄱ ㄷ ㅂ ㅅ ㅈ 만 있을 때
    {
        if (indexplayernamestat[indexplayername] == 1)
        {
            if(FindListInChar(hangle[y, x], cho) == indexunicode[indexplayername, 0]) // 같은거 입력함
            {
                playername[indexplayername]++;
                indexunicode[indexplayername, 0]++;
                indexplayernamestat[indexplayername] = 2;
            }
            else if(FindListInChar(hangle[y,x], jung) != -1) // 중성 입력함
            {
                switch (hangle[y, x])
                {
                    case 'ㅗ':
                    case 'ㅜ':
                    case 'ㅡ':
                        indexplayernamestat[indexplayername] = 3;
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 4;
                        break;
                }
                indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                SetCharName();
            }
            else // 중성도 아니고 다른 자음 입력함
            {
                indexplayernamestat[indexplayername] = 0;
                indexunicode[indexplayername, 0] = -1;
                SetName0();
            }
        }
    }

    private void SetName2() // 초성에다가 중성 입력함
    {
        if (indexplayernamestat[indexplayername] == 2)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) // 중성 입력함
            {
                switch (hangle[y, x])
                {
                    case 'ㅗ':
                    case 'ㅜ':
                    case 'ㅡ':
                        indexplayernamestat[indexplayername] = 3; // 중성 연결로
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 4; // 종성으로
                        break;
                }
                indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                SetCharName();
            }
            else // 자음 입력함 처음으로
            {
                indexplayernamestat[indexplayername] = 0;
                indexunicode[indexplayername, 0] = -1;
                indexunicode[indexplayername, 1] = -1;
                SetName0();
            }

            
        }
    }

    private void SetName3() // 중성에다가 ㅗ ㅜ ㅡ 입력함
    {
        if (indexplayernamestat[indexplayername] == 3)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) // 중성 입력함
            {
                switch (hangle[y, x])
                {
                    case 'ㅗ':
                    case 'ㅜ':
                    case 'ㅡ':
                        return;
                    default:
                        break;
                }
                if (indexunicode[indexplayername, 1] == 8) // ㅗ임
                {
                    switch (hangle[y, x])
                    {
                        case 'ㅏ':
                            indexunicode[indexplayername, 1] += 1;
                            break;
                        case 'ㅐ':
                            indexunicode[indexplayername, 1] += 2;
                            break;
                        case 'ㅣ':
                            indexunicode[indexplayername, 1] += 3;
                            break;
                        default:
                            indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                            break;
                    }
                    SetCharName();
                    indexplayernamestat[indexplayername] = 4;
                }
                if (indexunicode[indexplayername, 1] == 13) // ㅜ임
                {
                    switch (hangle[y, x])
                    {
                        case 'ㅓ':
                            indexunicode[indexplayername, 1] += 1;
                            break;
                        case 'ㅔ':
                            indexunicode[indexplayername, 1] += 2;
                            break;
                        case 'ㅣ':
                            indexunicode[indexplayername, 1] += 3;
                            break;
                        default:
                            indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                            break;
                    }
                    SetCharName();
                    indexplayernamestat[indexplayername] = 4;
                }
                if (indexunicode[indexplayername, 1] == 18) // ㅡ임
                {
                    switch (hangle[y, x])
                    {
                        case 'ㅣ':
                            indexunicode[indexplayername, 1] += 1;
                            break;
                        default:
                            indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                            break;
                    }
                    SetCharName();
                    indexplayernamestat[indexplayername] = 4;
                }

            }
            else // 자음 입력해서 종성으로 넘어감
            {
                indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                SetCharName();
                switch (hangle[y, x])
                {
                    case 'ㄱ':
                    case 'ㄴ':
                    case 'ㄹ':
                    case 'ㅂ':
                    case 'ㅅ':
                        indexplayernamestat[indexplayername] = 5;
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 6;
                        break;
                }
            }
        }
    }

    private void SetName4() // 종성 입력함
    {
        if(indexplayernamestat[indexplayername] == 4)
        {
            if (FindListInChar(hangle[y, x], cho) == -1) return; // 모음 입력하면 돌려보냄
            switch (hangle[y, x])
            {
                case 'ㄱ':
                case 'ㄴ':
                case 'ㄹ':
                case 'ㅂ':
                case 'ㅅ':
                    indexplayernamestat[indexplayername] = 5;
                    indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                    SetCharName();
                    break;
                default:
                    indexplayernamestat[indexplayername] = 6;
                    indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                    SetCharName();
                    break;
            }
        }
    }

    private void SetName5() // 종성이 ㄱ ㄴ ㄹ ㅂ ㅅ 임
    {
        if (indexplayernamestat[indexplayername] == 5)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) return; // 중성 입력함
            if (indexunicode[indexplayername, 2] == 1) // ㄱ임
            {
                switch (hangle[y, x])
                {
                    case 'ㄱ':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅅ':
                        indexunicode[indexplayername, 2] += 2;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    default:
                        indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                        indexplayernamestat[indexplayername] = 4;
                        SetName4();
                        break;
                }
                SetCharName();
            }
            else if (indexunicode[indexplayername, 2] == 4) // ㄴ임
            {
                switch (hangle[y, x])
                {
                    case 'ㅈ':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅎ':
                        indexunicode[indexplayername, 2] += 2;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    default:
                        indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                        indexplayernamestat[indexplayername] = 4;
                        SetName4();
                        break;
                }
                SetCharName();
            }
            else if (indexunicode[indexplayername, 2] == 8) // ㄹ임
            {
                switch (hangle[y, x])
                {
                    case 'ㄱ':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅁ':
                        indexunicode[indexplayername, 2] += 2;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅂ':
                        indexunicode[indexplayername, 2] += 3;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅅ':
                        indexunicode[indexplayername, 2] += 4;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅌ':
                        indexunicode[indexplayername, 2] += 5;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅍ':
                        indexunicode[indexplayername, 2] += 6;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case 'ㅎ':
                        indexunicode[indexplayername, 2] += 7;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    default:
                        indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                        indexplayernamestat[indexplayername] = 4;
                        SetName4();
                        break;
                }
            }
            else if (indexunicode[indexplayername, 2] == 17) // ㅂ임
            {
                switch (hangle[y, x])
                {

                    case 'ㅅ':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    default:
                        indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                        indexplayernamestat[indexplayername] = 4;
                        SetName4();
                        break;
                }
                SetCharName();
            }
            else if (indexunicode[indexplayername, 2] == 19) // ㅅ임
                {
                    switch (hangle[y, x])
                    {

                        case 'ㅅ':
                            indexunicode[indexplayername, 2] += 1;
                            indexplayernamestat[indexplayername] = 6;
                            break;
                        default:
                            indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                            indexplayernamestat[indexplayername] = 4;
                            SetName4();
                            break;
                    }
                    SetCharName();
                }
            }
    }

    private void SetName6() // 완성 됐는데 또 입력함
    {
        if(indexplayernamestat[indexplayername] == 6)
        {
            indexunicode[indexplayername, 0] = 0;
            indexunicode[indexplayername, 1] = 0;
            indexunicode[indexplayername, 2] = 0;
            indexplayernamestat[indexplayername] = 0;
            SetName0();
        }
    }

    private void InputZero() // 공백을 입력함
    {
        if(hangle[y,x] == ' ')
        {
            indexunicode[indexplayername, 0] = -1;
            indexunicode[indexplayername, 1] = -1;
            indexunicode[indexplayername, 2] = 0;
            indexplayernamestat[indexplayername] = 0;
            playername[indexplayername] = ' ';
        }
        else if(hangle[y,x] == '끝')
        {
            yesnoon = true;
            yesnobox.gameObject.SetActive(true);
            yes.gameObject.SetActive(true);
            no.gameObject.SetActive(true);


            MoveBox();
            MoveSelectBox();
            for(int i = 0; i < 5; i++)
            {
                if(playername[i] != ' ')
                {
                    return;
                }
            }
            playername[0] = '마';
            playername[1] = '리';
            playername[2] = '오';
        }
    }
}
