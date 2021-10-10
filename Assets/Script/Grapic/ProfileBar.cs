using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileBar : MonoBehaviour
{
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text DateText;
    [SerializeField]
    private Image ProfileImage;

    [SerializeField]
    private Sprite[] profileSprites;

    private SaveUser data;
    private SaveManager saveManager;

    [SerializeField]
    private int dateindex;

    private void Start()
    {
        Invoke("SetProfile", 0.1f);
    }

    public void SetProfile()
    {
        saveManager = SaveManager.Instance;
        switch(dateindex)
        {
            case 1:
                data = saveManager.saveUserData1;
                break;
            case 2:
                data = saveManager.saveUserData2;
                break;
            case 3:
                data = saveManager.saveUserData3;
                break;
            default:
                Debug.LogWarning("[Instance] Instance " + typeof(ProfileBar) + "몇 번째 세이브인지 체크해주세요");
                break;
        }

        if(data.writingData)
        {
            NameText.text = data.playerName;
            DateText.text = data.lateDate;
            ProfileImage.sprite = profileSprites[1];
            ProfileImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        }
        else
        {
            NameText.text = "파일이 없습니다";
            DateText.text = "";
            ProfileImage.sprite = profileSprites[0];
            ProfileImage.SetNativeSize();
        }
    }
}
