using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAreaReset : MonoBehaviour
{
    //
    [SerializeField]
    private int area_index = 0;
    private PlayerMove player;
    private bool setAreaReset = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        StartCoroutine(AreaCheak());
    }

    private void AreaObjectReset()
    {
        for(int i = 0; i < transform.childCount;i++)
        {
            if(transform.GetChild(i).GetComponent<GimicBase>() != null)
            {
                transform.GetChild(i).GetComponent<GimicBase>().AreaReset();
            }
            else
            {

            }
        }
    }

    private IEnumerator AreaCheak()
    {
        while(true)
        {
            if (area_index == player.nowArea)
            {
                if (!setAreaReset)
                {
                    AreaObjectReset();
                    setAreaReset = true;
                }
            }
            else
            {
                setAreaReset = false;
            }
            yield return null;
        }
    }
}
