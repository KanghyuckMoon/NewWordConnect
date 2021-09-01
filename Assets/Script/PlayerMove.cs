using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //저장 관련 설정
    [SerializeField]
    private PlayerSetting user = null;
    private string Save_Path = "";
    private string Save_FileName = "/MoveFile.txt";


    private float speed; //원하는 이동속도
    private float maxSpeed; //원하는 최대이동속도
    private float velocityX = 0;
    private float friction = 0; //마찰력
    private float gravityScale = 0; // 중력
    private bool downGravity = false; //떨어지는 속도에 제한을 둘지
    private bool jumpOn;
    private float jump = 0;

    private Rigidbody2D rigid;
    private BoxCollider2D collider;

    //단어의 힘
    private float realspeed = 0; //실제 이동속도

    private void Awake()
    {
        Save_Path = Application.dataPath + "/Save";
        //안드로이드 빌드에서는  Application.dataPath 대신에 Application.persistentDataPath
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        LoadToJson();
    }

    private void Update()
    {
        //관리자전용 함수
        Setting(); // 설정 함순
        InputJump();

        //입력 받는 곳
        InputMove();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void Setting()
    {
        speed = user.speed;
        maxSpeed = user.maxspeed;
        friction = user.friction;
        downGravity = user.downGravityOn;
        gravityScale = user.gravityScale;
        jump = user.jump;
        rigid.drag = friction; // 마찰력 설정
        rigid.gravityScale = gravityScale;
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
        rigid.AddForce(Vector2.right * (velocityX * speed));
        if(downGravity)
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigid.velocity.y, -maxSpeed, maxSpeed));
        }
        else
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y);
        }
    }
    private void Jump()
    {
        rigid.AddForce(Vector2.up * jump,ForceMode2D.Impulse);
        jumpOn = true;
    }

    //저장관련
    private void LoadToJson()
    {
        if (File.Exists(Save_Path + Save_FileName))
        {
            string json = File.ReadAllText(Save_Path + Save_FileName);
            user = JsonUtility.FromJson<PlayerSetting>(json);
        }
    }
    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(Save_Path + Save_FileName, json, System.Text.Encoding.UTF8);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpOn = false;
    }
}
