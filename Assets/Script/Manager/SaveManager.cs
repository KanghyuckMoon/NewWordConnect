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
    private string Save_KeySettingFileName = "/KeySettingFile.txt";
    private string Now_Save_FileName = "";

    [SerializeField]
    private SaveUser saveUser1;
    public SaveUser saveUserData1
    {
        get
        {
            return saveUser1;
        }
    }

    [SerializeField]
    private SaveUser saveUser2;
    public SaveUser saveUserData2
    {
        get
        {
            return saveUser2;
        }
    }

    [SerializeField]
    private SaveUser saveUser3;
    public SaveUser saveUserData3
    {
        get
        {
            return saveUser3;
        }
    }

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
                    Debug.LogWarning("[Instance] Instance " + typeof(SaveManager) + "1~3번째 세이브 파일이 아닙니다");
                    return null;
            }
        }
    }

    [SerializeField]
    private KeySetting keysetting;
    public KeySetting CurrenKeySetting
    {
        get
        {
            return keysetting;
        }
    }


    private void Awake()
    {
        Save_Path = Application.dataPath + "/Save";
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
        LoadKeySetting();
    }

    public void SetSaveUserData(int index)
    {
        nowSelectData = index;
        switch(index)
        {
            case 1:
                Now_Save_FileName = Save_FileName1;
                break;
            case 2:
                Now_Save_FileName = Save_FileName2;
                break;
            case 3:
                Now_Save_FileName = Save_FileName3;
                break;
        }
        LoadToJson();
    }

    public void ResetSaveUserData()
    {
        nowSelectData = 0;
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(CurrentSaveUser, true);
        File.WriteAllText(Save_Path + Now_Save_FileName, json, System.Text.Encoding.UTF8);
    }

    private void LoadToJson()
    {
        if (File.Exists(Save_Path + Now_Save_FileName))
        {
            SaveUser data = CurrentSaveUser;
            string json = File.ReadAllText(Save_Path + Now_Save_FileName);
            data = JsonUtility.FromJson<SaveUser>(json);
        }
    }

    private void LoadKeySetting()
    {
        if (File.Exists(Save_Path + Now_Save_FileName))
        {
            string json = File.ReadAllText(Save_Path + Save_KeySettingFileName);
            keysetting = JsonUtility.FromJson<KeySetting>(json);
        }
    }

    private void SaveKeySetting()
    {
        string json = JsonUtility.ToJson(CurrenKeySetting, true);
        File.WriteAllText(Save_Path + Save_KeySettingFileName, json, System.Text.Encoding.UTF8);
    }
}
