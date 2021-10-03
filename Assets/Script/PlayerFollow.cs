using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField]
    private bool horizontalOn;
    [SerializeField]
    private Vector2 LockVector2;


    [SerializeField]
    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().transform;
    }

    private void Update()
    {
        if(horizontalOn)
        {
            transform.position = new Vector2(LockVector2.x, player.position.y);
        }
        else
        {
            transform.position = new Vector2(player.position.x, LockVector2.y);
        }
    }

    public void SetPosition(Vector2 vetor2)
    {
        LockVector2 = new Vector2(vetor2.x, vetor2.y);
    }
}
