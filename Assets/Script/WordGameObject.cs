using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //�������� ��������
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

    //����
    protected Rigidbody2D rigid = null;

    //1�ʵ��� �����Ѵ�
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




    //�����

    public virtual void Jump()
    {
        rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
    }

    public virtual void TimePause()
    {
        pausevector = rigid.velocity;//���� �ӵ� ����
        w_speed = 0;//�̵��ӵ� 0
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
        rigid.gravityScale = 0; // �߷� ũ�� 0
        rigid.velocity = Vector2.zero;//���� �ӵ� 0���� ����
        Invoke("TimeCountinue", 1);
        
    }
    public virtual void TimeCountinue()
    {
        rigid.velocity = pausevector; //�ӵ� �Ҿ����
        w_speed = 1;//�̵��ӵ� ����
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
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
