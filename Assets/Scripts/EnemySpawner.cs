using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    private bool TimeLess;
    void Start()
    {
         InvokeRepeating("CreateUnits",5,15); // 5 20
    }

    private void Update()
    {
        if(GameController.Instance.leftTime<=60 && !TimeLess)
        {
            InvokeRepeating("CreateUnits",1,12); // 5 20
            TimeLess = true;
        }
    }

    private void CreateUnits()
    {
        GameController.Instance.CreateUnit(Random.Range(1,8),transform.position,false);
    }
}
