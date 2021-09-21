using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class MyText
{
    public string[] data;
    public string this[int index] { get { return data[index]; } }
    public int Length { get { return data.Length; } }
}

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private WordManager wordManager;
    
    enum Name
    {
        ¹®°­Çõ,
        ¹ÚÇØ¿ï,
        ¾î½Ã¿Â,
        ¶òÈÆ,
        ¤±¤¤¤·¤©
    }

    [SerializeField]
    private Text nameText = null;
    [SerializeField]
    private Text chatText = null;
    [SerializeField]
    private Image characterImage = null;

    [SerializeField]
    private Sprite[] characterSprite;

    public MyText[] textlist;
    //
    private int count = 0;
    private int textIndex = -1;
    private int index = 3;

    public void Chatting()
    {
        if (textlist[textIndex].data.Length <= count)
        {
            count = 0;
            wordManager.EventOff();
            return;
        }
        else
        {
            CharacterImageSet();
            index = 3;
            chatText.text = "";
            ChatLoading();
        }
    }
    
    public void ChatStart(int textindex)
    {
        textIndex = textindex;
        wordManager.EventOn();
        Chatting();
    }

    private void CharacterImageSet()
    {
        Name a = (Name)int.Parse(textlist[textIndex].data[count].Substring(0, 3));
        nameText.text = a.ToString();
        //characterImage.sprite = characterSprite[a];
    }

    private void ChatLoading()
    {
        if(index >= textlist[textIndex].data[count].Length)
        {
            count++;
            Invoke("Chatting", 1);
            return;
        }
        Debug.Log(index + " , " + textlist[textIndex].data[count].Length);
        chatText.text += textlist[textIndex].data[count][index];
        index++;
        Invoke("ChatLoading", 0.2f);
    }
}
