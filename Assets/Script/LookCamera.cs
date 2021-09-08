using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LookCamera : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_ZPosition = 10;
    public float rea_lm_Position = 0;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer virtualCameraTrans;
    private CameraMove cameraMove;
    public Vector3 pos;
    private bool startref = false;

    private void Start()
    {
        cameraMove = GetComponent<CameraMove>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        rea_lm_Position = m_ZPosition;

    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        pos = state.RawPosition;
        if (stage == CinemachineCore.Stage.Body)
        {
            rea_lm_Position = m_ZPosition + virtualCameraTrans.m_TrackedObjectOffset.y;
                pos.y = rea_lm_Position;
                state.RawPosition = pos;
        }
    }
}
