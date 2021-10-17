using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	private RectTransform[] optionTexts;
	[SerializeField]
	private RectTransform selectImage;
	private int optionselect = 0;

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
	}

	private void Update()
	{
		// Only check input when character is stopped
		if (Character.isMoving) return;
		InputEsc();
		if (isEsc) return;

		// First thing to do is try get the player input
		CheckForInput();
	}

	private void CheckForInput()
	{
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			Character.TrySetDirection(Direction.Up);
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			Character.TrySetDirection(Direction.Down);
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			Character.TrySetDirection(Direction.Left);
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
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
			if(Input.GetKeyDown(KeyCode.W))
            {
				optionselect--;
				if (optionselect < 0)
                {
					optionselect = 0;
					return;
				}
				MoveOptionSelect();
			}
			else if(Input.GetKeyDown(KeyCode.S))
			{
				optionselect++;
				if (optionselect > optionTexts.Length - 1)
				{
					optionselect = optionTexts.Length - 1;
					return;
				}
				MoveOptionSelect();
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
		selectImage.DOAnchorPosY(optionTexts[optionselect].anchoredPosition.y,0.2f);
		switch(optionselect)
        {
			case 2:
				selectImage.DOAnchorPosX(-130, 0.2f);
				break;
			case 3:
				selectImage.DOAnchorPosX(-220, 0.2f);
				break;
			default:
				selectImage.DOAnchorPosX(-116, 0.2f);
				break;
        }
    }
}
