using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{

    private string Save_Path = "";
    private string Save_FileName1 = "/SaveFile1.txt";
    private string Save_FileName2 = "/SaveFile2.txt";
    private string Save_FileName3 = "/SaveFile3.txt";

    private SaveUser saveUser1;
    private SaveUser saveUser2;
    private SaveUser saveUser3;
    private int nowSelectData = 0;
    public SaveUser CurrentSaveUser
    {
        get
        {
            switch(nowSelectData)
            {
                case 1:
                    return saveUser1;
                case 2:
                    return saveUser2;
                case 3:
                    return saveUser3;
                default:
                    Debug.LogWarning("[Instance] Instance " + typeof(SaveManager) + " is already destroyed. Returning null.");
                    return null;
            }
        }
    }


    private void Awake()
    {
        Save_Path = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
    }

    public void SetSaveUserData(int index)
    {
        nowSelectData = index;
    }

    public void ResetSaveUserData()
    {
        nowSelectData = 0;
    }
}
