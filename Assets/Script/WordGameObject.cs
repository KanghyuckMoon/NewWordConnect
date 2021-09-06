using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //물리설정 가져오기
    [SerializeField]
    protected PlayerSetting user = null;
    protected string Save_Path = "";
    protected string Save_FileName = "/MoveFile.txt";


    [SerializeField]
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

    //공용
    protected Rigidbody2D rigid = null;

    //1초동안 정지한다
    private bool pause;
    private float cooltime;
    private float gravityscale;
    private Vector2 pausevector;

    //블럭을 밟을 때 마다
    public float w_tile = 0;
    protected float w_vector1 = 0;

    private float realspeed = 0; //���� �̵��ӵ�

    //콜라이더
    protected Collider2D w_collider;

    private void Awake()
    {
        Save_Path = Application.dataPath + "/Save";
        //�ȵ���̵� ��忡����  Application.dataPath ��ſ� Application.persistentDataPath
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
        w_collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
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

    //실행어

    public virtual void Jump()
    {
        rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
    }

    public virtual void ColliderOff()
    {
        w_collider.enabled = false;
        Invoke("ColliderOn",1f);
    }
    public virtual void ColliderOn()
    {

        w_collider.enabled = true;
    }

    public virtual void TimePause()
    {
        pausevector = rigid.velocity;//현재 속도 저장
        w_speed = 0;//이동속도 0
        gravityscale = rigid.gravityScale; //중력 크기 저장
        rigid.gravityScale = 0; // 중력 크기 0
        rigid.velocity = Vector2.zero;//현재 속도 0으로 만듬
        Invoke("TimeCountinue", 1);
        
    }
    public virtual void TimeCountinue()
    {
        rigid.velocity = pausevector; //속도 불어오기
        w_speed = 1;//이동속도 정상
        gravityscale = rigid.gravityScale; //중력 크기 저장
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
