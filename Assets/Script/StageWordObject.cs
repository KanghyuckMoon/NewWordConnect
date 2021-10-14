using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWordObject : WordGameObject
{
    private StageManager stageManager;
    [SerializeField]
    private List<GimicBase> gimiclist = new List<GimicBase>();
    private List<Rigidbody2D> gimicHasLigid = new List<Rigidbody2D>();
    [SerializeField]
    private Transform areaobjects;
    private List<StageAreaReset> areaobj = new List<StageAreaReset>();
    [SerializeField]
    private GameObject gravityobj;

    protected override void Start()
    {
        base.Start();
        stageManager = FindObjectOfType<StageManager>();
        rigid = GetComponent<Rigidbody2D>();
        w_collider = GetComponent<Collider2D>();
        Settingvalue();

        for(int i = 0; i < areaobjects.childCount; i++)
        {
            if(areaobjects.GetChild(i).GetComponent<StageAreaReset>() != null)
            {
                areaobj.Add(areaobjects.GetChild(i).GetComponent<StageAreaReset>());
            }
        }

        for(int i = 0; i < areaobj.Count; i++)
        {
            for(int j = 0; j < areaobj[i].transform.childCount; j++)
            {
                if (areaobj[i].transform.GetChild(j).GetComponent<GimicBase>() != null)
                {
                    gimiclist.Add(areaobj[i].transform.GetChild(j).GetComponent<GimicBase>());
                }
                if (areaobj[i].transform.GetChild(j).GetComponent<Rigidbody2D>() != null)
                {
                    gimicHasLigid.Add(areaobj[i].transform.GetChild(j).GetComponent<Rigidbody2D>());
                }
            }
        }
    }

    public override void Settingvalue()
    {
        base.Settingvalue();
        speed = 1;
    }

    public override void Jump()
    {
        base.Jump();
        for(int i = 0; i < gimicHasLigid.Count;i++)
        {
            gimicHasLigid[i].velocity = new Vector2(gimicHasLigid[i].velocity.x, 0);
            gimicHasLigid[i].AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }

    public override void SizeUp()
    {
        if (sizeIndex < 2)
        {
            sizeIndex++;
            SetSizeIndexToScaleVector();
        }

    }

    protected override void SetSizeIndexToScaleVector()
    {
        switch(sizeIndex)
        {
            case 0:
                stageManager.transform.localScale = new Vector2(1, 1);
                gravityobj.transform.localScale = new Vector2(1f, 1f);
                break;
            case 1:
                stageManager.transform.localScale = new Vector2(1.2f, 1.2f);
                gravityobj.transform.localScale = new Vector2(1.2f, 1.2f);
                break;
            case 2:
                stageManager.transform.localScale = new Vector2(1.4f, 1.4f);
                gravityobj.transform.localScale = new Vector2(1.4f, 1.4f);
                break;
            case -1:
                stageManager.transform.localScale = new Vector2(0.8f, 0.8f);
                gravityobj.transform.localScale = new Vector2(0.8f, 0.8f);
                break;
            case -2:
                stageManager.transform.localScale = new Vector2(0.6f, 0.6f);
                gravityobj.transform.localScale = new Vector2(0.6f, 0.6f);
                break;
        }
    }

    public override void SizeDown()
    {
        if (sizeIndex > -2)
        {
            sizeIndex--;
            SetSizeIndexToScaleVector();
        }
    }

    private void GimicListSendRealSpeed()
    {
        for (int i = 0; i < gimiclist.Count; i++)
        {
            gimiclist[i].SetGimicSpeed(realspeed);
        }
    }

    private void GimicListSendMaterial(int index)
    {
        for (int i = 0; i < gimiclist.Count; i++)
        {
            gimiclist[i].SetMaterial(wordManager.ReturnMaterials(index));
        }
    }

    public override void SpeedUp()
    {
        base.SpeedUp();
        GimicListSendRealSpeed();
        GimicListSendMaterial(1);
    }

    public override void SpeedDown()
    {
        base.SpeedDown();
        GimicListSendRealSpeed();
        GimicListSendMaterial(3);
    }

    public override void SpeedStop()
    {
        base.SpeedStop();
        realspeed = 0;
        GimicListSendRealSpeed();
        GimicListSendMaterial(2);
    }

    public override void SpeedReset()
    {
        base.SpeedReset();
        GimicListSendRealSpeed();
        GimicListSendMaterial(0);
    }

    public override void SpeedStopnotinvoke()
    {
        base.SpeedStopnotinvoke();
        GimicListSendRealSpeed();
    }

    public override void TimeReset()
    {
        base.TimeReset();
        realspeed = 1;
        GimicListSendRealSpeed();
        GimicListSendMaterial(0);
    }
}
