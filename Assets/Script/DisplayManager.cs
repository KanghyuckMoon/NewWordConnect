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
        SetPosition(x, y);
    }

    protected override void Start()
    {

    }

    private void Update()
    {
        SetPosition(x,y + ay); // 크기 조절
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

    }
}