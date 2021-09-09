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
    protected float realspeed;

    //날씨 속도
    public float wheather_WindSpeed;
    public float wheather_WindAirSpeed;

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
    public bool jumpOn;

    public bool w_BlockOn = false;

    [SerializeField]
    private float w_Movetime = 0f;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    //공용
    protected Rigidbody2D rigid = null;
    protected int sizeIndex = 0;

    //1초동안 정지한다
    private bool w_pause;
    private Vector2 pausevector;

    //블럭을 밟을 때 마다
    public float w_tile = 0;
    protected float w_vector1 = 0;

    //콜라이더
    protected Collider2D w_collider;

    //떨어진다
    [SerializeField]
    protected bool superDownOn = false;

    private void Awake()
    {
        Save_Path = Application.persistentDataPath + "/Save";
        //�ȵ���̵� ��忡����  Application.dataPath ��ſ� Application.persistentDataPath
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        } if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rigid = GetComponent<Rigidbody2D>();
        }
        else
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        w_collider = GetComponent<Collider2D>();
        LoadToJson();
        Setting();
    }

    protected virtual void Start()
    {
        realspeed = speed;
        StartCoroutine(OnMoveDetect());
        realspeed = speed;
    }

    public virtual void Setting()
    {
        if (w_pause) return;
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
    protected virtual void JumpDrag()
    {
        if (jumpOn)
        {
            rigid.drag = airfriction; // 마찰력 설정
        }
        else
        {
            rigid.drag = friction; // 공기 마찰력 설정
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
            if ( !(rigid.velocity.x > -0.0002191039 * 10 && rigid.velocity.x < 0.0002191039 * 10) || rigid.velocity.y != 0)
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
    public virtual void Down()
    {
        rigid.AddForce(Vector2.down * (jump * 0.8f), ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
    }
    public virtual void SuperDown()
    {
        //rigid.AddForce(Vector2.down * (jump * 0.8f), ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
        superDownOn = true;
    }

    public virtual void SizeUp()
    {
        if(sizeIndex == 0)
        {
            sizeIndex = 1;
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            transform.localScale = new Vector2(1.4f, 1.4f);
        }
        else if(sizeIndex == -1)
        {
            sizeIndex = 0;
            transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            transform.localScale = new Vector2(0.8f, 0.8f);
        }
        
    }

    public virtual void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            transform.localScale = new Vector2(0.6f, 0.6f);
        }
    }

    public virtual void SpeedUp()
    {
        realspeed = speed * 2f;
        Invoke("SpeedReset", 1f);
    }
    public virtual void SpeedDown()
    {
        realspeed = speed * 0.5f;
        Invoke("SpeedReset", 1f);
    }
    public virtual void SpeedStop()
    {
        w_pause = true;
        realspeed = 0;
        gravityScale = 0;
        jump = 0;
        rigid.gravityScale = 0;
        pausevector = rigid.velocity;
        rigid.velocity = Vector2.zero;
        realspeed = 0;
        Invoke("TimeReset", 1f);
    }
    public virtual void SpeedStopnotinvoke()
    {
        w_pause = true;
        realspeed = 0;
        gravityScale = 0;
        jump = 0;
        rigid.gravityScale = 0;
        pausevector = rigid.velocity;
        rigid.velocity = Vector2.zero;
        realspeed = 0;
    }
    public virtual void SpeedReset()
    {
        realspeed = speed;
    }
    public virtual void TimeReset()
    {
        w_pause = false;
        realspeed = speed;
        rigid.velocity = pausevector;
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
        superDownOn = false;
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

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wind"))
        {
            rigid.AddForce(Vector2.right * 2f);
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
