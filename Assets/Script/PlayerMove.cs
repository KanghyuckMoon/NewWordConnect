using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : WordGameObject
{
    //���� ���� ����

    private float velocityX = 0;
    private bool downGravity = false; //�������� �ӵ��� ������ ����
    private bool jumpOn;

    private BoxCollider2D collider2d;

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

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        LoadToJson();
        StartCoroutine(OnMoveDetect());
    }

    private void Update()
    {
        //���������� �Լ�
        Setting(); // ���� �Լ�
        InputJump();
        JumpDrag();

        //�Է� �޴� ��
        InputMove();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void JumpDrag()
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
    public override void Jump()
    {
        rigid.AddForce(Vector2.up * jump,ForceMode2D.Impulse);
        jumpOn = true;
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
    }


    //�������
    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(Save_Path + Save_FileName, json, System.Text.Encoding.UTF8);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        w_Collider = true;
        w_ColliderEffect = false;
        jumpOn = false;
        w_tile = 0;
        w_vector1 = transform.position.x;
        w_BlockOn = true;
    }
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        w_Collider = false;
        w_ColliderEffect = false;
        jumpOn = true;
        w_BlockOn = false;
        w_tile = 0;
    }
}
