using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicSpring : GimicBase
{
    public void SpringTread()
    {
        animator.Play("SpringMove");
    }
}
