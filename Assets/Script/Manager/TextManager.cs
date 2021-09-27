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
        ������,
        ���ؿ�,
        ��ÿ�,
        ����,
        ��������
    }

    [SerializeField]
    private Text nameText = null;
    [SerializeField]
    private Text chatText = null;
    [SerializeField]
    private Image characterImage = null;
    [SerializeField]
    private GameObject endcousur;

    [SerializeField]
    private Sprite[] characterSprite;

    public MyText[] textlist;
    //
    private int count = 0;
    private int textIndex = -1;
    private int index = 3;
    private float delay = 0.05f;
    private bool chatend;

    public void Chatting()
    {
        endcousur.SetActive(false);
        chatend = false;
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
        Invoke("Chatting", 1);
        chatText.text = null;
        //Chatting();
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
            endcousur.SetActive(true);
            chatend = true;
            return;
        }
        chatText.text += textlist[textIndex].data[count][index];
        index++;
        Invoke("ChatLoading", delay);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(chatend)
            {
            Chatting();
            }
        }
    }
}
