using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : WordGameObject
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer virtualCameraTrans;
    private Vector2 position1;
    private bool jumpOn;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(0.05f);

    protected override void Start()
    {
        base.Start();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        LoadToJson();
        Setting();
        StartCoroutine(JumpCam());
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
        virtualCameraTrans.m_SoftZoneHeight = 2f;
        virtualCameraTrans.m_BiasY = -0.36f;
        jumpOn = true;
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
        Invoke("ResetCam", 1f);
    }

    protected IEnumerator JumpCam()
    {
        while(true)
        {
            if(jumpOn)
            {
                for(float i = 0; i < 10; i++)
                {
                    virtualCameraTrans.m_TrackedObjectOffset.y = Mathf.Lerp(0, jump / (friction * 0.5f), i / 10);
                    yield return WaitForSeconds;
                }
                jumpOn = false;
            }
            yield return null;
        }
    }

    private void ResetCam()
    {
        virtualCameraTrans.m_TrackedObjectOffset.y = 0;
        virtualCameraTrans.m_SoftZoneHeight = 0.5f;
        virtualCameraTrans.m_BiasY = 0;
    }


}
