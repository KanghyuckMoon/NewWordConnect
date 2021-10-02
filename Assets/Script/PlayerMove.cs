using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : WordGameObject
{
    private float velocityX = 0;
    private bool downGravity = false; 
    private Vector2 savePoint;

    private BoxCollider2D collider2d;

    public int nowArea = 0;
    private WordManager wordManager;
    private TextManager textManager;
    //애니메이션
    private SpriteRenderer spriteRenderer = null;
    private Collider2D colliders = null;
    private Animator animator = null;
    private bool isWalk = false;
    private ParticleSystem[] dust;
    private Vector2 scaleVetor = new Vector2(1, 1);
    [SerializeField]
    private DieEffect dieEffect;
    [SerializeField]
    private Transform cloth;
    private CameraMove maincam;
    private bool die = false;

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(OnMoveDetect());
        realspeed = speed;
        rigid.gravityScale = gravityScale;
        w_collider = GetComponent<Collider2D>();
        savePoint = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dust = GetComponentsInChildren<ParticleSystem>();
        colliders = GetComponent<Collider2D>();
        
        wordManager = FindObjectOfType<WordManager>();
        textManager = FindObjectOfType<TextManager>();
        maincam = Camera.main.GetComponent<CameraMove>();
    }

    private void Update()
    {
        if (die) return;
        if (wordManager.isEvent) return;
        //���������� �Լ�
        InputJump();
        JumpDrag();

        //�Է� �޴� ��
        InputMove();
    }

    private void FixedUpdate()
    {
        if (die) return;
        if (wordManager.isEvent) return;
        if (wordManager.isInputESC) return;
        Move();
        DownDust();
    }
    

    private void InputJump()
    {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
    }

    private void InputMove()
    {
        velocityX = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        isWalk = true;
        rigid.AddForce(Vector2.right * (velocityX * realspeed));
        if(downGravity)
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigid.velocity.y, -maxSpeed, maxSpeed));
        }
        else
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y);
        }
        SetAnimation();
    }
    public override void Jump()
    {
        rigid.AddForce(Vector2.up * jump,ForceMode2D.Impulse);
        jumpOn = true;
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
        CreateDust();
        PlaySound();
    }

    public override void SuperDown()
    {
        base.SuperDown();
    }


    //�������
    //private void SaveToJson()
    //{
    //    string json = JsonUtility.ToJson(user, true);
    //    File.WriteAllText(Save_Path + Save_FileName, json, System.Text.Encoding.UTF8);
    //}

    private void OnApplicationQuit()
    {
        //SaveToJson();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = false;
        jumpOn = false;
        w_tile = 0;
        w_vector1 = transform.position.x;
        w_BlockOn = true;
        CreateDust();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.transform.position.y < transform.position.y && rigid.velocity.y < 0)
            {
                Jump();
                collision.gameObject.GetComponent<EnemyBased>().Die();
                PlaySound();
                maincam.Shakecam(2f,0.2f);
            }
            else
            {
                Died();
            }
        }
        superDownOn = false;
    }
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        w_Collider = false;
        w_ColliderEffect = false;
        jumpOn = true;
        w_BlockOn = false;
        w_tile = 0;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (collision.gameObject.CompareTag("TextObj"))
        {
            if (wordManager.isEvent) return;
            if(Input.GetKeyDown(KeyCode.W))
            {
                textManager.ChatStart(collision.gameObject.GetComponent<TextObject>().ReturnTextIndex());
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wind"))
        {
            rigid.AddForce(Vector2.right * 2f);
        }
        if (collision.CompareTag("CameraLock"))
        {
            nowArea = collision.GetComponent<CameraSettingObject>().SetCameraMoveSetting();
        }
        if (collision.CompareTag("Spike"))
        {
            Died();
        }
        if (collision.CompareTag("BreakBlock") && !(rigid.velocity.y <= 0) && collision.transform.position.y >= transform.position.y)
        {
            collision.GetComponent<GimicBlock>().BreakBlock();
            rigid.velocity = new Vector2(rigid.velocity.x,0);
            rigid.AddForce(Vector2.down * 3f,ForceMode2D.Impulse);
        }
        else if (collision.CompareTag("BreakBlock") && superDownOn)
        {
            collision.GetComponent<GimicBlock>().BreakBlock();
        }
        if (collision.CompareTag("Spring") && (rigid.velocity.y <= 0))
        {
            collision.GetComponent<GimicSpring>().SpringTread();
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(Vector2.up * 30f, ForceMode2D.Impulse);
            PlaySound();
        }
    }

    public void Died()
    {
        cloth.gameObject.SetActive(false);
        maincam.Shakecam(3f, 0.3f);
        dieEffect.transform.position = transform.position;
        dieEffect.gameObject.SetActive(true);
        Invoke("DiedtoReset", 1f);
        wordManager.PlayToDieResetAnimation();
        die = true;
        spriteRenderer.enabled = false;
        colliders.enabled = false;
        rigid.gravityScale = 0f;
    }
    private void DiedtoReset()
    {
        transform.position = savePoint;
        cloth.position = transform.position;
        cloth.gameObject.SetActive(true);
        die = false;
        spriteRenderer.enabled = true;
        colliders.enabled = true;
        rigid.gravityScale = 4.3f;
    }

    public void SetSavePoint(Vector2 transform)
    {
        savePoint = transform;
    }

    private void SetAnimation()
    {
        if (velocityX == 1)
        {
            transform.localScale = new Vector2(-1 * scaleVetor.x, scaleVetor.y);
        }
        else if (velocityX == -1)
        {
            transform.localScale = new Vector2(scaleVetor.x, scaleVetor.y);
        }
        else
        {
            transform.localScale = new Vector2(ReturnPlusOrMinuse(transform.localScale.x) * scaleVetor.x, scaleVetor.y);
            isWalk = false;
        }
        animator.SetBool("IsWalk", isWalk);
    }

    private int ReturnPlusOrMinuse(float a)
    {
        if (a >= 0) return 1;
        else return -1;
    }

    private void CreateDust()
    {
        dust[0].Play();
    }

    private void DownDust()
    {
        if (superDownOn)
        {
            dust[1].Play();
        }
        else
        {
            if (rigid.velocity.y < -1.2f)
            {
                dust[0].Play();
            }
        }
    }

    public override void SizeUp()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = 1;
            scaleVetor = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            scaleVetor = new Vector2(1.4f, 1.4f);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            scaleVetor = new Vector2(1, 1);
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            scaleVetor = new Vector2(0.8f, 0.8f);
        }
        SetAnimation();
    }

    public override void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            scaleVetor = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            scaleVetor = new Vector2(1, 1);
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            scaleVetor = new Vector2(0.8f, 0.8f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            scaleVetor = new Vector2(1, 1);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            scaleVetor = new Vector2(0.6f, 0.6f);
        }
        SetAnimation();
    }
}
