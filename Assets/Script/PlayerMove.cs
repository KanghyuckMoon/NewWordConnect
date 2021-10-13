﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : WordGameObject
{
    private float velocityX = 0;
    private bool downGravity = false; 
    private Vector2 savePoint;
    public int nowArea = 0;
    private int dieArea = 0;
    private TextManager textManager;
    //애니메이션
    private Collider2D colliders = null;
    private Animator animator = null;
    private bool isWalk = false;
    private ParticleSystem[] dust;
    [SerializeField]
    private DieEffect dieEffect;
    [SerializeField]
    private Transform cloth;
    private CameraMove maincam;
    private bool die = false;
    private bool isAir = false;
    private int layerMask = 0;
    private bool isInvincibility = false;
    private bool win;

    protected override void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("StgaePhysicsOnlyDefault");
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

        Settingvalue();
    }

    protected override void SetEscStop()
    {
        if (isStop)
        {
            if (isStop) return;
            isStop = true;
            EscStop();
        }
        else
        {
            if (isStop)
            {
                isStop = false;
                EscReset();
            }
        }
    }

    private void Update()
    {
        if (win) return;
        if (die) return;
        if (wordManager.isEvent) return;
        if (w_pause) return;
        InputJump();
        InputMove();
    }

    private void FixedUpdate()
    {
        if (win) return;
        SetEscStop();
        if (isStop) return;
        if (die) return;
        if (wordManager.isEvent) return;
        if (wordManager.isInputESC) return;
        if (w_pause) return;
        SetJumpDrag();
        GravitySet();
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
        if (downGravity)
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
        CreateDust(0);
        PlaySound(1);
        SoundManager.Instance.SFXPlay(1);
    }

    public void GravitySet()
    {


        Vector2 frontvec = new Vector2(rigid.position.x, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontvec, Vector3.down,1, layerMask);
        if (rayHit.collider != null)
        {
            isAir = false;
        }
        else
        {
            isAir = true;
        }
        if (isAir)
        {
            rigid.gravityScale = 4.3f;
        }
        else
        {
            rigid.gravityScale = 1f;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = false;
        jumpOn = false;
        w_tile = 0;
        w_vector1 = transform.position.x;
        w_BlockOn = true;
        CreateDust(0);
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                if (collision.transform.position.y + 0.1f < transform.position.y)
                {
                    Jump();
                    collision.gameObject.GetComponent<EnemyBased>().Die();
                    PlaySound(1);
                    SoundManager.Instance.SFXPlay(1);
                    maincam.Shakecam(2f, 0.2f);
                }
                else
                {
                    Died();
                }
                break;
            case "Bloon":
                if (collision.transform.position.y < transform.position.y && rigid.velocity.y < 0)
                {
                    Jump();
                    collision.gameObject.GetComponent<GimicBloon>().BloonBoom();
                    PlaySound(1);
                    SoundManager.Instance.SFXPlay(1);
                    maincam.Shakecam(1f, 0.1f);
                }
                break;
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

        transform.SetParent(null);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (collision.gameObject.CompareTag("MovingTile"))
        {
            if(rigid.velocity.y <= 0 && transform.position.y > collision.transform.position.y)
            {
                transform.SetParent(collision.transform);
            }
            else
            {
                transform.SetParent(null);
            }
        }
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
        switch(collision.gameObject.tag)
        {
            case "Wind":
                rigid.AddForce(Vector2.right * 2f);
                break;
            case "CameraLock":
                nowArea = collision.GetComponent<CameraSettingObject>().SetCameraMoveSetting();
                break;
            case "Spike":
                Died();
                break;
            case "BreakBlock":
                if(!(rigid.velocity.y <= 0) && collision.transform.position.y >= transform.position.y)
                {
                    collision.GetComponent<GimicBlock>().BreakBlock();
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.down * 3f, ForceMode2D.Impulse);
                }
                else if(superDownOn)
                {
                    collision.GetComponent<GimicBlock>().BreakBlock();
                }
                break;
            case "Spring":
                if((rigid.velocity.y <= 0))
                {
                    collision.GetComponent<GimicSpring>().SpringTread();
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.up * 30f, ForceMode2D.Impulse);
                    PlaySound(1);
                    SoundManager.Instance.SFXPlay(1);
                }
                break;
            case "WinPoint":
                WinNextScene();
                break;
            case "GetWord":
                collision.GetComponent<NewItemGet>().GetItem();
                break;
            case "ColorBlock":
                if (!(rigid.velocity.y <= 0) && collision.transform.position.y >= transform.position.y)
                {
                    collision.GetComponent<GimicPassWord>().ChangeColor();
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.down * 3f, ForceMode2D.Impulse);
                }
                break;
        }
    }

    public void WinNextScene()
    {
        if (isInvincibility) return;
        win = true;
        isInvincibility = true;
        wordManager.WinGame();
        Invoke("MoveStageSelect",2f);
    }

    public void MoveStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void Died()
    {
        if (isInvincibility) return;
        cloth.gameObject.SetActive(false);
        maincam.Shakecam(3f, 0.3f);
        dieEffect.transform.position = transform.position;
        dieEffect.gameObject.SetActive(true);
        wordManager.PlayToDieResetAnimation();
        die = true;
        spriteRenderer.enabled = false;
        colliders.enabled = false;
        rigid.gravityScale = 0f;
        dieArea = nowArea;
        nowArea = -1;
        Invoke("DietoReset", 1);
    }
    private void DietoReset()
    {
        transform.position = savePoint;
        cloth.position = transform.position;
        cloth.gameObject.SetActive(true);
        die = false;
        spriteRenderer.enabled = true;
        colliders.enabled = true;
        rigid.gravityScale = 4.3f;
        nowArea = dieArea;
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

    private void CreateDust(int index)
    {
        //0이면 일반먼지 1이면 슈퍼먼지
        dust[index].Play();
    }

    private void DownDust()
    {
        if (superDownOn)
        {
            CreateDust(1);
        }
        else
        {
            if (rigid.velocity.y < -1.2f)
            {
                CreateDust(0);
            }
        }
    }

    public override void SizeUp()
    {
        if (sizeIndex < 2)
        {
            sizeIndex--;
            SetSizeIndexToScaleVector();
        }
        SetAnimation();
    }

    public override void SizeDown()
    {
        if(sizeIndex > -2)
        {
            sizeIndex--;
            SetSizeIndexToScaleVector();
        }
        SetAnimation();
    }



    //연산 관련 함수



    //충돌 관련 함수
    protected virtual void CollisionEnterEnemy()
    {

    }
    protected virtual void CollisionEnterBloon()
    {

    }
    protected virtual void CollisionStayMovingTile()
    {

    }
    protected virtual void TriggerStayTextobj()
    {

    }
    protected virtual void TriggerEnterWind()
    {

    }
    protected virtual void TriggerEnterCameraLock()
    {

    }
    protected virtual void TriggerEnterSpike()
    {

    }
    protected virtual void TriggerEnterBreakBlock()
    {

    }
    protected virtual void TriggerEnterSpring()
    {

    }
    protected virtual void TriggerEnterWinPoint()
    {

    }
    protected virtual void TriggerEnterGetWord()
    {

    }
    protected virtual void TriggerEnterColorBlock()
    {

    }
}
