using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicBlock : GimicBase
{
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;
    private Collider2D[] colliders;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        particle = GetComponent<ParticleSystem>();

    }

    public void BreakBlock()
    {
        spriteRenderer.enabled = false;
        colliders[0].enabled = false;
        colliders[1].enabled = false;
        particle.Play();
        Invoke("SetActiveFalse", 1f);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public override void AreaReset()
    {
        base.AreaReset();
        spriteRenderer.enabled = true;
        colliders[0].enabled = true;
        colliders[1].enabled = true;
    }
}
