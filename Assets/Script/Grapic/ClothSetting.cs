using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSetting : MonoBehaviour
{
    private List<Transform> capeTransform = new List<Transform>();
    private List<Vector2>  capePosition = new List<Vector2>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            capeTransform.Add(transform.GetChild(i));
            capePosition.Add(transform.GetChild(i).localPosition);
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            capeTransform[i].localPosition = capePosition[i];
        }
    }
}
