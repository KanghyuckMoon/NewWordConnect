using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicBase : MonoBehaviour
{
    public float realSpeed = 1;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    private string animationName = null;
    [SerializeField]
    private bool isanimation = true;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public virtual void SetGimicSpeed(float speed)
    {
        realSpeed = speed;
        if(isanimation)
        {
        animator.SetFloat(animationName, realSpeed);
        }
    }

    public virtual void AreaReset()
    {
        gameObject.SetActive(true);
    }

    public virtual void SetMaterial(Material material)
    {
        spriteRenderer.material = material;
    }
}
