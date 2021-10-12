using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicPassWord : GimicBase
{
    [SerializeField]
    private List<int> passwordcolor;
    private int nowpassword = 0;
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
        ChangeColor();
    }

    public void ChangeColor()
    {
        nowpassword++;
        if (nowpassword > 6) nowpassword = 0;
        switch(nowpassword)
        {
            case 0:
                spriteRenderer.color = new Color(1, 0, 0, 1);
                break;
            case 1:
                spriteRenderer.color = new Color(1, 0.5f, 0, 1);
                break;
            case 2:
                spriteRenderer.color = new Color(1, 1, 0, 1);
                break;
            case 3:
                spriteRenderer.color = new Color(0, 1, 0, 1);
                break;
            case 4:
                spriteRenderer.color = new Color(0, 1, 1, 1);
                break;
            case 5:
                spriteRenderer.color = new Color(0, 0, 1, 1);
                break;
            case 6:
                spriteRenderer.color = new Color(0.5f, 0, 1, 1);
                break;
        }
        CheakColor();
    }

    public void CheakColor()
    {

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
