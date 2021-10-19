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

	public string StageSceneName = null;
	public string ViewStageName = null;

	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private int stageindex = 0;
	[SerializeField]
	private int stagestat;
	[SerializeField]
	private Material material;

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
		stagestat = SaveManager.Instance.CurrentSaveUser.isstageClears[stageindex];
		if (HideStageIcon)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}
		GetComponent<SpriteRenderer>().sprite = sprites[stagestat];
		if(stagestat > 1)
		{
			int index = 0;
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.material = material;
			lineRenderer.SetWidth(0.5f, 0.5f);
			lineRenderer.positionCount--;
			lineRenderer.SetPosition(index, new Vector2(transform.position.x,transform.position.y - 0.5f));
			if (UpPin != null)
			{
				LineRender(ref index, lineRenderer,UpPin);
			}
			if (DownPin != null)
			{
				LineRender(ref index, lineRenderer,DownPin);
			}
			if (RightPin != null)
			{
				LineRender(ref index, lineRenderer,RightPin);
			}
			if (LeftPin != null)
			{
				LineRender(ref index, lineRenderer,LeftPin);
			}

		}
	}

	private void LineRender(ref int index, LineRenderer lineRenderer, StagePin pin)
	{
		index++;
		lineRenderer.positionCount++;
		lineRenderer.SetPosition(index, new Vector2(pin.transform.position.x, pin.transform.position.y - 0.5f));
		index++;
		lineRenderer.positionCount++;
		lineRenderer.SetPosition(index, new Vector2(transform.position.x, transform.position.y - 0.5f));
	}

	public int ReturnStat()
    {
		return stagestat;
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
