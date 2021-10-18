using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBased : WordGameObject
{
    [SerializeField]
    protected float enemymoveSpeed = 1;


    protected int nexMove = -1;

    protected float speedset = 1;
    [Header("������ �� �������� ��������")]
    [SerializeField]
    protected bool downDetectionOn = false;
    [Header("���������� ���̿� ���� ������������")]
    [SerializeField]
    protected bool downDistanceOn = false;
    [Header("���������� ���̿� ���� ���������� ��")]
    [SerializeField]
    protected float downDistanceValue = 0;
    [Header("�տ� �ִ� ���� ����")]
    [SerializeField]
    protected bool fowardDetectionOn = true;
    [Header("�տ� �ִ� ������ ��� �Ÿ��� ��������")]
    [SerializeField]
    protected float fowardDetectionValue = 0.2f;

    protected Vector2 resetPosition;
    protected bool setAreaReset = false;
    protected Animator animator;
    protected float originmass;

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        w_collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        base.Start();
        Settingvalue();
        resetPosition = transform.position;
        animator.SetFloat("Speed", speedset);
        originmass = rigid.mass;
    }

    public override void Settingvalue()
    {
        base.Settingvalue();
        speed = 1f;
    }

    private void FixedUpdate()
    {
        SetEscStop();
        if (isStop) return;
        if (setArea == -1 || setArea == player.nowArea)
        { 
            if(!setAreaReset)
            {
                ResetArea();
                ResetEnemyPosition();
                setAreaReset = true;
            }
        SetJumpDrag();
        EnemyMove();
        }
        else
        {
            setAreaReset = false;
        }
    }

    protected void ResetEnemyPosition()
    {
        rigid.velocity = Vector2.zero;
        transform.position = resetPosition;
    }

    protected virtual void EnemyMove()
    {
        rigid.velocity = new Vector2(nexMove * speedset * enemymoveSpeed, rigid.velocity.y);
        if (jumpOn) return;
        Vector2 frontvec = new Vector2(rigid.position.x + nexMove * 0.5f, rigid.position.y);
        Vector2 frontvec2 = new Vector2(rigid.position.x + nexMove * 0.5f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontvec, Vector3.down, LayerMask.GetMask("StgaePhysicsOnlyDefault"));
        RaycastHit2D rayHit2 = Physics2D.Raycast(frontvec2, nexMove == -1 ? Vector3.left : Vector3.right);
        Debug.DrawRay(frontvec, Vector3.down, new Color(0, 1, 0));
        Debug.DrawRay(frontvec2, nexMove == -1 ? Vector3.left : Vector3.right, new Color(1, 1, 0));
        if(downDetectionOn) // ������ �� ������
        {
            if (rayHit.collider == null) // �ٴڿ� �ƹ��͵� ���� ��
            {
                nexMove *= -1;
                transform.localScale = new Vector2(-nexMove, 1);

            }
            else
            {
                if (downDistanceOn) // �ٴڰŸ��� üũ�ϴ°�?
                {
                    if (rayHit.distance >= downDistanceValue)
                    {
                        nexMove *= -1;
                        transform.localScale = new Vector2(-nexMove, 1);
                    }
                    else
                    {

                    }
                }
            }
        }
        
        if (rayHit2.collider != null) // �տ� ���� �Ǵ�
        {
            if(fowardDetectionOn)
            {
                {
                    if (rayHit2.distance <= fowardDetectionValue)
                    {
                        if (rayHit2.collider.tag != "Player")
                        {
                        nexMove *= -1;
                        transform.localScale = new Vector2(-nexMove, 1);

                        }
                    }
                }
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void SpeedUp()
    {
        speedset = 1.5f;
        animator.SetFloat("Speed", speedset);
        base.SpeedUp();
    }
    public override void SpeedDown()
    {
        speedset = 0.5f;
        animator.SetFloat("Speed", speedset);
        base.SpeedDown();
    }
    public override void SpeedStop()
    {
        speedset = 0;
        animator.SetFloat("Speed", speedset);
        base.SpeedStop();
        speed = 1f;
    }
    public override void SpeedReset()
    {
        animator.SetFloat("Speed", speedset);
        speedset = 1;
        base.SpeedReset();
    }
    public override void TimeReset()
    {
        base.TimeReset();
        speedset = 1;
        animator.SetFloat("Speed", speedset);
        speed = enemymoveSpeed;
    }
    public override void SpeedStopnotinvoke()
    {
        speedset = 0;
        animator.SetFloat("Speed", speedset);
        base.SpeedStopnotinvoke();
    }

    public override void Died()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        rigid.mass = 0;
        if (isBloon)
        {
            bloon.BloonisNotJoint();
        }
    }


    public virtual void ResetArea()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
        rigid.mass = originmass;

    }
}
