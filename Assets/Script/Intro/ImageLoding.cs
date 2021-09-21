using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Image,
    Sprite,
    Text
};

[System.Serializable]
public class FadeObject
{
    public Type type;
    public float lodingAmount;
    public float endAmount;
    public float delayTime;
    public float holdingTime;
    public GameObject fadeObj;
}

public class ImageLoding : MonoBehaviour
{
    [SerializeField]
    private List<FadeObject> fadeObjects = new List<FadeObject>();
    private WaitForSeconds waitForSecondsDelay;
    private int orderindex = 0;
    private bool endfade = false;

    public bool GetEnd()
    {
        return endfade;
    }

    private void Start()
    {
        OrderDraw();
    }

    private void OrderDraw()
    {
        if (orderindex >= fadeObjects.Count)
        {
            endfade = true;
            return;
        }
        waitForSecondsDelay = new WaitForSeconds(fadeObjects[orderindex].delayTime);
        switch (fadeObjects[orderindex].type)
        {
            case Type.Image:
                StartCoroutine(FadeInOutImage(orderindex));
                break;
            case Type.Sprite:
                StartCoroutine(FadeInOutSprite(orderindex));
                break;
            case Type.Text:
                StartCoroutine(FadeInOutText(orderindex));
                break;
        }
        orderindex++;
    }
    
    private IEnumerator FadeInOutImage(int index)
    {
        fadeObjects[index].fadeObj.SetActive(true);
        float a = fadeObjects[index].lodingAmount * 0.016f;
        float b = fadeObjects[index].endAmount * 0.016f;
        Image obj = fadeObjects[index].fadeObj.GetComponent<Image>();
        for(float i = 0; i<=1;i+=a)
        {
            obj.color = new Color(1, 1, 1,i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeObjects[index].holdingTime);
        for (float i = 1; i >= 0; i -= b)
        {
            obj.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 0);
        fadeObjects[index].fadeObj.SetActive(false);
        OrderDraw();
        yield return null;
    }

    private IEnumerator FadeInOutSprite(int index)
    {
        fadeObjects[index].fadeObj.SetActive(true);
        float a = fadeObjects[index].lodingAmount * 0.016f;
        float b = fadeObjects[index].endAmount * 0.016f;
        SpriteRenderer obj = fadeObjects[index].fadeObj.GetComponent<SpriteRenderer>();
        for (float i = 0; i <= 1; i += a)
        {
            obj.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeObjects[index].holdingTime);
        for (float i = 1; i >= 0; i -= b)
        {
            obj.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 0);
        fadeObjects[index].fadeObj.SetActive(false);
        OrderDraw();
        yield return null;
    }

    private IEnumerator FadeInOutText(int index)
    {
        fadeObjects[index].fadeObj.SetActive(true);
        float a = fadeObjects[index].lodingAmount * 0.016f;
        float b = fadeObjects[index].endAmount * 0.016f;
        Text obj = fadeObjects[index].fadeObj.GetComponent<Text>();
        for (float i = 0; i <= 1; i += a)
        {
            obj.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeObjects[index].holdingTime);
        for (float i = 1; i >= 0; i -= b)
        {
            obj.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 0);
        fadeObjects[index].fadeObj.SetActive(false);
        OrderDraw();
        yield return null;
    }
}
