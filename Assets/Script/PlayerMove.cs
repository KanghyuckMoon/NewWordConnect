using System.IO;
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
    private Vector2 frontvec;
    private RaycastHit2D rayHit;
    [SerializeField]
    private int[] NextStageUnLockList;
    [SerializeField]
    private int NowStageIndex;

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

    private void Update()
    {
        if (win) return;
        if (die) return;
        if (wordManager.isEvent) return;
        if (w_pause) return;
        InputDied();
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
    

    private void InputDied()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Died();
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
        base.Jump();
        jumpOn = true;
        CreateDust(0);
    }

    public void GravitySet()
    {
        frontvec = new Vector2(rigid.position.x, rigid.position.y);
        rayHit = Physics2D.Raycast(frontvec, Vector3.down,1, layerMask);
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
                CollisionEnterEnemy(collision.gameObject);
                break;
            case "Bloon":
                CollisionEnterBloon(collision.gameObject);
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
            CollisionStayMovingTile(collision.gameObject);
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (collision.gameObject.CompareTag("TextObj"))
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
            TriggerStayTextobj(collision.gameObject);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Wind":
                TriggerEnterWind();
                break;
            case "CameraLock":
                TriggerEnterCameraLock(collision.gameObject);
                break;
            case "Spike":
                Died();
                break;
            case "BreakBlock":
                TriggerEnterBreakBlock(collision.gameObject);
                break;
            case "Spring":
                TriggerEnterSpring(collision.gameObject);
                break;
            case "WinPoint":
                WinNextScene();
                break;
            case "GetWord":
                collision.GetComponent<NewItemGet>().GetItem();
                break;
            case "ColorBlock":
                TriggerEnterColorBlock(collision.gameObject);
                break;
            case "AutoTextObj":
                TriggerStayTextobj(collision.gameObject);
                break;
        }
    }

    public void WinNextScene()
    {
        if (isInvincibility) return;
        win = true;
        isInvincibility = true;
        wordManager.WinGame();
        if(NowStageIndex == -1)
        {

        }
        else
        {
            for (int i = 0; i < NextStageUnLockList.Length; i++)
            {
                if (SaveManager.Instance.CurrentSaveUser.isstageClears[NextStageUnLockList[i]] == 0)
                {
                    SaveManager.Instance.CurrentSaveUser.isstageClears[NextStageUnLockList[i]] = 1;
                }
            }
            SaveManager.Instance.CurrentSaveUser.isstageClears[NowStageIndex] = 2;
        }
        Invoke("MoveStageSelect",2f);
    }

    public void MoveStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public override void Died()
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

    public override void SpeedUp()
    {
        realspeed = speed * 3.5f;
        ChangeMaterial(1);
        PlaySound(0.5f, 2);
        Invoke("SpeedReset", 1f);
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

    //충돌 관련 함수
    protected virtual void CollisionEnterEnemy(GameObject collision)
    {
        if (collision.transform.position.y + 0.1f < transform.position.y)
        {
            Jump();
            collision.gameObject.GetComponent<EnemyBased>().Died();
            maincam.Shakecam(2f, 0.2f);
        }
        else
        {
            Died();
        }
    }
    protected virtual void CollisionEnterBloon(GameObject collision)
    {
        if (collision.transform.position.y < transform.position.y && rigid.velocity.y < 0)
        {
            Jump();
            collision.gameObject.GetComponent<GimicBloon>().BloonBoom();
            maincam.Shakecam(1f, 0.1f);
        }
    }
    protected virtual void CollisionStayMovingTile(GameObject collision)
    {
        if (rigid.velocity.y <= 0 && transform.position.y > collision.transform.position.y)
        {
            transform.SetParent(collision.transform);
        }
        else
        {
            transform.SetParent(null);
        }
    }
    protected virtual void TriggerStayTextobj(GameObject collision)
    {
        if (wordManager.isEvent) return;
        textManager.ChatStart(collision.GetComponent<TextObject>().ReturnTextIndex());
    }
    protected virtual void TriggerEnterCameraLock(GameObject collision)
    {
        nowArea = collision.GetComponent<CameraSettingObject>().SetCameraMoveSetting();
    }
    protected virtual void TriggerEnterSpike()
    {

    }
    
    protected virtual void TriggerEnterSpring(GameObject collision)
    {
        if ((rigid.velocity.y <= 0))
        {
            collision.GetComponent<GimicSpring>().SpringTread();
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(Vector2.up * 30f, ForceMode2D.Impulse);
            PlaySound(1, 0);
        }
    }
    protected virtual void TriggerEnterWinPoint()
    {

    }
    protected virtual void TriggerEnterGetWord()
    {

    }
}
