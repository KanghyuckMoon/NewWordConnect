using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int panelCount = 0;

    [SerializeField]
    private GameObject scrollbar;

    [SerializeField]
    private List<Image> images; //키캡
    [SerializeField]
    private List<Text> texts; //텍스트
    [SerializeField]
    private GameObject panel; //패널


    private void Start()
    {
        SetListUI();
        SetSizeListUI();
    }

    private void Update()
    {
        CountPanel();
    }

    public void CreatePanel()
    {
        GameObject newPanel = null;
        newPanel = Instantiate(panel, panel.transform.parent);
        SetListUI();
        SetSizeListUI();
    }
    public void BackPanel()
    {
        if (panelCount == 1) return;
        GameObject backPanel = null;
        backPanel = scrollbar.transform.GetChild(panelCount - 1).gameObject;
        Destroy(backPanel);
        SetListUI();
        SetSizeListUI();
        images.RemoveAt(images.Count - 1);
        texts.RemoveAt(texts.Count - 1);
    }
    public void ClearPanel()
    {
        int count = panelCount;
        for (int i = 1; i<count;i++)
        {
            Destroy(scrollbar.transform.GetChild(i).gameObject);
            images.RemoveAt(images.Count - 1);
            texts.RemoveAt(texts.Count - 1);
        }
        SetSizeListUI();
    }

    private void CountPanel()
    {
        panelCount = scrollbar.transform.childCount;

    }

    private void SetListUI()
    {
        CountPanel();
        SetListImage();
        SetListText();
    }

    private void SetListImage()
    {
        images.Clear();
        for (int i = 0; i < scrollbar.transform.childCount; i++)
        {
            images.Add(scrollbar.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<Image>());
        }
    }
    private void SetListText()
    {
        texts.Clear();
        for (int i = 0; i < scrollbar.transform.childCount; i++)
        {
            texts.Add(scrollbar.transform.GetChild(i).transform.GetChild(1).transform.GetComponent<Text>());
        }
    }

    private void SetSizeListUI()
    {
        SetSizeListImage();
        SetSizeListText();
    }

    private void SetSizeListImage()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (images.Count == 1)
            {
                images[i].rectTransform.sizeDelta = new Vector2(100, 100);
                return;
            }
            if (images.Count == 0)
            {
                return;
            }
            images[i].rectTransform.sizeDelta =
                new Vector2(100 / (images.Count * 0.5f),
                100 / (images.Count * 0.5f));
        }
    }
    private void SetSizeListText()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            if (texts.Count == 1)
            {
                texts[i].rectTransform.sizeDelta =
                       new Vector2(640 / texts.Count,
                       texts[i].rectTransform.rect.height);
                texts[i].fontSize = 60;
                return;
            }
            texts[i].rectTransform.sizeDelta =
                   new Vector2(640 / texts.Count,
                   texts[i].rectTransform.rect.height);
            texts[i].fontSize = OuttoFontSize();
        }
    }

    private int OuttoFontSize()
    {
        switch(panelCount)
        {
            case 1:
                return 60;
            case 2:
                return 50;
            case 3:
                return 40;
            case 4:
                return 35;
            case 5:
                return 30;
            case 6:
                return 26;
            case 7:
                return 24;
            case 8:
                return 22;
            case 9:
                return 20;
            default:
                return 20;
        }
    }

}
