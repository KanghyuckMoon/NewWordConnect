using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather_Thunder : MonoBehaviour
{
    private Transform playerMove;
    private float thunderdely = 6f;
    private float currendely = 0;
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private Collider2D colliders;
    private CameraMove cameraMove;

    void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>().transform;
        cameraMove = Camera.main.GetComponent<CameraMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if(currendely < thunderdely)
        {
            currendely += Time.deltaTime;
        }
        else
        {
            ThunderEnable();
            currendely = 0;
        }
    }

    private void ThunderEnable()
    {
        spriteRenderer.sprite = sprites[0];
        spriteRenderer.enabled = true;
        colliders.enabled = false;
        transform.position = new Vector2(playerMove.position.x + Random.Range(-3f,3f), playerMove.position.y);
        Invoke("ThunderOn", 0.5f);
    }

    private void ThunderOn()
    {
        spriteRenderer.sprite = sprites[1];
        spriteRenderer.enabled = true;
        colliders.enabled = true;
        cameraMove.Shakecam(1, 0.1f);
        Invoke("ThunderOff", 0.1f);
    }    

    private void ThunderOff()
    {
        spriteRenderer.enabled = false;
        colliders.enabled = false;
    }
}
