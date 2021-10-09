using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Type
{
    Image,
    Sprite,
    Text,
    ImageText
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
    private WaitForSeconds waitTextDelay;
    private int orderindex = 0;
    private bool endfade = false;
    private string tempText = "";
    [SerializeField]
    private bool NextSceneOn;
    [SerializeField]
    private string NextSceneName;

    public bool GetEnd()
    {
        return endfade;
    }

    private void Start()
    {
        waitTextDelay = new WaitForSeconds(0.1f);
        OrderDraw();
    }

    private void OrderDraw()
    {
        if (orderindex >= fadeObjects.Count)
        {
            endfade = true;
            if (NextSceneOn)
            {
                SceneManager.LoadScene(NextSceneName);
            }

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
            case Type.ImageText:
                StartCoroutine(FadeInOutImageText(orderindex));
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

    private IEnumerator FadeInOutImageText(int index)
    {
        Text obj = fadeObjects[index].fadeObj.GetComponentInChildren<Text>();
        tempText = obj.text;
        obj.text = "";
        fadeObjects[index].fadeObj.SetActive(true);
        float a = fadeObjects[index].lodingAmount * 0.016f;
        float b = fadeObjects[index].endAmount * 0.016f;
        Image obj2 = fadeObjects[index].fadeObj.GetComponentInChildren<Image>();
        obj.color = new Color(1, 1, 1, 1);
        for (float i = 0; i <= 1; i += a)
        {
            obj2.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }

        for(int i = 0; i <= tempText.Length - 1; i++)
        {
            obj.text += tempText[i];
            yield return waitTextDelay;
        }

        obj2.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeObjects[index].holdingTime);
        for (float i = 1; i >= 0; i -= b)
        {
            obj.color = new Color(1, 1, 1, i);
            obj2.color = new Color(1, 1, 1, i);
            yield return waitForSecondsDelay;
        }
        obj.color = new Color(1, 1, 1, 0);
        obj2.color = new Color(1, 1, 1, 0);
        fadeObjects[index].fadeObj.SetActive(false);
        OrderDraw();
        yield return null;
    }
}
