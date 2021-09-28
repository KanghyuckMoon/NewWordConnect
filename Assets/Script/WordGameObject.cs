using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    protected string Save_Path = "";
    protected string Save_FileName = "/MoveFile.txt";



    //물리 변수
    protected float speed = 24.5f;
    protected float maxSpeed = 27.5f;
    protected float friction = 7.5f;
    protected float airfriction = 4.0f;
    protected bool downGravityOn = true;
    protected float jump = 22.5f;
    protected float gravityScale = 4.300000190734863f;
    protected float realspeed;

    //날씨 속도
    protected float wheather_WindSpeed;
    protected float wheather_WindAirSpeed;


    //변수들
    protected float w_speed = 1;
    protected float w_size = 1;
    protected bool w_notcollider = false;

    protected bool w_visible = false;
    public bool W_Visible {get { return w_visible; } }

    protected bool w_visibleEffect = false;
    public bool W_VisibleEffect { get { return w_visibleEffect; } }
    public void W_VisibleEffectOntrue() { w_visibleEffect = true;}

    protected bool w_MoveOn = false;
    public bool W_MoveOn { get { return w_MoveOn; } }

    protected bool w_MoveOnEffect = false;
    public bool W_MoveOnEffect { get { return w_MoveOnEffect; } }

    protected bool w_Collider = false;
    public bool W_Collider { get { return w_Collider; } }

    protected bool w_ColliderEffect = false;
    public bool W_ColliderEffect { get { return w_ColliderEffect; } }

    protected bool w_ColliderOn = false;
    public bool W_ColliderOn { get { return w_ColliderOn; } }

    protected bool jumpOn;
    public bool JumpOn { get { return jumpOn; } }

    protected bool w_BlockOn = false;
    public bool W_BlockOn { get { return w_BlockOn; } }

    [SerializeField]
    protected float w_Movetime = 0f;
    protected WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    //공용
    protected Rigidbody2D rigid = null;
    protected int sizeIndex = 0;

    //1초동안 정지한다
    private bool w_pause;
    private Vector2 pausevector;

    //블럭을 밟을 때 마다
    protected float w_tile = 0;
    public float W_Tile { get { return w_tile; } }
    protected float w_vector1 = 0;

    //콜라이더
    protected Collider2D w_collider;

    //떨어진다
    protected bool superDownOn = false;

    protected PlayerMove player;
    //private void Awake()
    //{
    //    //StartSetJsonSetting();
    //}

    //구역 설정
    [Header ("구역 설정")]
    public int setArea = -1;

    public bool isSound = false;
    public bool isSoundEffect = true;

    public void SetPlayer(PlayerMove player)
    {
        this.player = player;
    }
    public void SetPlayer()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    private void StartSetJsonSetting()
    {
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rigid = GetComponent<Rigidbody2D>();
        }
        else
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        w_collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        realspeed = speed;
        StartCoroutine(OnMoveDetect());
        SetPlayer();
    }

    public virtual void Settingvalue()
    {
     speed = 24.5f;
     maxSpeed = 27.5f;
     friction = 7.5f;
     airfriction = 4.0f;
     downGravityOn = true;
     jump = 22.5f;
     gravityScale = 4.300000190734863f;
     rigid.gravityScale = gravityScale;
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


    protected virtual IEnumerator OnMoveDetect()
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
        if(setArea == -1 || setArea == player.nowArea)
        {
            rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            w_MoveOn = true;
            w_MoveOnEffect = false;
            w_tile = 0;
            PlaySound();
        }
    }
    public virtual void Down()
    {
        rigid.AddForce(Vector2.down * (jump * 0.8f), ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
        PlaySound();
    }
    public virtual void SuperDown()
    {
        rigid.AddForce(Vector2.down * (jump * 0.01f), ForceMode2D.Impulse);
        w_MoveOn = true;
        w_MoveOnEffect = false;
        superDownOn = true;
        PlaySound();
        Invoke("SuperDownFalse",0.5f);
    }
    public virtual void SuperDownFalse()
    {
        superDownOn = false;
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
        PlaySound();
        Invoke("SpeedReset", 1f);
    }
    public virtual void SpeedDown()
    {
        realspeed = speed * 0.5f;
        PlaySound();
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
        PlaySound();
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
        gravityScale = 4.300000190734863f;
        jump = 22.5f;
        rigid.gravityScale = 4.300000190734863f;
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

    public virtual float ReturnVelocityY()
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
        jumpOn = false;
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
        jumpOn = true;
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

    public float ReturnRealSpeed()
    {
        return realspeed;
    }

    protected void PlaySound()
    {
        isSound = true;
        Invoke("ResetSound", 1f);
    }

    protected void ResetSound()
    {
        isSound = false;
        isSoundEffect = true;
    }
}
