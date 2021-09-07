using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWordObject : WordGameObject
{
    private StageManager stageManager;


    protected override void Start()
    {
        base.Start();
        stageManager = FindObjectOfType<StageManager>();
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
}
