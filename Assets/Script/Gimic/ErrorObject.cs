using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorObject : MonoBehaviour
{
    private DisplayManager display;
    private Vector2 originalPosition;
    private Camera mainCamera = null;

    private void Start()
    {
        originalPosition = transform.position;
        display = FindObjectOfType<DisplayManager>();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3((transform.parent.position.x + originalPosition.x) + (display.ax * 0.005f), (transform.parent.position.y + originalPosition.y) + (display.ay * 0.005f),10);
        transform.localScale = new Vector2(display.scaleVector.x, display.scaleVector.y);
        //transform.position = Camera.main.WorldToViewportPoint(-originalPosition);

    }
}
