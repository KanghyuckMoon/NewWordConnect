using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameObject : MonoBehaviour
{
    //워드게임오브젝트
    private PlayerSetting user = null;

    protected float w_speed = 1;
    protected float w_size = 1;
    protected bool w_notcollider = false;

    //공용
    protected Rigidbody2D rigid = null;

    //1초동안 정지한다
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


    //실행어
    private void TimePause()
    {
        pausevector = rigid.velocity;//현재 속도 저장
        w_speed = 0;//이동속도 0
        gravityscale = rigid.gravityScale; //중력 크기 저장
        rigid.gravityScale = 0; // 중력 크기 0
        rigid.velocity = Vector2.zero;//현재 속도 0으로 만듬
        Invoke("TimeCountinue", 1);
        
    }
    private void TimeCountinue()
    {
        rigid.velocity = pausevector; //속도 불어오기
        w_speed = 1;//이동속도 정상
        gravityscale = rigid.gravityScale; //중력 크기 저장
    }
}
