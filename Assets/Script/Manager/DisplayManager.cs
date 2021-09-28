 using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class DisplayManager : WordGameObject
{

    public int x;
    public int y;
    public int ax;
    public int ay;
    private int[] sizevector = new int[2];
    public Vector2 scaleVector = new Vector2(1, 1);
    private Vector2 notMoveVector = Vector2.zero;

    public void Set1()
    {
        SetPosition(x, y);
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "NewWordConnect"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif

    // Use this for initialization
    void Awake()
    { 
        x = 1250 / 2;
        y = 520 / 2;
        sizevector[0] = 640;
        sizevector[1] = 480;
        SetPosition(x, y);
    }

    protected override void Start()
    {

    }

    protected override IEnumerator OnMoveDetect()
    {
        while (true)
        {
            if (w_Movetime < 0.02f)
            {
                w_Movetime += Time.deltaTime;

            }
            else
            {
                w_MoveOn = false;
                w_MoveOnEffect = true;
            }
            if (new Vector2(x + ax, y + ay) != notMoveVector)
            {
                notMoveVector = new Vector2(x + ax, y + ay);
                w_MoveOn = true;
                w_Movetime = 0f;
            }

            yield return waitForSeconds;
        }
    }

    private void Update()
    {
        SetPosition(x + ax,y + ay); // 위치 조절
        Screen.SetResolution(sizevector[0],sizevector[1],false);
    }

    public override void Jump()
    {
        ay += -50;
    }
    public override void Down()
    {
        ay += 50;
    }
    public override void SizeUp()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = 1;
            sizevector[0] = 720;
            ax = -40;
            ay += -30;
            sizevector[1] = 540;
            scaleVector = new Vector2(0.5f, 0.5f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            sizevector[0] = 1280;
            ax = -320;
            ay += -210;
            sizevector[1] = 960;
            scaleVector = new Vector2(0.375f, 0.375f);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            sizevector[0] = 640;
            ax = 0;
            sizevector[1] = 480;
            scaleVector = new Vector2(1, 1);
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            sizevector[0] = 320;
            ax = 160;
            ay += -120;
            sizevector[1] = 240;
            scaleVector = new Vector2(1.125f, 1.125f);
        }
    }

    public override void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            sizevector[0] = 720;
            ax = -40;
            ay += 210;
            sizevector[1] = 540;
            scaleVector = new Vector2(0.5f, 0.5f);
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            sizevector[0] = 640;
            ax = 0;
            ay += 30;
            sizevector[1] = 480;
            scaleVector = new Vector2(1, 1);
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            sizevector[0] = 320;
            ax = 160;
            ay += 120;
            sizevector[1] = 240;
            scaleVector = new Vector2(1.125f, 1.125f);
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            sizevector[0] = 240;
            ax = 200;
            ay += 30;
            sizevector[1] = 180;
            scaleVector = new Vector2(2, 2);
        }
    }
}