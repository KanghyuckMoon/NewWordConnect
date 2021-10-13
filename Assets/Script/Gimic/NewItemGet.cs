using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemGet : MonoBehaviour
{
    enum whatWord
    {
        subject,
        condition,
        execution
    }
    [SerializeField]
    private whatWord Type;
    [SerializeField]
    private int wordsIndex;
    private SaveUser saveuser;
    private WordManager wordManager;
    [SerializeField]
    private Transform right;
    private Vector3 onrotation;

    private void Start()
    {
        saveuser = SaveManager.Instance.saveUserData1;
        wordManager = FindObjectOfType<WordManager>();
        StartCoroutine(RotateRight());
    }

    private IEnumerator RotateRight()
    {
        onrotation = right.eulerAngles;
        while (true)
        {
            onrotation.z += 1;
            right.eulerAngles = onrotation;
            yield return null;
        }
    }

    public void GetItem()
    {
        switch(Type)
        {
            case whatWord.subject:
                if (FindGetBool(saveuser.subjectGet)) break;
                else
                {
                    wordManager.AddSubjectWord(wordsIndex);
                    // 단어 추가
                }
                break;
            case whatWord.condition:
                if (FindGetBool(saveuser.conditionGet)) break;
                else
                {
                    wordManager.AddConditionWord(wordsIndex);
                    // 단어 추가
                }
                break;
            case whatWord.execution:
                if (FindGetBool(saveuser.executionGet)) break;
                else
                {
                    wordManager.AddExecutionWord(wordsIndex);
                    // 단어 추가
                }
                break;
        }
        gameObject.SetActive(false);
    }

    private bool FindGetBool(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == wordsIndex)
            {
                return true;
            }
        }
        return false;
    }

}
