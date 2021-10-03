using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicBloon : MonoBehaviour
{
    [SerializeField]
    private float plusY; // 최대 상승 높이
    [SerializeField]
    private float upSpeed = 1; // 올라가는 속도
    private float maxPlusY;
    private Vector2 originalPosition;
    private Rigidbody2D rigid;
    private PlayerMove player;
    private DistanceJoint2D distanceJoint2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int setArea = 0;
    private bool setAreaReset = false;
    private bool bloonboom = false;
    private LineRenderer lineRenderer = null;
    [SerializeField]
    private Transform isJointObject;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        distanceJoint2D.connectedBody = isJointObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        player = FindObjectOfType<PlayerMove>();
        maxPlusY = plusY;
    }

    private void FixedUpdate()
    {
        if (setArea == -1 || setArea == player.nowArea)
        {
            if (!setAreaReset)
            {
                setAreaReset = true;
                ResetArea();
            }
            if (bloonboom) return;
            BloonMove();
            LineDraw();
        }
        else
        {
            setAreaReset = false;
        }

    }

    private void BloonMove()
    {
        if (originalPosition.y + plusY > transform.position.y)
        {
            rigid.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        }
    }

    private void LineDraw()
    {
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1, isJointObject.position);
    }

    public void BloonBoom()
    {
        distanceJoint2D.enabled = false;
        spriteRenderer.enabled = false;
        lineRenderer.enabled = false;
        bloonboom = true;
        rigid.gravityScale = 0;
    }

    public void BloonisNotJoint()
    {
        plusY += 10;
        Invoke("BloonBoom",1);
    }

    public void ResetArea()
    {
        transform.position = originalPosition;
        distanceJoint2D.enabled = true;
        spriteRenderer.enabled = true;
        lineRenderer.enabled = true;
        bloonboom = false;
        plusY = maxPlusY;
    }

    public float force = 100f;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        GameObject target = collision2D.gameObject;

        Vector3 inNormal = Vector3.Normalize(
            transform.position - target.transform.position);
        rigid.AddForce(inNormal * force, ForceMode2D.Impulse);
    }

}
