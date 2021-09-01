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
    private List<Image> images;
    [SerializeField]
    private GameObject panel;


    private void Start()
    {
        SetListImages();
        SetSizeListImages();
    }

    private void Update()
    {
        CountPanel();
    }

    public void CreatePanel()
    {
        GameObject newPanel = null;
        newPanel = Instantiate(panel, panel.transform.parent);
        SetListImages();
        SetSizeListImages();
    }
    public void BackPanel()
    {
        if (panelCount == 1) return;
        GameObject backPanel = null;
        backPanel = scrollbar.transform.GetChild(panelCount - 1).gameObject;
        Destroy(backPanel);
        SetListImages();
        SetSizeListImages();
        images.RemoveAt(images.Count - 1);
    }
    public void ClearPanel()
    {
        int count = panelCount;
        for (int i = 1; i<count;i++)
        {
            Destroy(scrollbar.transform.GetChild(i).gameObject);
            images.RemoveAt(images.Count - 1);
        }
        SetSizeListImages();
    }

    private void CountPanel()
    {
        panelCount = scrollbar.transform.childCount;

    }

    private void SetListImages()
    {
        CountPanel();
        images.Clear();
        for(int i = 0; i <scrollbar.transform.childCount;i++)
        {
            images.Add(scrollbar.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<Image>());
        }
    }

    private void SetSizeListImages()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (images.Count == 1)
            {
                images[i].rectTransform.sizeDelta = new Vector2(100,100);
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

    
}
