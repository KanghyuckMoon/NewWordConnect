using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicButton : MonoBehaviour
{
    private bool onoff;
    [Header("���̶� �����ϼ�")]
    [SerializeField]
    private GimicDoor gimicDoor = null;

    private void OnCollisionStay2D(Collision2D collision)
    {
        onoff = true;
        gimicDoor.OnoffDoor(onoff);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onoff = false;
        gimicDoor.OnoffDoor(onoff);
    }
}
