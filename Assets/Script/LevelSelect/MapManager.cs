using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class MapManager : MonoBehaviour
{
	public StageCharacter Character;
	public StagePin StartPin;
	private SaveUser saveUser;
	[SerializeField]
	private Text stagenameText;

	private void Start()
	{
		saveUser = SaveManager.Instance.CurrentSaveUser;
		Character.Initialise(this, StartPin); // ĳ���� ��ġ ����
	}

	private void Update()
	{
		if (Character.isMoving) return; // �̵� ���̸� �� ����

		CheckForInput(); // �Է� ����
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
}
