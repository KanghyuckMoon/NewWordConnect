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

    private void Start()
    {
        saveuser = SaveManager.Instance.saveUserData1;
        wordManager = FindObjectOfType<WordManager>();
    }

    public void GetItem()
    {
        switch(Type)
        {
            case whatWord.subject:
                if (saveuser.subjectGet[wordsIndex]) return;
                else
                {
                    saveuser.subjectGet[wordsIndex] = true;
                    wordManager.AddSubjectWord(wordsIndex);
                    // 단어 추가
                }
                break;
            case whatWord.condition:
                if (saveuser.conditionGet[wordsIndex]) return;
                else
                {
                    saveuser.conditionGet[wordsIndex] = true;
                    wordManager.AddConditionWord(wordsIndex);
                    // 단어 추가
                }
                break;
            case whatWord.execution:
                if (saveuser.executionGet[wordsIndex]) return;
                else
                {
                    saveuser.executionGet[wordsIndex] = true;
                    wordManager.AddExecutionWord(wordsIndex);
                    // 단어 추가
                }
                break;
        }
    }
}
