using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : WordGameObject
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer virtualCameraTrans;
    private Vector2 position1;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(0.05f); 
    public bool jumpmoveOn = false;
    private LookCamera lookCamera = null;
    private Vector3 lockPosition = new Vector3(1,10,0);


    protected override void Start()
    {
        base.Start();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        lookCamera = GetComponent<LookCamera>();
        LoadToJson();
        Setting();

        //state = virtualCamera.GetComponent<CinemachineFramingTransposer>().VirtualCamera.State;
        //state = virtualCamera.GetComponent<CinemachineFramingTransposer>().VcamState;
        //state = GetComponent<CinemachineExtension>().VirtualCamera.State;
        //state = GetComponent<CinemachineBrain>().CurrentCameraState;
        //state = virtualCamera.State;
        //GetComponent<CinemachineVirtualCamera>().cor
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
        jumpmoveOn = true;
        //Invoke("ResetCam", 1f);
    }
    private void LateUpdate()
    {

    }
    protected IEnumerator JumpCam()
    {
        while(true)
        {
            if(jumpOn)
            {
                for(float i = 0; i < 11; i++)
                {
                    virtualCameraTrans.m_TrackedObjectOffset.y = Mathf.Lerp(0, jump / (friction * 0.5f), i / 10);
                    //state.RawPosition = new Vector3(lockPosition.x, lockPosition.y + virtualCameraTrans.m_TrackedObjectOffset.y, lockPosition.z);
                    yield return WaitForSeconds;
                }
                jumpOn = false;
                virtualCameraTrans.m_TrackedObjectOffset.y = jump / (friction * 0.5f);
                virtualCameraTrans.m_SoftZoneHeight = 2f;
                virtualCameraTrans.m_BiasY = -0.36f;
            }
            else if(!jumpOn && virtualCameraTrans.m_TrackedObjectOffset.y != 0)
            {
                for (float i = 0; i < 20; i++)
                {
                    virtualCameraTrans.m_TrackedObjectOffset.y = Mathf.Lerp(virtualCameraTrans.m_TrackedObjectOffset.y, 0, i / 20);
                    /*lookCamera.rea_lm_Position = lookCamera.m_ZPosition + virtualCameraTrans.m_TrackedObjectOffset.y*/;
                    //state.RawPosition = lockPosition;
                    virtualCameraTrans.m_BiasY = Mathf.Lerp(-0.36f, 0, i / 20);
                    virtualCameraTrans.m_SoftZoneHeight = Mathf.Lerp(2, 0.5f, i / 20);
                    yield return WaitForSeconds;
                }
                virtualCameraTrans.m_TrackedObjectOffset.y = 0;
                virtualCameraTrans.m_SoftZoneHeight = 0.5f;
                virtualCameraTrans.m_BiasY = 0;
                jumpmoveOn = false;
            }
            yield return null;
        }
    }

    //protected void PostPipelineStageCallback(
    //    CinemachineVirtualCameraBase vcam,
    //    CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    //{
    //    pos = state.RawPosition;
    //    if (stage == CinemachineCore.Stage.Body)
    //    {
    //        rea_lm_Position = m_ZPosition + virtualCameraTrans.m_TrackedObjectOffset.y;
    //        pos.y = rea_lm_Position;
    //        state.RawPosition = pos;
    //    }
    //}

    public override void SizeUp()
    {
        if (sizeIndex == 0)
        {
            sizeIndex = 1;
            virtualCamera.m_Lens.OrthographicSize = 4.5f;
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            virtualCamera.m_Lens.OrthographicSize = 5.25f;
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            virtualCamera.m_Lens.OrthographicSize = 3.75f;
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            virtualCamera.m_Lens.OrthographicSize = 3;
        }

    }

    public override void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            virtualCamera.m_Lens.OrthographicSize = 4.5f;
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            virtualCamera.m_Lens.OrthographicSize = 3.75f;
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            virtualCamera.m_Lens.OrthographicSize = 3;
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            virtualCamera.m_Lens.OrthographicSize = 2.25f;
        }
    }

    private void ResetCam()
    {
        virtualCameraTrans.m_TrackedObjectOffset.y = 0;
        virtualCameraTrans.m_SoftZoneHeight = 0.5f;
        virtualCameraTrans.m_BiasY = 0;
    }


}
