using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private int panelCount = 0;

    [SerializeField]
    private GameObject scrollbar;

    [SerializeField]
    private List<Image> images;
    [SerializeField]
    private List<GameObject> game;

    private void Start()
    {
        SetListImages();
        SetSizeListImages();
    }

    private void Update()
    {
        CountPanel();
    }

    private void CountPanel()
    {
        panelCount = scrollbar.transform.childCount;

    }

    private void SetListImages()
    {
        for(int i = 0; i <scrollbar.transform.childCount;i++)
        {
            Debug.Log(scrollbar.transform.GetChild(i).transform.GetChild(0));
            game.Add(scrollbar.transform.GetChild(i).transform.GetChild(0).gameObject);
            images.Add(scrollbar.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<Image>());
        }
    }

    private void SetSizeListImages()
    {
        for(int i = 0; i < images.Count; i++)
        {
            if (images.Count == 1) return;
            if (images.Count == 0) return;
            images[i].rectTransform.sizeDelta = 
                new Vector2(images[i].rectTransform.rect.width / (images.Count * 0.5f),
                images[i].rectTransform.rect.height / (images.Count * 0.5f));

        }
    }
}
