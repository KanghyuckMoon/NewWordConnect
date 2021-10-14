using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimicReturnMovingTile : GimicBase
{
    [SerializeField]
    private float returnSpeed;
    [SerializeField]
    private Vector2 originalPosition = Vector2.zero;
    private bool returnOn;
    private float returndely = 0;
    [SerializeField]
    private float returndistance = 2;
    
    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
        realSpeed = 1;
        StartCoroutine(CheakPosition());
    }
    private IEnumerator CheakPosition()
    {
        while(true)
        {
            if(originalPosition != (Vector2)transform.position && (returndely <= 0 || Vector2.Distance(originalPosition,(Vector2)transform.position) > returndistance))
            {
                returndely = 10;
                transform.DOMove(originalPosition, 1);
            }
            returndely -= Time.deltaTime;
            yield return null;
        }    
    }
}
