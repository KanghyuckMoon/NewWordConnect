using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadScene("UIword", LoadSceneMode.Additive);
    }
}
