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
    private int index = 0;

    public void Chatting()
    {
        if (textlist[textIndex].data.Length <= count)
        {
            count = 0;
            return;
        }
        else
        {
            CharacterImageSet();
            count++;
        }
    }
    
    public void ChatStart(int textindex)
    {
        textIndex = textindex;
        Chatting();
    }

    private void CharacterImageSet()
    {
        int a = int.Parse(textlist[textIndex].data[count].Substring(0, 3));
        //characterImage.sprite = characterSprite[a];
    }

    private void ChatLoading()
    {
        if(index >= textlist[textIndex].data[count].Length)
        {
            Invoke("Chatting", 1);
        }
        chatText.text = "";
        chatText.text += textlist[textIndex].data[count][index];
        index++;
        Invoke("ChatLoading", 1);
    }
}
