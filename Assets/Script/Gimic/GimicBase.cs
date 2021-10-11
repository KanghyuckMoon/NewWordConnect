using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicBase : MonoBehaviour
{
    public float realSpeed = 1;
    protected Animator animator;
    [SerializeField]
    private string animationName = null;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    public virtual void SetGimicSpeed(float speed)
    {
        realSpeed = speed;
        animator.SetFloat(animationName, realSpeed);
    }

    public virtual void AreaReset()
    {
        gameObject.SetActive(true);
    }
}
