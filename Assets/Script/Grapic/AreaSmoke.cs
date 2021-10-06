using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSmoke : MonoBehaviour
{
    [SerializeField]
    private int area_index = 0;
    private PlayerMove player;
    private bool setAreaReset = false;
    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SetSmoke()
    {
        spriteRenderer.enabled = true;
    }
    private void NotSetSmoke()
    {
        spriteRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (area_index != player.nowArea)
        {
            if (!setAreaReset)
            {
                SetSmoke();
                setAreaReset = true;
            }
        }
        else
        {
            if(setAreaReset)
            {
                NotSetSmoke();
                setAreaReset = false;

            }
        }
    }
}
