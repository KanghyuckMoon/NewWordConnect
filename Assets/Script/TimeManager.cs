using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : WordGameObject
{
    [SerializeField]
    private float times = 0;

    public bool isRewinding = false;
    public bool isStartRecord = false;
    public bool isRecord = false;
    private WaitForSecondsRealtime waitForSeconds;
    [SerializeField]
    private List<Vector2> positions = new List<Vector2>();
    private Transform playerTransform;


    protected override void Start()
    {
        base.Start();
        waitForSeconds = new WaitForSecondsRealtime(0.1f);
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        //StartCoroutine(RewindRepeat());
    }

    private void Update()
    {
        times = Time.timeScale;
        //if (isStartRecord)
        //{
        //    if (isRewinding)
        //    {
        //        StartRewind();
        //    }
        //    else
        //    {
        //        StopRewind();
        //    }
        //}
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }
        if(Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        if(isRecord)
        {
            Record();
        }
    }

    private IEnumerator RewindRepeat()
    {
        while(true)
        {
            if(isStartRecord)
            {
                if(isRewinding)
                {
                    Rewind();
                }
                else
                {
                    Debug.Log("a");
                    Record();
                }
            }
            yield return waitForSeconds;
        }
    }

    public void ResetPositions()
    {
        isStartRecord = false;
        positions.Clear();
    }

    public override void Setting()
    {
        speed = user.speed;
        maxSpeed = user.maxspeed;
        friction = user.friction;
        airfriction = user.aitfriction;
        downGravityOn = user.downGravityOn;
        gravityScale = 0;
        jump = user.jump;

        rigid.drag = friction;
        rigid.gravityScale = 0;
    }

    public override void Jump()
    {
        Time.timeScale = 10;
        Invoke("Jumpoff", 0.1f);
    }
    private void Jumpoff()
    {
        Time.timeScale = 1;
    }
    public override void Down()
    {
        isRecord = true;
        DOVirtual.DelayedCall(1f, ResetBool, true);
    }
    public void firStartRewind()
    {
        isRewinding = true;
        //DOVirtual.DelayedCall(0.5f, ResetBool, true);
    }
    public void ResetBool()
    {
        isRecord = false;
        isRewinding = true;
    }
    public override void SpeedUp()
    {
        Time.timeScale = 1.5f;
        Invoke("SpeedReset", 1);
    }
    public override void SpeedDown()
    {
        Time.timeScale = 0.5f;
        Invoke("SpeedReset", 1);
    }
    public override void SpeedStop()
    {
        Time.timeScale = 0;
        DOVirtual.DelayedCall(1, SpeedReset, true);
    }
    public override void SpeedReset()
    {
        Time.timeScale = 1f;
    }

    public override void SizeUp()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = 1;
            Time.timeScale = 1.2f;
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            Time.timeScale = 1;
        }

    }
    public override void SizeDown()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = -1;
            Time.timeScale = 0.8f;
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            Time.timeScale = 1;
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        //isStartRecord = false;
    }

    public void Record()
    {
        positions.Insert(0, playerTransform.position);
    }

    public void Rewind()
    {
        if (positions.Count > 0)
        {
            playerTransform.position = positions[0];
            positions.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }
}
