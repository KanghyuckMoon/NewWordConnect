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
    [SerializeField]
    private bool isParticle = false;
    [SerializeField]
    private ParticleSystem particle;
    //
    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AreaSet());
        if(isParticle)
        {
            particle = GetComponent<ParticleSystem>();
        }
    }

    private void SetSmoke()
    {
        spriteRenderer.DOColor(new Color(0.3f, 0.3f, 0.3f, 1), 1);
        if(isParticle)
        particle.Play();
    }
    private void NotSetSmoke()
    {
        spriteRenderer.DOColor(new Color(1f, 1f, 1f, 0), 1);
        if (isParticle)
            particle.Stop();
    }

    private IEnumerator AreaSet()
    {
        while(true)
        {
            if(player.nowArea == -1)
            {

            }
            else if (area_index != player.nowArea)
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
