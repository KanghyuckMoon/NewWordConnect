using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //�������� ��������
    [SerializeField]
    protected PlayerSetting user = null;
    protected string Save_Path = "";
    protected string Save_FileName = "/MoveFile.txt";


    protected float speed;
    protected float maxSpeed;
    protected float friction;
    protected float airfriction;
    protected bool downGravityOn;
    protected float jump;
    protected float gravityScale;

    protected float w_speed = 1;
    protected float w_size = 1;
    protected bool w_notcollider = false;
    public bool w_visible = false;
    public bool w_visibleEffect = false;
    public bool w_MoveOn = false;
    public bool w_MoveOnEffect = false;
    public bool w_Collider = false;
    public bool w_ColliderEffect = false;
    public bool w_ColliderOn = false;


    public bool w_BlockOn = false;

    [SerializeField]
    private float w_Movetime = 0f;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    //����
    protected Rigidbody2D rigid = null;

    //1�ʵ��� �����Ѵ�
    private bool pause;
    private float cooltime;
    private float gravityscale;
    private Vector2 pausevector;

    //���� ���� �� ����
    public float w_tile = 0;
    protected float w_vector1 = 0;


    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        LoadToJson();
        Setting();
        StartCoroutine(OnMoveDetect());
    }

    public virtual void Setting()
    {
        speed = user.speed;
        maxSpeed = user.maxspeed;
        friction = user.friction;
        airfriction = user.aitfriction;
        downGravityOn = user.downGravityOn;
        gravityScale = user.gravityScale;
        jump = user.jump;
        rigid.gravityScale = gravityScale;
    }

    public virtual void LoadToJson()
    {
        if (File.Exists(Save_Path + Save_FileName))
        {
            string json = File.ReadAllText(Save_Path + Save_FileName);
            user = JsonUtility.FromJson<PlayerSetting>(json);
        }
    }


    protected IEnumerator OnMoveDetect()
    {
        while(true)
        {
            if (w_Movetime < 0.02f)
            {
                w_Movetime += Time.deltaTime;

            }
            else
            {
                w_MoveOn = false;
                w_MoveOnEffect = true;
            }
            if ( !(rigid.velocity.x > -1.502492e-05 * 10 && rigid.velocity.x < 1.502492e-05 * 10) || rigid.velocity.y != 0)
            {
                    w_MoveOn = true;
                    w_Movetime = 0f;
            }

            yield return waitForSeconds;
        }
        
    }

    //�����

    public virtual void Jump()
    {
        rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
    }

    public virtual void TimePause()
    {
        pausevector = rigid.velocity;//���� �ӵ� ����
        w_speed = 0;//�̵��ӵ� 0
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
        rigid.gravityScale = 0; // �߷� ũ�� 0
        rigid.velocity = Vector2.zero;//���� �ӵ� 0���� ����
        Invoke("TimeCountinue", 1);
        
    }
    public virtual void TimeCountinue()
    {
        rigid.velocity = pausevector; //�ӵ� �Ҿ����
        w_speed = 1;//�̵��ӵ� ����
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
    }

    public void OnBecameVisible()
    {
        w_visible = true;
    }
    public void OnBecameInvisible()
    {
        w_visible = false;
        w_visibleEffect = false;
    }

    public float ReturnVelocityY()
    {
        return rigid.velocity.y;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = false;
        w_tile = 0;
        w_vector1 = transform.position.x;
        w_BlockOn = true;
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = true;
        if(w_BlockOn)
        {
            w_tile = Mathf.Abs(w_vector1 - transform.position.x);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        w_Collider = false;
        w_ColliderEffect = false;
        w_BlockOn = false;
        w_tile = 0;
    }

    public void SetCollider()
    {
        w_ColliderEffect = true;
    }

    public void SetMoveZero()
    {
        w_BlockOn = false;
        w_tile = 0;
    }
}
