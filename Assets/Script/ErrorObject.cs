using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorObject : MonoBehaviour
{
    private DisplayManager display;
    private Vector2 originalPosition;

    private void Start()
    {
        display = FindObjectOfType<DisplayManager>();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x,transform.parent.position.y + (display.ay / 40));
    }
}
