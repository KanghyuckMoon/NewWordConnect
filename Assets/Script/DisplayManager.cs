using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class DisplayManager : WordGameObject
{


    public InputField inputx;
    public InputField inputy;
    public int x;
    public int y;
    public Text text;

    public void Set1()
    {
        SetPosition(x, y);
        //Display.displays[1].SetParams(1920,1080,x,y);
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
        var popWidth = 702;
        var popHeight = 630;
        x = 1250 / 2;
        y = 520 / 2;
        SetPosition(x, y);
    }

    private void Update()
    {
        var mousePos = Input.mousePosition; 
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //SetPosition(Mathf.Clamp((int)(mousePos.x * 1),0, 1920), Mathf.Clamp((int)(mousePos.y / 2f), 0, 520)); // 크기 조절
        SetPosition(x,y); // 크기 조절
        //text.text = string.Format((int)mousePos.x + " " + Mathf.Clamp((int)mousePos.y / 2 > 0 ? -(int)mousePos.y / 2 : (int)mousePos.y / 2, 0, 520)); // 텍스트로 알려줌
        int a = (int)Input.GetAxisRaw("Horizontal");
        int b = (int)Input.GetAxisRaw("Vertical");
        //x += a;
        //y += -b;
    }

    public override void Jump()
    {
        y += -50;
    }
    public override void Down()
    {
        y += 50;
    }
    public override void SizeUp()
    {

    }
}