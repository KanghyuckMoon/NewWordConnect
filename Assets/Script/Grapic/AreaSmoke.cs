using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreaSmoke : MonoBehaviour
{
    [SerializeField]
    private int area_index = 0;
    private PlayerMove player;
    private bool setAreaReset = true;
    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AreaSet());
    }

    private void SetSmoke()
    {
        spriteRenderer.DOColor(new Color(0.3f, 0.3f, 0.3f, 1), 1);
    }
    private void NotSetSmoke()
    {
        spriteRenderer.DOColor(new Color(1f, 1f, 1f, 0), 1);
    }

    private IEnumerator AreaSet()
    {
        while(true)
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
                if (setAreaReset)
                {
                    NotSetSmoke();
                    setAreaReset = false;

                }
            }
            yield return null;
        }
    }
}
