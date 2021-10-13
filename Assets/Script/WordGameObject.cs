using System.Collections;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //물리 변수
    [SerializeField]
    protected float speed = 24.5f;
    protected float maxSpeed = 27.5f;
    protected float friction = 7.5f;
    protected float airfriction = 4.0f;
    protected bool downGravityOn = true;
    protected float jump = 22.5f;
    protected float gravityScale = 4.3f;
    protected float realspeed;

    //날씨 속도
    protected float wheather_WindSpeed;
    protected float wheather_WindAirSpeed;


    //변수들
    protected float w_speed = 1;
    protected float w_size = 1;
    protected bool w_notcollider = false;

    protected bool w_visible = false;
    public bool W_Visible { get { return w_visible; } }

    protected bool w_visibleEffect = false;
    public bool W_VisibleEffect { get { return w_visibleEffect; } }
    public void W_VisibleEffectOntrue() { w_visibleEffect = true; }

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
    protected Vector2 scaleVetor = new Vector2(1, 1);

    //공용
    protected Rigidbody2D rigid = null;
    protected int sizeIndex = 0;

    //1초동안 정지한다
    protected bool w_pause;
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
    protected WordManager wordManager;

    //구역 설정
    [Header("구역 설정")]
    public int setArea = -1;

    public bool isSound = false;
    public bool isSoundEffect = true;

    //풍선설정
    public bool isBloon = false;
    [SerializeField]
    protected GimicBloon bloon;

    public bool isStop = false;
    protected Vector2 stopVector = Vector2.zero;

    protected SpriteRenderer spriteRenderer;

    //용사,모든적,기믹인지
    [SerializeField]
    private bool isObject = false;

    public void SetPlayer(PlayerMove player)
    {
        this.player = player;
    }
    public void SetPlayer()
    {
        player = FindObjectOfType<PlayerMove>();
        wordManager = FindObjectOfType<WordManager>();
    }

    //메시지

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        realspeed = speed;
        StartCoroutine(OnMoveDetect());
        SetPlayer();
        if (isObject)
        {
            Settingvalue();
        }
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
        if (collision.gameObject.CompareTag("BreakBlock"))
        {
            CollisionBreakBlock(collision.gameObject);
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BreakBlock"))
        {
            TriggerEnterBreakBlock(collision.gameObject);
        }

    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = true;
        if (w_BlockOn)
        {
            w_tile = Mathf.Abs(w_vector1 - transform.position.x);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (setArea == -1 || setArea == player.nowArea)
        {
            if (collision.gameObject.CompareTag("Wind"))
            {
                TriggerEnterWind();
            }
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

    //ESC 관련 함수
    protected virtual void SetEscStop()
    {
        if (player.isStop)
        {
            if (isStop) return;
            isStop = true;
            Stop();
        }
        else
        {
            if (isStop)
            {
                isStop = false;
                ReStart();
            }
        }
    }

    public void Stop()
    {
        realspeed = 0;
        gravityScale = 0;
        jump = 0;
        rigid.gravityScale = 0;
        stopVector = rigid.velocity;
        rigid.velocity = Vector2.zero;
        realspeed = 0;
    }

    public void ReStart()
    {
        realspeed = speed;
        rigid.velocity = stopVector;
        gravityScale = 4.3f;
        jump = 22.5f;
        rigid.gravityScale = 4.3f;
    }



    public virtual void Settingvalue()
    {
        speed = 24.5f;
        maxSpeed = 27.5f;
        friction = 7.5f;
        airfriction = 4.0f;
        downGravityOn = true;
        jump = 22.5f;
        gravityScale = 4.3f;
        rigid.gravityScale = gravityScale;
        realspeed = speed;
    }

    protected virtual void SetJumpDrag()
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
        while (true)
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
            if (!(rigid.velocity.x > -0.0002191039 * 10 && rigid.velocity.x < 0.0002191039 * 10) || rigid.velocity.y != 0)
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
        if (isObject)
        {
            rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
        PlaySound(1, 1);
    }
    public virtual void Down()
    {
        if (isObject)
        {
            rigid.AddForce(Vector2.down * (jump * 0.8f), ForceMode2D.Impulse);
        }
        w_MoveOn = true;
        w_MoveOnEffect = false;
        PlaySound(1, 0);
    }
    public virtual void SuperDown()
    {
        if (isObject)
        {
            rigid.AddForce(Vector2.down * (jump * 0.01f), ForceMode2D.Impulse);
        }
        w_MoveOn = true;
        w_MoveOnEffect = false;
        superDownOn = true;
        PlaySound(1, 0);
        Invoke("SuperDownFalse", 0.2f);

    }
    public virtual void SuperDownFalse()
    {
        if (rigid.velocity.y >= 0)
        {
            superDownOn = false;
        }
    }



    public virtual void SpeedUp()
    {
        realspeed = speed * 2f;
        ChangeMaterial(1);
        PlaySound(0.5f, 2);
        Invoke("SpeedReset", 1f);
    }
    public virtual void SpeedDown()
    {
        realspeed = speed * 0.5f;
        ChangeMaterial(3);
        PlaySound(0.5f, 4);
        Invoke("SpeedReset", 1f);
    }
    public virtual void SpeedStop()
    {
        w_pause = true;
        ChangeMaterial(2);
        Stop();
        pausevector = rigid.velocity;
        PlaySound(0.5f, 3);
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
        ChangeMaterial(0);
        realspeed = speed;
    }
    public virtual void TimeReset()
    {
        w_pause = false;
        if (isObject)
        {
            spriteRenderer.material = wordManager.ReturnMaterials(0);
            Settingvalue();
            rigid.velocity = pausevector;
        }
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


    protected void PlaySound(float time, int soundindex)
    {
        isSound = true;
        SoundManager.Instance.SFXPlay(soundindex);
        Invoke("ResetSound", time);
    }

    protected void ResetSound()
    {
        isSound = false;
        isSoundEffect = true;
    }

    //기능 함수
    public void PlayFunction(string meshode)
    {
        if (setArea == -1 || setArea == player.nowArea)
        {
            Invoke(meshode, 0);
        }
    }

    public virtual void SizeUp()
    {
        if (sizeIndex < 2)
        {
            sizeIndex++;
            SetSizeIndexToScaleVector();
        }
    }

    public virtual void SizeDown()
    {
        if (sizeIndex > -2)
        {
            sizeIndex--;
            SetSizeIndexToScaleVector();
        }
    }
    public void OnBecameVisible()
    {
        if (setArea == -1 || setArea == player.nowArea)
        {
            w_visible = true;
        }
    }
    public void OnBecameInvisible()
    {
        if (setArea == -1 || setArea == player.nowArea)
        {
            w_visible = false;
            w_visibleEffect = false;
        }
    }
    public virtual void ColliderOff()
    {
        if (isObject)
        {
            spriteRenderer.material = wordManager.ReturnMaterials(4);
        }
        w_collider.enabled = false;
        Invoke("ColliderOn", 1f);
    }
    public virtual void ColliderOn()
    {
        if (isObject)
        {
            spriteRenderer.material = wordManager.ReturnMaterials(0);
        }
        w_collider.enabled = true;
    }


    //연산 함수
    protected virtual void SetSizeIndexToScaleVector()
    {
        switch (sizeIndex)
        {
            case 0:
                scaleVetor = new Vector2(1, 1);
                break;
            case 1:
                scaleVetor = new Vector2(1.2f, 1.2f);
                break;
            case 2:
                scaleVetor = new Vector2(1.4f, 1.4f);
                break;
            case -1:
                scaleVetor = new Vector2(0.8f, 0.8f);
                break;
            case -2:
                scaleVetor = new Vector2(0.6f, 0.6f);
                break;
        }
    }

    public virtual float ReturnVelocityY()
    {
        return rigid.velocity.y;
    }


    //그래픽 함수
    protected void ChangeMaterial(int index)
    {
        if (isObject)
        {
            spriteRenderer.material = wordManager.ReturnMaterials(index);
        }
    }

    //충돌용 함수
    protected virtual void TriggerEnterBreakBlock(GameObject collision)
    {
        if (!(rigid.velocity.y <= 0) && collision.transform.position.y >= transform.position.y)
        {
            collision.GetComponent<GimicBlock>().BreakBlock();
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(Vector2.down * 3f, ForceMode2D.Impulse);
        }
        else if (superDownOn)
        {
            collision.GetComponent<GimicBlock>().BreakBlock();
        }
    }
    protected virtual void CollisionBreakBlock(GameObject collision)
    {
        if (superDownOn)
        {
            collision.GetComponent<GimicBlock>().BreakBlock();
        }
    }


    protected virtual void TriggerEnterWind()
    {
        rigid.AddForce(Vector2.right * 2f);
    }
}
