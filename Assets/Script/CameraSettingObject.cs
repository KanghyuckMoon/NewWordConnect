using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSettingObject : MonoBehaviour
{

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
        if(cameraLock)
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = lockTransform;
        }
        else
        {
            mainCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
        }
    }
}
