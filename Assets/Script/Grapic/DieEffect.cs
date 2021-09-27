using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
    private TextMesh[] texts = new TextMesh[3];
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);

    private void Awake()
    {
        for(int i = 0; i < 3; i++)
        {
        texts[i] = transform.GetChild(i).GetComponent<TextMesh>();
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < 3; i++)
        {
        texts[i].transform.position = transform.position;
        }
        StartCoroutine(DieEffectCoroutine());
    }

    private IEnumerator DieEffectCoroutine()
    {
        Vector3[] textvector = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            textvector[i] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f,1f), 0);
        }
        for (int i = 0; i < 30; i++)
        {
            texts[0].transform.Translate(textvector[0] * 0.05f);
            texts[0].color = new Color(0, 0, 0, (30f  - i) / 30);
            texts[1].transform.Translate(textvector[1] * 0.025f);
            texts[1].color = new Color(0, 0, 0, (30f - i) / 30);
            texts[2].transform.Translate(textvector[2] * 0.05f);
            texts[2].color = new Color(0, 0, 0, (30f - i) / 30);
            yield return waitForSeconds;
        }
        texts[0].color = new Color(0, 0, 0, 0);
        texts[1].color = new Color(0, 0, 0, 0);
        texts[2].color = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
        yield return null;
    }
}
