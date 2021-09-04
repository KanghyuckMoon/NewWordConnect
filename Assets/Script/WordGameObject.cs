using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //������ӿ�����Ʈ
    private PlayerSetting user = null;

    protected float w_speed = 1;
    protected float w_size = 1;
    protected bool w_notcollider = false;

    //����
    protected Rigidbody2D rigid = null;

    //1�ʵ��� �����Ѵ�
    private bool pause;
    private float cooltime;
    private float gravityscale;
    private Vector2 pausevector;


    private void Start()
    {
        if(gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            rigid = GetComponent<Rigidbody2D>();
        }
    }


    //�����
    private void TimePause()
    {
        pausevector = rigid.velocity;//���� �ӵ� ����
        w_speed = 0;//�̵��ӵ� 0
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
        rigid.gravityScale = 0; // �߷� ũ�� 0
        rigid.velocity = Vector2.zero;//���� �ӵ� 0���� ����
        Invoke("TimeCountinue", 1);
        
    }
    private void TimeCountinue()
    {
        rigid.velocity = pausevector; //�ӵ� �Ҿ����
        w_speed = 1;//�̵��ӵ� ����
        gravityscale = rigid.gravityScale; //�߷� ũ�� ����
    }
}
