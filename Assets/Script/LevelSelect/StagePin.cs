using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePin : MonoBehaviour
{
	[Header("��Ÿ �ɼ�")]
	public bool isAutomatic = false;
	[SerializeField]
	private bool HideStageIcon = false;
	[SerializeField]
	private string SceneToLoad = null;

	[Header("����Ǵ� �� ����")] //
	[SerializeField]
	private StagePin UpPin = null;
	[SerializeField]
	private StagePin DownPin = null;
	[SerializeField]
	private StagePin LeftPin = null;
	[SerializeField]
	private StagePin RightPin = null;

	public string StageName = null;

	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private int stageindex = 0;
	[SerializeField]
	private int stagestat;


	private Dictionary<Direction, StagePin> _pinDirections;

	private void Start()
	{
		_pinDirections = new Dictionary<Direction, StagePin>
		{
			{ Direction.Up, UpPin },
			{ Direction.Down, DownPin },
			{ Direction.Left, LeftPin },
			{ Direction.Right, RightPin }
		};

		// Hide the icon if needed
		if (HideStageIcon)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}
		GetComponent<SpriteRenderer>().sprite = sprites[stagestat];
	}

	public int ReturnStat()
    {
		return stagestat = SaveManager.Instance.CurrentSaveUser.stageClear[stageindex];
    }

	public StagePin GetPinInDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return UpPin;
			case Direction.Down:
				return DownPin;
			case Direction.Left:
				return LeftPin;
			case Direction.Right:
				return RightPin;
			default:
				throw new Exception("����� ���� �����ϴ�");
		}
	}

	public StagePin GetNextPin(StagePin pin)
	{
		return _pinDirections.FirstOrDefault(x => x.Value != null && x.Value != pin).Value;
	}

	private void OnDrawGizmos()
	{
		if (UpPin != null) DrawLine(UpPin);
		if (RightPin != null) DrawLine(RightPin);
		if (DownPin != null) DrawLine(DownPin);
		if (LeftPin != null) DrawLine(LeftPin);
	}

	protected void DrawLine(StagePin pin)
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, pin.transform.position);
	}
}
