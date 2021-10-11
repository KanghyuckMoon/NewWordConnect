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
        {'��','��','��','��','��','��','��','��','��','��'},
        {'��','��','��','��','��','��','��','��','��','��'},
        {'��','��','��','��',' ','��','��','��','��','��'}
    };
    private char[] cho = { '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��' };
    private char[] jung = { '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��',
                            '��', '��', '��', '��', '��', '��' };
    private char[] jong = { ' ', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��',
                            '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��' };

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

    private void SetName0() // �ƹ� �͵� ���� ��
    {
        if (indexplayernamestat[indexplayername] == 0)
        {
            if (FindListInChar(hangle[y, x],cho) == -1) return;
            playername[indexplayername] = hangle[y, x];
            indexunicode[indexplayername, 0] = FindListInChar(hangle[y, x], cho);
            switch (hangle[y, x])
            {
                case '��':
                case '��':
                case '��':
                case '��':
                case '��':
                    indexplayernamestat[indexplayername] = 1;
                    break;
                default:
                    indexplayernamestat[indexplayername] = 2;
                    break;
            }
        }
    }

    private void SetName1() // �� �� �� �� �� �� ���� ��
    {
        if (indexplayernamestat[indexplayername] == 1)
        {
            if(FindListInChar(hangle[y, x], cho) == indexunicode[indexplayername, 0]) // ������ �Է���
            {
                playername[indexplayername]++;
                indexunicode[indexplayername, 0]++;
                indexplayernamestat[indexplayername] = 2;
            }
            else if(FindListInChar(hangle[y,x], jung) != -1) // �߼� �Է���
            {
                switch (hangle[y, x])
                {
                    case '��':
                    case '��':
                    case '��':
                        indexplayernamestat[indexplayername] = 3;
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 4;
                        break;
                }
                indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                SetCharName();
            }
            else // �߼��� �ƴϰ� �ٸ� ���� �Է���
            {
                indexplayernamestat[indexplayername] = 0;
                indexunicode[indexplayername, 0] = -1;
                SetName0();
            }
        }
    }

    private void SetName2() // �ʼ����ٰ� �߼� �Է���
    {
        if (indexplayernamestat[indexplayername] == 2)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) // �߼� �Է���
            {
                switch (hangle[y, x])
                {
                    case '��':
                    case '��':
                    case '��':
                        indexplayernamestat[indexplayername] = 3; // �߼� �����
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 4; // ��������
                        break;
                }
                indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                SetCharName();
            }
            else // ���� �Է��� ó������
            {
                indexplayernamestat[indexplayername] = 0;
                indexunicode[indexplayername, 0] = -1;
                indexunicode[indexplayername, 1] = -1;
                SetName0();
            }

            
        }
    }

    private void SetName3() // �߼����ٰ� �� �� �� �Է���
    {
        if (indexplayernamestat[indexplayername] == 3)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) // �߼� �Է���
            {
                switch (hangle[y, x])
                {
                    case '��':
                    case '��':
                    case '��':
                        return;
                    default:
                        break;
                }
                if (indexunicode[indexplayername, 1] == 8) // ����
                {
                    switch (hangle[y, x])
                    {
                        case '��':
                            indexunicode[indexplayername, 1] += 1;
                            break;
                        case '��':
                            indexunicode[indexplayername, 1] += 2;
                            break;
                        case '��':
                            indexunicode[indexplayername, 1] += 3;
                            break;
                        default:
                            indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                            break;
                    }
                    SetCharName();
                    indexplayernamestat[indexplayername] = 4;
                }
                if (indexunicode[indexplayername, 1] == 13) // ����
                {
                    switch (hangle[y, x])
                    {
                        case '��':
                            indexunicode[indexplayername, 1] += 1;
                            break;
                        case '��':
                            indexunicode[indexplayername, 1] += 2;
                            break;
                        case '��':
                            indexunicode[indexplayername, 1] += 3;
                            break;
                        default:
                            indexunicode[indexplayername, 1] = FindListInChar(hangle[y, x], jung);
                            break;
                    }
                    SetCharName();
                    indexplayernamestat[indexplayername] = 4;
                }
                if (indexunicode[indexplayername, 1] == 18) // ����
                {
                    switch (hangle[y, x])
                    {
                        case '��':
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
            else // ���� �Է��ؼ� �������� �Ѿ
            {
                indexunicode[indexplayername, 2] = FindListInChar(hangle[y, x], jong);
                SetCharName();
                switch (hangle[y, x])
                {
                    case '��':
                    case '��':
                    case '��':
                    case '��':
                    case '��':
                        indexplayernamestat[indexplayername] = 5;
                        break;
                    default:
                        indexplayernamestat[indexplayername] = 6;
                        break;
                }
            }
        }
    }

    private void SetName4() // ���� �Է���
    {
        if(indexplayernamestat[indexplayername] == 4)
        {
            if (FindListInChar(hangle[y, x], cho) == -1) return; // ���� �Է��ϸ� ��������
            switch (hangle[y, x])
            {
                case '��':
                case '��':
                case '��':
                case '��':
                case '��':
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

    private void SetName5() // ������ �� �� �� �� �� ��
    {
        if (indexplayernamestat[indexplayername] == 5)
        {
            if (FindListInChar(hangle[y, x], jung) != -1) return; // �߼� �Է���
            if (indexunicode[indexplayername, 2] == 1) // ����
            {
                switch (hangle[y, x])
                {
                    case '��':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
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
            else if (indexunicode[indexplayername, 2] == 4) // ����
            {
                switch (hangle[y, x])
                {
                    case '��':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
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
            else if (indexunicode[indexplayername, 2] == 8) // ����
            {
                switch (hangle[y, x])
                {
                    case '��':
                        indexunicode[indexplayername, 2] += 1;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
                        indexunicode[indexplayername, 2] += 2;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
                        indexunicode[indexplayername, 2] += 3;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
                        indexunicode[indexplayername, 2] += 4;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
                        indexunicode[indexplayername, 2] += 5;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
                        indexunicode[indexplayername, 2] += 6;
                        indexplayernamestat[indexplayername] = 6;
                        break;
                    case '��':
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
            else if (indexunicode[indexplayername, 2] == 17) // ����
            {
                switch (hangle[y, x])
                {

                    case '��':
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
            else if (indexunicode[indexplayername, 2] == 19) // ����
                {
                    switch (hangle[y, x])
                    {

                        case '��':
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

    private void SetName6() // �ϼ� �ƴµ� �� �Է���
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

    private void InputZero() // ������ �Է���
    {
        if(hangle[y,x] == ' ')
        {
            indexunicode[indexplayername, 0] = -1;
            indexunicode[indexplayername, 1] = -1;
            indexunicode[indexplayername, 2] = 0;
            indexplayernamestat[indexplayername] = 0;
            playername[indexplayername] = ' ';
        }
        else if(hangle[y,x] == '��')
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
            playername[0] = '��';
            playername[1] = '��';
            playername[2] = '��';
        }
    }
}
