using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        meshAgent.speed = unitInfo.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        UnitMove();
    }
}
