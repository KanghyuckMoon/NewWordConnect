using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCharacter : MonoBehaviour
{
    public bool isMoving;
    public float Speed = 3f;

    public StagePin CurrentPin { get; private set; }
    private StagePin targetPin;
    private string inStageName;
    private MapManager mapManager;

    public void Initialise(MapManager mapManager, StagePin startPin)
    {
        this.mapManager = mapManager;
        SetCurrentPin(startPin);
    }

    private void Update()
    {
        if (targetPin == null) return;

        // Get the characters current position and the targets position
        var currentPosition = transform.position;
        var targetPosition = targetPin.transform.position;

        // If the character isn't that close to the target move closer
        if (Vector3.Distance(currentPosition, targetPosition) > .02f)
        {
            transform.position = Vector3.MoveTowards(
                currentPosition,
                targetPosition,
                Time.deltaTime * Speed
            );
        }
        else
        {
            if (targetPin.isAutomatic)
            {
                var pin = targetPin.GetNextPin(CurrentPin);
                MoveToPin(pin);
            }
            else
            {
                SetCurrentPin(targetPin);
            }
        }
    }

    public void TrySetDirection(Direction direction)
    {
        var pin = CurrentPin.GetPinInDirection(direction);
        if (pin == null) return;
        if (pin.ReturnStat() == 0) return;
        MoveToPin(pin);
    }

    private void MoveToPin(StagePin pin)
    {
        targetPin = pin;
        isMoving = true;
    }

    public void SetCurrentPin(StagePin pin)
    {
        CurrentPin = pin;
        targetPin = null;
        inStageName = pin.StageName;
        transform.position = pin.transform.position;
        isMoving = false;
        mapManager.SetStageName(pin.StageName);
    }

    public void StageMoveScene()
    {
        SceneManager.LoadScene(inStageName);
    }
}
