using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class MapManager : MonoBehaviour
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
	private SaveUser saveUser;
	private KeySetting keysetting;
	private int optionselect = 0;
	[SerializeField]
	private RectTransform optionarrow = null;
	[SerializeField]
	private RectTransform[] optionRectTexts = null;
	[SerializeField]
	private Text[] optionTexts = null;

	[SerializeField]
	private Text stagenameText;
	[SerializeField]
	private GameObject escSystem;

	private bool isEsc = false;

	private void Start()
	{
		saveUser = SaveManager.Instance.CurrentSaveUser;
		keysetting = SaveManager.Instance.CurrenKeySetting;
		Character.Initialise(this, StartPin); // 캐릭터 위치 설정
	}

	private void Update()
	{
		if (Character.isMoving) return; // 이동 중이면 안 받음
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isEsc = !isEsc;
			escSystem.SetActive(isEsc);
		}
		if(isEsc)
        {
		OptionInput();

        }
		if (isEsc) return;

		CheckForInput(); // 입력 받음
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
			Character.StageMoveScene();
        }
	}

	public void SetStageName(string name)
    {
		stagenameText.text = name;
    }

	private void OptionInput()
	{
		for (int i = 0; i < keyCodes.Length; i++)
		{
			if (Input.GetKeyDown(keyCodes[i] - (keysetting.Numpad ? 0 : 208)))
			{
				if (optionselect == 2) SetBackGroundVolume(i);
				if (optionselect == 3)
				{
					SetEffectVolume(i);
					SoundManager.Instance.SFXPlay(2);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			OptionSelect(-1);
			MoveOption();
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			OptionSelect(1);
			MoveOption();
		}
	}

	private void SetTextWASD()
	{
		optionTexts[0].text = "WASD/방향키 - " + (keysetting.Wasd ? "WASD" : "방향키");
	}

	private void SetTextNumpad()
	{
		optionTexts[1].text = "넘패드/키패드 - " + (keysetting.Numpad ? "넘패드" : "키패드");
	}

	private void SetBackGroundVolume(int num)
	{
		SoundManager.Instance.SetBgSoundVolume(num);
		if (num == 9)
		{
			optionTexts[2].text = "배경음악 - " + 100;
		}
		else
		{
			optionTexts[2].text = "배경음악 - " + (num * 10);
		}
	}
	private void SetEffectVolume(int num)
	{
		SoundManager.Instance.SetEffectSoundVolume(num);
		if (num == 9)
		{
			optionTexts[3].text = "효과음 - " + 100;
		}
		else
		{
			optionTexts[3].text = "효과음 - " + (num * 10);
		}
	}
	private void MoveOption()
	{
		optionarrow.DOAnchorPosY(optionRectTexts[optionselect].anchoredPosition.y, 0.2f);
	}

	private void OptionSelect(int i)
	{
		if (optionselect + i < 0) return;
		if (optionselect + i > optionRectTexts.Length - 1) return;
		optionselect += i;
	}
}
