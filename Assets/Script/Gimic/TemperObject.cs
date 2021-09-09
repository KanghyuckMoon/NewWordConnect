using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperObject : MonoBehaviour
{
    private TemperentManager settingManager;

    [SerializeField]
    private int destroyTemper = 1;

    void Start()
    {
        settingManager = FindObjectOfType<TemperentManager>();
    }

    private void Update()
    {
        TemperDestroy();

    }

    private void TemperDestroy()
    {
        if (settingManager.tempdan == destroyTemper) Destroy(gameObject);
    }
}
