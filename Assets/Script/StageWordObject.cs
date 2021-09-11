using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWordObject : WordGameObject
{
    private StageManager stageManager;
    [SerializeField]
    private List<GimicBase> gimiclist = new List<GimicBase>();

    protected override void Start()
    {
        //base.Start();
        stageManager = FindObjectOfType<StageManager>();
        rigid = GetComponent<Rigidbody2D>();
        w_collider = GetComponent<Collider2D>();
        Settingvalue();
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<GimicBase>() != null)
            {
                gimiclist.Add(transform.GetChild(i).GetComponent<GimicBase>());
            }
        }
    }

    public override void Settingvalue()
    {
        base.Settingvalue();
        speed = 1;
    }

    public override void SizeUp()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = 1;
            stageManager.transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            stageManager.transform.localScale = new Vector2(1.4f, 1.4f);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            stageManager.transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            stageManager.transform.localScale = new Vector2(0.8f, 0.8f);
        }

    }

    public override void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            stageManager.transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            stageManager.transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            stageManager.transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            stageManager.transform.localScale = new Vector2(1, 1);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            stageManager.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    }

    private void GimicListSendRealSpeed()
    {
        for (int i = 0; i < gimiclist.Count; i++)
        {
            gimiclist[i].SetGimicSpeed(realspeed);
        }
    }

    public override void SpeedUp()
    {
        base.SpeedUp();
        GimicListSendRealSpeed();
    }

    public override void SpeedDown()
    {
        base.SpeedDown();
        GimicListSendRealSpeed();
    }

    public override void SpeedStop()
    {
        base.SpeedStop();
        GimicListSendRealSpeed();
    }

    public override void SpeedReset()
    {
        base.SpeedReset();
        GimicListSendRealSpeed();
    }

    public override void SpeedStopnotinvoke()
    {
        base.SpeedStopnotinvoke();
        GimicListSendRealSpeed();
    }

    public override void TimeReset()
    {
        base.TimeReset();
        GimicListSendRealSpeed();
    }
}
