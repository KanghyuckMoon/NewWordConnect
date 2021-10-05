using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSmoke : MonoBehaviour
{
    [SerializeField]
    private int area_index = 0;
    private PlayerMove player;
    private bool setAreaReset = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    private void SetSmoke()
    {

    }
    private void NotSetSmoke()
    {
        
    }

    private void FixedUpdate()
    {
        if (area_index == player.nowArea)
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
