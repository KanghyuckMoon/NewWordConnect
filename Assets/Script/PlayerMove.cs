using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //���� ���� ����
    [SerializeField]
    private PlayerSetting user = null;
    private string Save_Path = "";
    private string Save_FileName = "/MoveFile.txt";


    private float speed; //���ϴ� �̵��ӵ�
    private float maxSpeed; //���ϴ� �ִ��̵��ӵ�
    private float velocityX = 0;
    private float friction = 0; //������
    private float airfriction = 0; //������
    private float gravityScale = 0; // �߷�
    private bool downGravity = false; //�������� �ӵ��� ������ ����
    private bool jumpOn;
    private float jump = 0;

    private Rigidbody2D rigid;
    private BoxCollider2D collider;

    //�ܾ��� ��
    private float realspeed = 0; //���� �̵��ӵ�

    private void Awake()
    {
        Save_Path = Application.dataPath + "/Save";
        //�ȵ���̵� ��忡����  Application.dataPath ��ſ� Application.persistentDataPath
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
        //���������� �Լ�
        Setting(); // ���� �Լ�
        InputJump();

        //�Է� �޴� ��
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
        airfriction = user.aitfriction;
        downGravity = user.downGravityOn;
        gravityScale = user.gravityScale;
        jump = user.jump;
        if(jumpOn)
        {
            rigid.drag = airfriction; // 마찰력 설정
        }
        else
        {
            rigid.drag = friction; // 공기 마찰력 설정
        }
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


    //�������
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        jumpOn = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        jumpOn = true;
    }
}
