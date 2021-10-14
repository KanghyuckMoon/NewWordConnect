using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicRotation : GimicBase
{
    [SerializeField]
    private float rotationSpeed;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Rotation());
    }

    public override void SetGimicSpeed(float speed)
    {
        realSpeed = speed;
        //animator.SetFloat(animationName, realSpeed);
    }

    private IEnumerator Rotation()
    {
        Vector3 onrotation = transform.eulerAngles;
        while (true)
        {
            onrotation.z += realSpeed * rotationSpeed;
            transform.eulerAngles = onrotation;
            yield return null;
        }
    }
}
