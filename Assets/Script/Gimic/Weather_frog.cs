using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather_frog : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private GameObject monsterPref;
    [SerializeField]
    private float size;
    [SerializeField]
    private float y_Position;
    [SerializeField]
    private float spawndelay;

    private void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }



}
