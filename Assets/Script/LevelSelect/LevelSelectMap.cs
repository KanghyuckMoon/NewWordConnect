using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMap : MonoBehaviour
{
	public StageCharacter Character;
	public StagePin StartPin;
	[SerializeField]
	private Text SelectedNameText;
	[SerializeField]
	private Text ClearText;


	private void Start()
	{
		// Pass a ref and default the player Starting Pin
		Character.Initialise(this, StartPin);
	}

	private void Update()
	{
		// Only check input when character is stopped
		if (Character.isMoving) return;

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
}
