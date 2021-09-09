using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSettingObject : MonoBehaviour
{
    [SerializeField]
    private Transform verticalObj;
    [SerializeField]
    private Transform horizontalObj;
    [SerializeField]
    private bool horizontalOn = false;
    [SerializeField]
    private bool verticalOn = false;
    [SerializeField]
    private bool cameraLock = false;
    [SerializeField]
    private Transform lockTransform = null;
    private Transform playerTransform = null;

    private Camera mainCamera = null;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    public void SetCameraMoveSetting()
    {
        if(horizontalOn)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = horizontalObj;
            return;
        }
        if(verticalOn)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = verticalObj;
            return;
        }
        if(cameraLock)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = lockTransform;
            return;
        }
        else
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
        }
    }
}
