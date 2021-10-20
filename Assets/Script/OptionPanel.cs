using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;

public class OptionPanel : MonoBehaviour
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

	private enum OptionType
    {
		MainTitle,
		InGame,
		StageSelect
    }

	private KeySetting keysetting;
	private SaveUser saveuser;

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
	private OptionType optionType;

	//사운드 관련
	[SerializeField]
	private GameObject soundsettingObj;
	[SerializeField]
	private RectTransform[] soundsettingoptionTexts;

	[SerializeField]
	private RectTransform selectImage;

	private int optionselect = 0;
	private int nowoptionselect = -1;
	private bool isKeySetting;
	public bool isEnabled;
	public bool isDontNum;

    private void Start() //데이터를 가져옴
    {
		keysetting = SaveManager.Instance.CurrenKeySetting;
		if(keysetting == null)
        {
			if (File.Exists(Application.dataPath + "/Save/KeySettingFile.txt"))
			{
				string json = File.ReadAllText(Application.dataPath + "/Save/KeySettingFile.txt");
				keysetting = JsonUtility.FromJson<KeySetting>(json);
			}
		}
		saveuser = SaveManager.Instance.CurrentSaveUser;
		MoveOptionSelect();
		OptionObjSetActive();
		WASDKeySetting(keysetting.Wasd);
	}

	public bool ReturnToisKeySetting()
	{
		return isKeySetting;
	}
	public bool ReturnToisDontNum()
	{
		return isDontNum;
	}

    private void Update()
    {
		InputEsc();
    }

    private void InputEsc()
	{
		if (isEnabled)
		{
			if (Input.GetKeyDown((KeyCode)keysetting.wasdKeyCodes[0])) // W
			{
				optionselect--;
				if (optionselect < 0)
				{
					optionselect = 0;
					return;
				}
				MoveOptionSelect();
			}
			else if (Input.GetKeyDown((KeyCode)keysetting.wasdKeyCodes[1])) // S
			{
				optionselect++;
				if (nowoptionselect == -1)
				{
					if (optionselect > optionTexts.Length - 1)
					{
						optionselect = optionTexts.Length - 1;
						return;
					}
				}
				else if (nowoptionselect == 0)
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
			else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				if (nowoptionselect == -1)
				{
					isDontNum = false;
					nowoptionselect = optionselect;
					optionselect = 0;
					if (nowoptionselect == 1) isDontNum = true;
					OptionObjSetActive();
					MoveOptionSelect();
				}
				else if (nowoptionselect == 0)
				{
					isDontNum = false;
					OptionObjSetActive();
					MoveOptionSelect();
					if (optionselect == 0)
					{
						keysettingBtn[0].SetActive(true);
						keysettingBtn[1].SetActive(false);
						isKeySetting = true;
					}
					else if (optionselect == 1)
					{
						keysettingBtn[0].SetActive(false);
						keysettingBtn[1].SetActive(true);
						isKeySetting = true;
					}
					optionselect = 0;
				}
				else if (nowoptionselect == 1)
				{
					isDontNum = true;
				}
				else if (nowoptionselect == 2)
				{
					if(optionType == OptionType.InGame)
					{
						SceneManager.LoadScene("StageSelect");
					}
					else
                    {
						SceneManager.LoadScene("MainTitle");
                    }
				}
				else if (nowoptionselect == 3)
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
						if (nowoptionselect == 1)
						{
							if (optionselect == 0)
							{
								SetBackGroundVolume(i);
							}
							else
							{
								SetEffectVolume(i);
							}
						}
					}
				}
			}
		}
	}

	private void MoveOptionSelect()
	{
		if (nowoptionselect == -1)
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
		if (nowoptionselect == -1)
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

	public void SetWASD(bool input) //WASD 버튼에 넣는다
	{
		keysetting.Wasd = input;
		SaveManager.Instance.SaveKeySetting();
		isKeySetting = false;
		WASDKeySetting(input);
		keysettingBtn[0].SetActive(false);
		keysettingBtn[1].SetActive(false);
	}
	private void WASDKeySetting(bool input) //WASD 키코드 설정
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
	public void SetKeyPad(bool input) // 키패드 설정
	{
		keysetting.Numpad = input;
		SaveManager.Instance.SaveKeySetting();
		isKeySetting = false;
		keysettingBtn[0].SetActive(false);
		keysettingBtn[1].SetActive(false);
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
