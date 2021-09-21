using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class StagePin : MonoBehaviour
{
	[Header("기타 옵션")]
	public bool isAutomatic = false;
	[SerializeField]
	private bool HideStageIcon = false;
	[SerializeField]
	private string SceneToLoad = null;

	[Header("연결되는 핀 설정")] //
	[SerializeField]
	private StagePin UpPin = null;
	[SerializeField]
	private StagePin DownPin = null;
	[SerializeField]
	private StagePin LeftPin = null;
	[SerializeField]
	private StagePin RightPin = null;

	public string StageName = null;

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
				throw new Exception("연결된 핀이 없습니다");
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
