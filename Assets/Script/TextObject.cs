using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : MonoBehaviour
{
    [SerializeField]
    private int textIndex = 0;

    public int ReturnTextIndex()
    {
        return textIndex;
    }
}
