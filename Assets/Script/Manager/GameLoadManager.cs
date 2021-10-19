using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        LoadUI();
    }

    private void Update()
    {
    }

    private void LoadUI()
    {
        SceneManager.LoadScene("UIword", LoadSceneMode.Additive);
    }
}
