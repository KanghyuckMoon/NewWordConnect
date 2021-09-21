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
    [SerializeField]
    private Transform savePoint = null;
    private Transform playerTransform = null;
    [SerializeField]
    private int setAreanum = 0;

    private Camera mainCamera = null;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    public int SetCameraMoveSetting()
    {
        playerTransform.GetComponent<PlayerMove>().SetSavePoint(savePoint.position);
        if(horizontalOn)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = horizontalObj;
            return setAreanum;
        }
        if(verticalOn)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = verticalObj;
            return setAreanum;
        }
        if(cameraLock)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = lockTransform;
            return setAreanum;
        }
        else
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
            return setAreanum;
        }
    }
}
