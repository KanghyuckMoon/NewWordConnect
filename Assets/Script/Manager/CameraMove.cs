using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraMove : WordGameObject
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer virtualCameraTrans;
    private Vector2 position1;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(0.05f); 
    public bool downmoveOn = false;
    private LookCamera lookCamera = null;
    private Vector3 lockPosition = new Vector3(1,10,0);
    private Vector3 NotMovePosition;
    private bool downon;
    private float shaketimer = 0;
    private float camerasize = 3.75f;
    private float scaleUpDownSize = 1f;

    protected override void Start()
    {
        //base.Start();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        lookCamera = GetComponent<LookCamera>();
        StartCoroutine(JumpCam());
        StartCoroutine(OnMoveDetect());
        SetPlayer();
    }


    //public override void Setting()
    //{
    //    speed = user.speed;
    //    maxSpeed = user.maxspeed;
    //    friction = user.friction;
    //    airfriction = user.aitfriction;
    //    downGravityOn = user.downGravityOn;
    //    gravityScale = 0;
    //    jump = user.jump;

    //    rigid.drag = friction;
    //    rigid.gravityScale = 0;
    //}

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
                downon = false;
            }
            if (transform.position != NotMovePosition)
            {
                if(NotMovePosition.y - 1 > transform.position.y)
                {
                    downon = true;
                }
                else
                {
                    downon = false;
                }
                NotMovePosition = transform.position;
                w_MoveOn = true;
                w_Movetime = 0f;
            }

            yield return waitForSeconds;
        }

    }


    public override void Jump()
    {
        virtualCameraTrans.m_SoftZoneHeight = 2f;
        virtualCameraTrans.m_BiasY = -0.36f;
        jumpOn = true;
        w_MoveOn = true;
        w_MoveOnEffect = false;
        w_tile = 0;
        PlaySound();
    }
    public override void Down()
    {
        w_MoveOn = true;
        w_MoveOnEffect = false;
        downmoveOn = true;
        PlaySound();
    }
    private void FixedUpdate()
    {
        if (shaketimer > 0)
        {
            shaketimer -= Time.deltaTime;
            if (shaketimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
            if (transform.eulerAngles.x != 0 || transform.eulerAngles.y != 0 || transform.eulerAngles.z != 0)
            {
                transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            }
        }
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
                    yield return WaitForSeconds;
                }
                jumpOn = false;
                virtualCameraTrans.m_TrackedObjectOffset.y = jump / (friction * 0.5f);
                virtualCameraTrans.m_SoftZoneHeight = 2f;
                virtualCameraTrans.m_BiasY = -0.36f;
            }
            if(downmoveOn)
            {
                for (float i = 0; i < 11; i++)
                {
                    virtualCameraTrans.m_TrackedObjectOffset.y = Mathf.Lerp(0, -(jump / (friction * 0.5f)), -(i / 10));
                    yield return WaitForSeconds;
                }
                downmoveOn = false;
                virtualCameraTrans.m_TrackedObjectOffset.y = -(jump / (friction * 0.5f));
                virtualCameraTrans.m_SoftZoneHeight = 2f;
                virtualCameraTrans.m_BiasY = 0.36f;
            }
            else if((!jumpOn && !downmoveOn) && virtualCameraTrans.m_TrackedObjectOffset.y != 0)
            {
                for (float i = 0; i < 20; i++)
                {
                    virtualCameraTrans.m_TrackedObjectOffset.y = Mathf.Lerp(virtualCameraTrans.m_TrackedObjectOffset.y, 0, i / 20);
                    virtualCameraTrans.m_BiasY = Mathf.Lerp(virtualCameraTrans.m_BiasY, 0, i / 20);
                    virtualCameraTrans.m_SoftZoneHeight = Mathf.Lerp(virtualCameraTrans.m_SoftZoneHeight, 0.5f, i / 20);
                    yield return WaitForSeconds;
                }
                virtualCameraTrans.m_TrackedObjectOffset.y = 0;
                virtualCameraTrans.m_SoftZoneHeight = 0.5f;
                virtualCameraTrans.m_BiasY = 0;
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
            scaleUpDownSize = 1.2f;
            ChangeCameraSize();
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 2;
            scaleUpDownSize = 1.4f;
            ChangeCameraSize();
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = 0;
            scaleUpDownSize = 1f;
            ChangeCameraSize();
        }
        else if (sizeIndex == -2)
        {
            sizeIndex = -1;
            scaleUpDownSize = 0.8f;
            ChangeCameraSize();
        }

    }

    public override void SizeDown()
    {
        if (sizeIndex == 2)
        {
            sizeIndex = 1;
            scaleUpDownSize = 1.2f;
            ChangeCameraSize();
        }
        else if (sizeIndex == 1)
        {
            sizeIndex = 0;
            scaleUpDownSize = 1f;
            ChangeCameraSize();
        }
        else if (sizeIndex == 0)
        {
            sizeIndex = -1;
            scaleUpDownSize = 0.8f;
            ChangeCameraSize();
        }
        else if (sizeIndex == -1)
        {
            sizeIndex = -2;
            scaleUpDownSize = 0.6f; 
            ChangeCameraSize();
        }
    }

    private void ChangeCameraSize()
    {
        virtualCamera.m_Lens.OrthographicSize = camerasize * scaleUpDownSize;
    }

    private void ResetCam()
    {
        virtualCameraTrans.m_TrackedObjectOffset.y = 0;
        virtualCameraTrans.m_SoftZoneHeight = 0.5f;
        virtualCameraTrans.m_BiasY = 0;
    }

    public override void SpeedUp()
    {
        virtualCameraTrans.m_XDamping = 0;
        virtualCameraTrans.m_YDamping = 0;
        PlaySound();
        Invoke("SpeedReset", 1);
    }
    public override void SpeedDown()
    {
        virtualCameraTrans.m_XDamping = 2;
        virtualCameraTrans.m_YDamping = 2;
        PlaySound();
        Invoke("SpeedReset", 1);
    }
    public override void SpeedStop()
    {
        virtualCamera.Follow = null;
        PlaySound();
        Invoke("SpeedReset", 1);
    }
    public override void SpeedReset()
    {
        virtualCamera.Follow = player.transform;
        virtualCameraTrans.m_XDamping = 0.5f;
        virtualCameraTrans.m_YDamping = 0.5f;
    }
    public override float ReturnVelocityY()
    {
        return downon ? -4 : 0;
    }

    public void Shakecam(float power, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = power;
        shaketimer = time;
    }

    public void SetCameraSize(float size)
    {
        camerasize = size;
        ChangeCameraSize();
    }
}
