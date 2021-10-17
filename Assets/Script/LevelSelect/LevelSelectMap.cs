using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelSelectMap : MonoBehaviour
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

	public StageCharacter Character;
	public StagePin StartPin;
	[SerializeField]
	private Text SelectedNameText;
	[SerializeField]
	private Text ClearText;
	private bool isEsc;
	private KeySetting keysetting;
	private SaveUser saveuser;
	[SerializeField]
	private GameObject[] backgroundObj;

	//ESC 하위 오브젝트들
	[SerializeField]
	private GameObject EscUI;
	private int selectedESC = 1;
	[SerializeField]
	private Image[] EscImages;
	[SerializeField]
	private Text[] EscTexts;
	[SerializeField]
	private RectTransform[] Panels;
	[SerializeField]
	private Image CharacterImage;
	[SerializeField]
	private RectTransform[] getsubjectWords;
	[SerializeField]
	private RectTransform[] getconditionWords;
	[SerializeField]
	private RectTransform[] getexecutionWords;
	//옵션 패널
	[SerializeField]
	private GameObject optionTextObj;
	[SerializeField]
	private RectTransform[] optionTexts;
	[SerializeField]
	private GameObject keysettingObj;
	[SerializeField]
	private GameObject[] keysettingBtn;
	[SerializeField]
	private RectTransform[] keysettingoptionTexts;
	[SerializeField]
	private GameObject soundsettingObj;
	[SerializeField]
	private RectTransform[] soundsettingoptionTexts;
	[SerializeField]
	private RectTransform selectImage;
	private int optionselect = 0;
	private int nowoptionselect = -1;
	private bool isKeySetting;

	[SerializeField]
	private Material[] materials;

	private void Start()
	{
		// Pass a ref and default the player Starting Pin
		Character.Initialise(this, StartPin);
		keysetting = SaveManager.Instance.CurrenKeySetting;
		saveuser = SaveManager.Instance.CurrentSaveUser;
		SetSizeImage();
		SetActiveWords();
		OptionObjSetActive();
		WASDKeySetting(keysetting.Wasd);
	}

	private void Update()
	{
		// Only check input when character is stopped
		if (isKeySetting) return;
		if (Character.isMoving) return;
		InputEsc();
		if (isEsc) return;

		// First thing to do is try get the player input
		CheckForInput();
	}

	private void CheckForInput()
	{
		if (Input.GetKeyUp((KeyCode)keysetting.wasdKeyCodes[0]))
		{
			Character.TrySetDirection(Direction.Up);
		}
		else if (Input.GetKeyUp((KeyCode)keysetting.wasdKeyCodes[1]))
		{
			Character.TrySetDirection(Direction.Down);
		}
		else if (Input.GetKeyUp((KeyCode)keysetting.wasdKeyCodes[2]))
		{
			Character.TrySetDirection(Direction.Left);
		}
		else if (Input.GetKeyUp((KeyCode)keysetting.wasdKeyCodes[3]))
		{
			Character.TrySetDirection(Direction.Right);
		}
		else if (Input.GetKeyDown(KeyCode.Space))
        {
			Character.StageStart();
        }
	}

	public void SetTexts()
    {
		SelectedNameText.text = Character.CurrentPin.ViewStageName;
		if (Character.CurrentPin.ReturnStat() == 2)
        {
			ClearText.gameObject.SetActive(true);
        }
		else
		{
			ClearText.gameObject.SetActive(false);
		}

	}

	private void InputEsc()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
			isEsc = !isEsc;
			EscUI.SetActive(isEsc);
			backgroundObj[0].SetActive(!isEsc);
			backgroundObj[1].SetActive(!isEsc);
        }
		else if(selectedESC == 2 && isEsc)
        {
			if(Input.GetKeyDown((KeyCode)keysetting.wasdKeyCodes[0])) // W
            {
				optionselect--;
				if (optionselect < 0)
                {
					optionselect = 0;
					return;
				}
				MoveOptionSelect();
			}
			else if(Input.GetKeyDown((KeyCode)keysetting.wasdKeyCodes[1])) // S
			{
				optionselect++;
				if(nowoptionselect == -1)
				{
					if (optionselect > optionTexts.Length - 1)
					{
						optionselect = optionTexts.Length - 1;
						return;
					}
				}
				else if(nowoptionselect == 0)
                {
					if (optionselect > keysettingoptionTexts.Length - 1)
					{
						optionselect = keysettingoptionTexts.Length - 1;
						return;
					}
				}
				else if (nowoptionselect == 1)
				{
					if (optionselect > soundsettingoptionTexts.Length - 1)
					{
						optionselect = soundsettingoptionTexts.Length - 1;
						return;
					}
				}
				MoveOptionSelect();
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (nowoptionselect == -1)
                {
					nowoptionselect = optionselect;
					optionselect = 0;
					OptionObjSetActive();
					MoveOptionSelect();
				}
				else if(nowoptionselect == 0)
				{
					OptionObjSetActive();
					MoveOptionSelect();
					if(optionselect == 0)
                    {
						keysettingBtn[0].SetActive(true);
						keysettingBtn[1].SetActive(false);
						isKeySetting = true;
                    }
					else if(optionselect == 1)
					{
						keysettingBtn[0].SetActive(false);
						keysettingBtn[1].SetActive(true);
						isKeySetting = true;
					}
					optionselect = 0;
				}
				else if(nowoptionselect == 1)
				{

				}
				else if(nowoptionselect == 2)
                {
					SceneManager.LoadScene("MainTitle");
                }
				else if(nowoptionselect == 3)
				{
					#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
					#else
                            Application.Quit(); // 어플리케이션 종료
					#endif
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backspace))
			{
				nowoptionselect = -1;
				optionselect = 0;
				OptionObjSetActive();
				MoveOptionSelect();
			}
			else
            {
				for (int i = 0; i < keyCodes.Length; i++)
				{
					if (Input.GetKeyDown(keyCodes[i] - (keysetting.Numpad ? 0 : 208)))
					{
						if(nowoptionselect == 1)
                        {
							if(optionselect == 0)
                            {
								SetBackGroundVolume(i);
                            }
							else
                            {
								SetEffectVolume(i);
                            }
                        }
						else if (i < 3 && i > 0)
						{
							selectedESC = i;
							SetSizeImage();
						}
					}
				}
			}
        }
		else
		{
			for (int i = 0; i < keyCodes.Length; i++)
			{
				if (Input.GetKeyDown(keyCodes[i] - (keysetting.Numpad ? 0 : 208)))
				{
						if (i < 3 && i > 0)
						{
							selectedESC = i;
							SetSizeImage();
						}
				}
			}
		}
	}
	private void SetSizeImage()
    {
		for(int i = 1; i<EscImages.Length;i++)
        {
			if(i == selectedESC)
			{
				EscImages[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(420,80), 0.3f);
				EscTexts[i].GetComponent<RectTransform>().DOScale(new Vector2(1,1), 0.3f);
				EscImages[i].material = materials[1];
				if (EscImages[i].GetComponent<RectTransform>().anchoredPosition.x > 0)
				{
					EscImages[i].GetComponent<RectTransform>().DOAnchorPosX(140, 0.3f);
				}
				else
				{
					EscImages[i].GetComponent<RectTransform>().DOAnchorPosX(-140, 0.3f);
                }
				Panels[i - 1].DOAnchorPosX(0, 0.3f);
			}
			else
			{
				EscImages[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(340, 50), 0.3f);
				EscTexts[i].GetComponent<RectTransform>().DOScale(new Vector2(0.7f, 0.7f), 0.3f);
				EscImages[i].material = materials[0];
				if (EscImages[i].GetComponent<RectTransform>().anchoredPosition.x > 0)
				{
					EscImages[i].GetComponent<RectTransform>().DOAnchorPosX(177, 0.3f);
				}
				else
				{
					EscImages[i].GetComponent<RectTransform>().DOAnchorPosX(-177, 0.3f);
				}
				if(i < selectedESC)
                {
					Panels[i - 1].DOAnchorPosX(-700, 0.3f);
                }
				else
				{
					Panels[i - 1].DOAnchorPosX(700, 0.3f);
				}
			}
        }
    }

	private void SetActiveWords()
    {
		for(int i = 1; i<saveuser.subjectGet.Count - 1; i++)
        {
			if (saveuser.subjectGet[i - 1] - 1 <= -1) continue;
			getsubjectWords[saveuser.subjectGet[i - 1]-1].gameObject.SetActive(true);
        }
		for (int i = 1; i < saveuser.conditionGet.Count; i++)
		{
			if (saveuser.conditionGet[i - 1]-1 <= -1) continue;
			getconditionWords[saveuser.conditionGet[i - 1]-1].gameObject.SetActive(true);
		}
		for (int i = 1; i < saveuser.executionGet.Count; i++)
		{
			if (saveuser.executionGet[i - 1]-1 <= -1) continue;
			getexecutionWords[saveuser.executionGet[i - 1]-1].gameObject.SetActive(true);
		}
		ShakeWords();
	}

	private void ShakeWords()
	{
		for(int i = 0; i < getsubjectWords.Length; i++)
        {
			if(getsubjectWords[i].gameObject.activeSelf)
				getsubjectWords[i].DOShakeAnchorPos(10, 2, 1,180);
        }
		for (int i = 0; i < getconditionWords.Length; i++)
		{
			if (getconditionWords[i].gameObject.activeSelf)
				getconditionWords[i].DOShakeAnchorPos(10, 2, 1,180);
		}
		for (int i = 0; i < getexecutionWords.Length; i++)
		{
			if (getexecutionWords[i].gameObject.activeSelf)
				getexecutionWords[i].DOShakeAnchorPos(10, 2, 1,180);
		}

		Invoke("ShakeWords", 5);
	}

	private void MoveOptionSelect()
    {
		if(nowoptionselect == -1)
        {
			selectImage.DOAnchorPosY(optionTexts[optionselect].anchoredPosition.y, 0.2f);
			switch (optionselect)
			{
				case 1:
					selectImage.DOAnchorPosX(-130, 0.2f);
					break;
				case 2:
					selectImage.DOAnchorPosX(-220, 0.2f);
					break;
				default:
					selectImage.DOAnchorPosX(-116, 0.2f);
					break;
			}
		}
		else if (nowoptionselect == 0)
		{
			selectImage.DOAnchorPosY(keysettingoptionTexts[optionselect].anchoredPosition.y, 0.2f);
			selectImage.DOAnchorPosX(-220, 0.2f);
		}
		else if (nowoptionselect == 1)
		{
			selectImage.DOAnchorPosY(soundsettingoptionTexts[optionselect].anchoredPosition.y, 0.2f);
			selectImage.DOAnchorPosX(-140, 0.2f);
		}
		
    }

	private void OptionObjSetActive()
    {
		if(nowoptionselect == -1)
        {
			keysettingObj.SetActive(false);
			keysettingBtn[0].SetActive(false);
			keysettingBtn[1].SetActive(false);
			soundsettingObj.SetActive(false);
			optionTextObj.SetActive(true);
        }
		else if (nowoptionselect == 0)
		{
			keysettingObj.SetActive(true);
			keysettingBtn[0].SetActive(false);
			keysettingBtn[1].SetActive(false);
			soundsettingObj.SetActive(false);
			optionTextObj.SetActive(false);
		}
		else if (nowoptionselect == 1)
		{
			keysettingObj.SetActive(false);
			keysettingBtn[0].SetActive(false);
			keysettingBtn[1].SetActive(false);
			soundsettingObj.SetActive(true);
			optionTextObj.SetActive(false);
		}
	}

	public void SetWASD(bool input)
	{
		keysetting.Wasd = input;
		SaveManager.Instance.SaveKeySetting();
		isKeySetting = false;
		WASDKeySetting(input);
		keysettingBtn[0].SetActive(false);
		keysettingBtn[1].SetActive(false);
	}
	public void SetKeyPad(bool input)
	{
		keysetting.Numpad = input;
		SaveManager.Instance.SaveKeySetting();
		isKeySetting = false;
		keysettingBtn[0].SetActive(false);
		keysettingBtn[1].SetActive(false);
	}

	private void WASDKeySetting(bool input)
    {
		if (input)
		{
			keysetting.wasdKeyCodes[0] = (int)KeyCode.W;
			keysetting.wasdKeyCodes[1] = (int)KeyCode.S;
			keysetting.wasdKeyCodes[2] = (int)KeyCode.D;
			keysetting.wasdKeyCodes[3] = (int)KeyCode.A;
		}
		else
		{
			keysetting.wasdKeyCodes[0] = (int)KeyCode.UpArrow;
			keysetting.wasdKeyCodes[1] = (int)KeyCode.DownArrow;
			keysetting.wasdKeyCodes[2] = (int)KeyCode.LeftArrow;
			keysetting.wasdKeyCodes[3] = (int)KeyCode.RightArrow;
		}
	}

	private void SetBackGroundVolume(int num)
	{
		SoundManager.Instance.SetBgSoundVolume(num);
		if (num == 9)
		{
			soundsettingoptionTexts[0].GetComponent<Text>().text = "배경음악 - " + 100;
		}
		else
		{
			soundsettingoptionTexts[0].GetComponent<Text>().text = "배경음악 - " + (num * 10);
		}
	}
	private void SetEffectVolume(int num)
	{
		SoundManager.Instance.SetEffectSoundVolume(num);
		if (num == 9)
		{
			soundsettingoptionTexts[1].GetComponent<Text>().text = "효과음 - " + 100;
		}
		else
		{
			soundsettingoptionTexts[1].GetComponent<Text>().text = "효과음 - " + (num * 10);
		}
	}
}
