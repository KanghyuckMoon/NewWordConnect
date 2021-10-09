using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroInput : MonoBehaviour
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
    private Text wariningText;

    [SerializeField]
    private bool introOn = false;

    private int introSubject = 0;
    private int introCondition = 0;
    private int introExecution = 0;
    private int num = 0;
    private int nowword = 0;

    [SerializeField]
    private Transform boxSubject = null;
    [SerializeField]
    private Transform boxCondition = null;
    [SerializeField]
    private Transform boxExecution = null;

    [SerializeField]
    private GameObject firstWord = null;
    [SerializeField]
    private GameObject secondWord = null;
    [SerializeField]
    private GameObject thirdWord = null;

    private Vector2 firstposition = Vector2.zero;
    private Vector2 secondposition = Vector2.zero;
    private Vector2 thirdposition = Vector2.zero;
    private float movetime = 0.5f;
    private bool playing = false;

    [SerializeField]
    private ImageLoding imageLoding = null;
    [SerializeField]
    private Image[] numbers;
    [SerializeField]
    private Text guidetext;
    private WaitForSeconds waitForSeconds = null;

    private bool numpadOn = true;


    [SerializeField]
    private Image[] keysettingImage;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(0.1f);
        firstposition = firstWord.transform.position;
        secondposition = secondWord.transform.position;
        thirdposition = thirdWord.transform.position;
        imageLoding.gameObject.SetActive(true);
    }

    private void Update()
    {
        
        if(introOn)
        {
            InputInIntro();
        }
        else
        {
            if (imageLoding.GetEnd() == true)
            {
                if (playing) return;
                playing = true;
                StartCoroutine(KeySettingStart());
            }
        }
    }

    public void KeySettingEnd()
    {
        StartCoroutine(KeySettingEndFade());
    }

    public void SetWasd(bool input)
    {
        SaveManager.Instance.CurrenKeySetting.Wasd = input;
    }
    public void SetKeyPad(bool input)
    {
        SaveManager.Instance.CurrenKeySetting.Numpad = input;
        numpadOn = input;
    }

    private IEnumerator KeySettingStart()
    {
        for (int j = 0; j < keysettingImage.Length; j++)
        {
            keysettingImage[j].gameObject.SetActive(true);
            keysettingImage[j].color = new Color(1, 1, 1, 0);
        }
        for (float i = 0; i <= 1; i+= 0.1f)
        {
            for(int j = 0; j < keysettingImage.Length; j++)
            {
                keysettingImage[j].color = new Color(1, 1, 1, i);
            }
            yield return waitForSeconds;
        }
        yield return waitForSeconds;
    }

    private IEnumerator KeySettingEndFade()
    {
        
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            for (int j = 0; j < keysettingImage.Length; j++)
            {
                keysettingImage[j].color = new Color(1, 1, 1, i);
            }
            yield return waitForSeconds;
        }
        for (int j = 0; j < keysettingImage.Length; j++)
        {
            keysettingImage[j].gameObject.SetActive(false);
        }
        yield return waitForSeconds;
        IntroStart();
    }

    public void IntroStart()
    {
        StartCoroutine(FadeIn());
    }

    public void IntroEnd()
    {
        if (playing) return;
        playing = true;
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeIn()
    {
        wariningText.gameObject.SetActive(true);
        boxSubject.gameObject.SetActive(true);
        boxCondition.gameObject.SetActive(true);
        boxExecution.gameObject.SetActive(true);
        firstWord.SetActive(true);
        secondWord.SetActive(true);
        thirdWord.SetActive(true);
        numbers[0].gameObject.SetActive(true);
        numbers[1].gameObject.SetActive(true);
        numbers[2].gameObject.SetActive(true);
        guidetext.gameObject.SetActive(true);

        for (float i = 0; i <= 1; i += 0.1f)
        {
            wariningText.color = new Color(1, 1, 1, i);
            boxSubject.GetComponent<Image>().color = new Color(1, 1, 1, i);
            boxCondition.GetComponent<Image>().color = new Color(1, 1, 1, i);
            boxExecution.GetComponent<Image>().color = new Color(1, 1, 1, i);
            numbers[0].GetComponent<Image>().color = new Color(1, 1, 1, i);
            numbers[1].GetComponent<Image>().color = new Color(1, 1, 1, i);
            numbers[2].GetComponent<Image>().color = new Color(1, 1, 1, i);
            guidetext.GetComponent<Text>().color = new Color(1, 1, 1, i);
            firstWord.GetComponent<Text>().color = new Color(1, 1, 1, i);
            secondWord.GetComponent<Text>().color = new Color(1, 1, 1, i);
            thirdWord.GetComponent<Text>().color = new Color(1, 1, 1, i);

            yield return waitForSeconds;
        }
        wariningText.color = new Color(1, 1, 1, 1);
        boxSubject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        boxCondition.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        boxExecution.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        firstWord.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        secondWord.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        thirdWord.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        introOn = true;
        playing = false;
        yield return null;
    }

    private IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            wariningText.color = new Color(1, 1, 1, i);
            boxSubject.GetComponent<Image>().color = new Color(1, 1, 1, i);
            boxCondition.GetComponent<Image>().color = new Color(1, 1, 1, i);
            boxExecution.GetComponent<Image>().color = new Color(1, 1, 1, i);
            firstWord.GetComponent<Text>().color = new Color(1, 1, 1, i);
            secondWord.GetComponent<Text>().color = new Color(1, 1, 1, i);
            thirdWord.GetComponent<Text>().color = new Color(1, 1, 1, i);
            guidetext.GetComponent<Text>().color = new Color(1, 1, 1, i);
            yield return waitForSeconds;
        }
        wariningText.color = new Color(1, 1, 1, 0);
        boxSubject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxCondition.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxExecution.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        firstWord.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        secondWord.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        thirdWord.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        wariningText.gameObject.SetActive(false);
        boxSubject.gameObject.SetActive(false);
        boxCondition.gameObject.SetActive(false);
        boxExecution.gameObject.SetActive(false);
        firstWord.SetActive(false);
        secondWord.SetActive(false);
        thirdWord.SetActive(false);
        numbers[0].gameObject.SetActive(false);
        numbers[1].gameObject.SetActive(false);
        numbers[2].gameObject.SetActive(false);
        guidetext.gameObject.SetActive(false);

        introOn = false;
        SceneManager.LoadScene("MainTitle");
        yield return null;
    }

    private void InputInIntro()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(  keyCodes[i] - (numpadOn ? 0 : 208)))
            {
                num = i;

                switch(nowword)
                {
                    case 0:
                        if (introSubject == num || introCondition == num || introExecution == num) return;
                        nowword = 1;
                        introSubject = num;
                        MoveWord(num);
                        break;
                    case 1:
                        if (introSubject == num || introCondition == num || introExecution == num) return;
                        nowword = 2;
                        introCondition = num;
                        MoveWord(num);
                        break;
                    case 2:
                        if (introSubject == num || introCondition == num || introExecution == num) return;
                        nowword = 3;
                        introExecution = num;
                        MoveWord(num);
                        if (introSubject == 1 && introCondition == 2 && introExecution == 3) IntroEnd();
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetMouseButtonDown(1))
            {
                switch (nowword)
                {
                    case 1:
                        BackMoveWord();
                        nowword = 0;
                        introSubject = 0;
                        return;
                    case 2:
                        BackMoveWord();
                        nowword = 1;
                        introCondition = 0;
                        return;
                    case 3:
                        BackMoveWord();
                        nowword = 2;
                        introExecution = 0;
                        return;
                }
            }
        }
    }

    private void BackMoveWord()
    {
        switch(nowword)
        {
            case 1:
                switch(introSubject)
                {
                    case 1:
                        firstWord.transform.DOMove(firstposition, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(secondposition, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(thirdposition, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                }
                break;
            case 2:
                switch (introCondition)
                {
                    case 1:
                        firstWord.transform.DOMove(firstposition, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(secondposition, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(thirdposition, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                }
                break;
            case 3:
                switch (introExecution)
                {
                    case 1:
                        firstWord.transform.DOMove(firstposition, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(secondposition, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(thirdposition, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 1), 1);
                        break;
                }
                break;
        }
    }

    private void MoveWord(int num)
    {
        switch (nowword)
        {
            case 1:
                switch (num)
                {
                    case 1:
                        firstWord.transform.DOMove(boxSubject.position, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(boxSubject.position, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(boxSubject.position, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                }
                break;
            case 2:
                switch (num)
                {
                    case 1:
                        firstWord.transform.DOMove(boxCondition.position, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(boxCondition.position, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(boxCondition.position, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                }
                break;
            case 3:
                switch (num)
                {
                    case 1:
                        firstWord.transform.DOMove(boxExecution.position, movetime);
                        numbers[0].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 2:
                        secondWord.transform.DOMove(boxExecution.position, movetime);
                        numbers[1].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                    case 3:
                        thirdWord.transform.DOMove(boxExecution.position, movetime);
                        numbers[2].DOColor(new Color(1, 1, 1, 0), 1);
                        break;
                }
                break;
        }
    }
}
