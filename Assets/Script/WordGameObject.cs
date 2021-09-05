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

    //공용
    protected Rigidbody2D rigid = null;

    //1초동안 정지한다
    private bool pause;
    private float cooltime;
    private float gravityscale;
    private Vector2 pausevector;


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




    //실행어

    public virtual void Jump()
    {
        rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
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
    }
}
